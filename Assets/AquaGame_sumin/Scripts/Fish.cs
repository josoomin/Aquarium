using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace josoomin
{
    public class Fish : MonoBehaviour
    {
        public GameObject _food;

        Vector2 _moveVector;

        [SerializeField] sbyte _moveX;
        [SerializeField] sbyte _moveY;
        [SerializeField] int _moveOrStop;
        [SerializeField] float _moveTime;
        [SerializeField] float _stayTime;
        [SerializeField] float _hungry;
        [SerializeField] float _overTime = 0;
        [SerializeField] float _dropCoinTime = 0;
        [SerializeField] float _reTime = 0;
        [SerializeField] float _eatMoveTime;

        public GameObject _SilverCoin;
        public GameObject _GoldCoin;

        int _hungryLow = 20;
        int _hungryMiddle = 10;
        int _hungryHigh = 0;

        int _dropNowTime = 10;
        int _fishDeleteTime = 0;

        bool _eat = false;
        bool _die = false;

        float _minValue = 1.0f;
        float _maxValue = 2.0f;

        float _fullHungry = 25;

        float force = 10.0f; // 가할 힘의 크기
        float distanceThreshold = 3.0f; // 일정 거리 이하가 되면 속도를 감소시키는 거리

        Rigidbody2D _myRigid;
        SpriteRenderer _rend;

        [SerializeField] List<Guppy> guli = new List<Guppy>();

        Animator _myani;

        void Start()
        {
            _hungry = _fullHungry;
            _myani = GetComponent<Animator>();
            _myRigid = GetComponent<Rigidbody2D>();
            _rend = GetComponent<SpriteRenderer>();
            MoveRandom();
        }

        void Update()
        {
            _reTime += Time.deltaTime;
            if (_reTime >= _moveTime && _eat == false && _die == false)
            {
                MoveRandom();
                _reTime = 0;
            }

            HungryGage();
            DropCoin();


        }

        public void MoveRandom()
        {
            _moveTime = Random.Range(_minValue, _maxValue);

            _moveOrStop = Random.Range(0, 10);

            if (_die == false && _eat == false)
            {
                if (_moveOrStop % 2 == 0)
                {
                    _moveX = (sbyte)Random.Range(-50, 50);
                    _moveY = (sbyte)Random.Range(-50, 50);
                    _moveVector = new Vector2(_moveX, _moveY);

                    if (_moveX >= 0)
                    {
                        _rend.flipX = true;
                    }
                    else
                    {
                        _rend.flipX = false;
                    }

                    _myRigid.AddForce(_moveVector);
                }

                else if (_moveOrStop % 2 != 0)
                {
                    _stayTime = Random.Range(_minValue, _maxValue);
                    _myRigid.velocity = Vector3.zero;

                    while (_stayTime >= _overTime)
                    {
                        _overTime += Time.deltaTime;

                        if (_stayTime <= _overTime)
                        {
                            _overTime = 0;
                            break;
                        }
                    }
                }
            }
        }

        public void HungryGage()
        {
            if (_die == false) _hungry -= Time.deltaTime;

            if (_hungry <= _hungryLow && _die == false)
            {
                EatToMove();
                if (_hungry <= _hungryMiddle)
                {
                    _rend.color = Color.green;

                    if (_hungry <= _hungryHigh)
                    {
                        _rend.color = Color.white;
                        gameObject.transform.position = transform.position;
                        _die = true;
                        _myani.SetTrigger("Die");
                        return;
                    }
                }

                else
                {
                    _rend.color = Color.white;
                }
            }
        }

        void DropCoin()
        {
            _dropCoinTime += Time.deltaTime;

            if (_dropCoinTime >= _dropNowTime && _die == false)
            {
                if (gameObject.tag == "Guppy")
                {
                    CreateCoin(_SilverCoin);
                }

                if (gameObject.tag == "Piranha")
                {
                    CreateCoin(_GoldCoin);
                }
                _dropCoinTime = 0;
            }
        }

        void CreateCoin(GameObject CoinPre)
        {
            GameObject CloneCoin = Instantiate(CoinPre);

            Vector3 mypos = gameObject.transform.position;
            CloneCoin.transform.position = new Vector3(mypos.x, mypos.y, -9.5f);
            CloneCoin.transform.parent = Aquarium.I._allCoin.transform;

            Coin coin = CloneCoin.GetComponent<Coin>();

            Aquarium.I._coinList.Add(coin);
        }

        void EatToMove()
        {
            _eat = true;

            if (gameObject.tag == "Guppy" && _food == null && Aquarium.I._foodList.Count > 0)
            {
                SearchFood_Guppy();
            }

            if (gameObject.tag == "Piranha" && _food == null)
            {
                SearchFood_Piranha();
            }

            if (_food != null && _die == false)
            {
                Vector3 direction = (_food.transform.position - transform.position).normalized; // 방향 벡터 계산
                float distance = Vector3.Distance(transform.position, _food.transform.position); // 두 물체 간의 거리 계산

                // 두 물체가 일정 거리 이하일 경우
                if (distance < distanceThreshold)
                {
                    float reducedForce = force * (distance / distanceThreshold); // 힘의 크기 감소
                    _myRigid.AddForce(direction * reducedForce); // 감소된 힘으로 이동
                }
                else
                {
                    _myRigid.AddForce(direction * force); // 원래 힘으로 이동
                }

                if (direction.x >= 0)
                {
                    _rend.flipX = true;
                }
                else
                {
                    _rend.flipX = false;
                }

                Vector2 pos1 = transform.position;
                Vector2 pos2 = _food.transform.position;
                Vector2 dir = pos1 - pos2;

                if (dir.magnitude < 0.5f)
                {
                    if (_food == null && Aquarium.I._foodList.Count > 0)
                    {
                        SearchFood_Guppy();
                    }

                    if (_food == null)
                    {
                        SearchFood_Piranha();
                    }

                    else
                    {
                        Eat();
                    }
                }
            }
        }

        void SearchFood_Guppy()
        {
            List<Food> foodList = Aquarium.I._foodList;

            foodList.Sort(delegate (Food a, Food b)
            {
                return compare1(a, b);
            });

            for (int i = 0; i < foodList.Count; i++)
            {
                if (foodList[i] != null && _food == null)
                {
                    _food = foodList[i].gameObject;
                }

                else
                {
                    return;
                }
            }
        }

        void SearchFood_Piranha()
        {
            if (Aquarium.I._fishList != null)
            {
                List<Fish> fili = Aquarium.I._fishList;

                for (int i = 0; i < fili.Count; i++)
                {
                    if (fili[i].tag == "Guppy")
                    {
                        guli.Add(fili[i] as Guppy);
                    }
                }
            }

            guli.Sort(delegate (Guppy a, Guppy b)
            {
                return compare2(a, b);
            });

            for (int i = 0; i < guli.Count; i++)
            {
                if (guli[i] != null && _food == null)
                {
                    _food = guli[i].gameObject;
                }

                else
                {
                    return;
                }
            }
        }

        int compare1(Food a, Food b)
        {
            Vector3 _a = a.transform.position;
            Vector3 _b = b.transform.position;
            Vector3 _me = transform.position;

            float lengthAC = Vector3.Distance(_a, _me);
            float lengthBC = Vector3.Distance(_b, _me);

            return lengthAC < lengthBC ? -1 : 1;
        }

        int compare2(Guppy a, Guppy b)
        {
            if (a == null || b == null)
            {
                guli = new List<Guppy>();
                _food = null;
                return 0;
            }

            Vector3 _a = a.transform.position;
            Vector3 _b = b.transform.position;
            Vector3 _me = transform.position;

            float lengthAC = Vector3.Distance(_a, _me);
            float lengthBC = Vector3.Distance(_b, _me);

            return lengthAC < lengthBC ? -1 : 1;
        }

        void Eat()
        {
            _myani.SetTrigger("Eat");
            _hungry = _fullHungry;
            _eat = false;

            _myRigid.velocity = Vector3.zero;

            List<Food> foli = Aquarium.I._foodList;
            List<Fish> fili = Aquarium.I._fishList;

            if (gameObject.tag == "Guppy")
            {
                for (int i = 0; i < foli.Count; i++)
                {
                    if (foli[i].gameObject == _food)
                    {
                        foli.RemoveAt(i);
                        Destroy(_food);
                        _food = null;
                    }
                }
            }

            if (gameObject.tag == "Piranha")
            {
                for (int i = 0; i < fili.Count; i++)
                {
                    if (fili[i].gameObject == _food)
                    {
                        fili.RemoveAt(i);
                        Destroy(_food);
                        _food = null;
                    }
                }
                guli = new List<Guppy>();
            }
        }

        IEnumerator Die()
        {
            _myRigid.velocity = Vector3.zero;
            yield return new WaitForSeconds(_fishDeleteTime);

            while (_rend.color.a > 0)
            {
                var color = _rend.color;
                color.a -= (.5f * Time.deltaTime);

                _rend.color = color;
                yield return null;
            }

            List<Fish> fili = Aquarium.I._fishList;

            for (int i = 0; i < fili.Count; i++)
            {
                if (fili[i] == this)
                {
                    fili.RemoveAt(i);
                    Destroy(gameObject);
                }
            }
        }
    }
}