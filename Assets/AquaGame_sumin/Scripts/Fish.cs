using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace josoomin
{
    public class Fish : MonoBehaviour
    {
        public GameObject _food;
        float _eatMoveSpeed = 1;

        Vector2 _moveVector;

        [SerializeField] sbyte _moveX;
        [SerializeField] sbyte _moveY;
        [SerializeField] int _moveOrStop;
        [SerializeField] float _moveTime;
        [SerializeField] float _stayTime;
        [SerializeField] float _time = 0;
        [SerializeField] float _hungry = 100;
        [SerializeField] float _dropCoinTime = 0;
        [SerializeField] float _reTime = 0;
        [SerializeField] float _eatMoveTime;

        public GameObject _SilverCoin;
        public GameObject _GoldCoin;

        int _hungryLow = 70;
        int _hungryMiddle = 50;
        int _hungryHigh = 0;

        int _dropNowTime = 20;
        int _fishDeleteTime = 0;

        bool _eat;
        bool _die = false;

        Renderer _fishColor;

        float _minValue = 2.0f;
        float _maxValue = 10.0f;

        Rigidbody2D _myRigid;
        SpriteRenderer _rend;

        Animator _myani;

        void Start()
        {
            _myani = GetComponent<Animator>();
            _fishColor = GetComponent<Renderer>();
            _myRigid = GetComponent<Rigidbody2D>();
            _rend = GetComponent<SpriteRenderer>();
            moveRandom();
        }

        void Update()
        {
            _reTime += Time.deltaTime;
            if (_reTime >= _moveTime && _eat == false && _die == false)
            {
                moveRandom();
                _reTime = 0;
            }
            HungryGage();
            DropCoin();
        }

        public void moveRandom()
        {
            _moveTime = Random.Range(_minValue, _maxValue);

            _moveOrStop = Random.Range(0, 10);

            if (_die == false)
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

                    while (_stayTime >= _time)
                    {
                        _time += Time.deltaTime;
                        if (_time >= _stayTime)
                        {
                            _time = 0;
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
                if (_hungry <= _hungryMiddle && _die == false)
                {
                    _fishColor.material.color = Color.green;

                    if (_hungry <= _hungryHigh && _die == false)
                    {
                        gameObject.transform.position = transform.position;
                        _die = true;
                        _myani.SetTrigger("Die");
                        return;
                    }
                }
                else
                {
                    _fishColor.material.color = Color.white;
                    _eat = false;
                }
            }
        }

        void DropCoin()
        {
            _dropCoinTime += Time.deltaTime;

            if(_dropCoinTime >= _dropNowTime && _die == false)
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
            Debug.Log("¸ÔÀ¸·¯ ÀÌµ¿");
            _eat = true;

            SearchFood();

            if (_food != null && _die == false)
            {
                Vector3 direction = _food.transform.position - transform.position;
                direction.Normalize();
                _myRigid.AddForce(direction * _eatMoveSpeed, ForceMode2D.Force);

                if (direction.x >= 0)
                {
                    _rend.flipX = true;
                }
                else
                {
                    _rend.flipX = false;
                }

            }

            Vector2 pos1 = transform.position;
            Vector2 pos2 = _food.transform.position;
            Vector2 dir = pos1 - pos2;
            if (dir.magnitude < 0.5f)
            {
                Eat(_food);
            }
        }

        void SearchFood()
        {
            //if (gameObject.tag == "Guppy")
            //{
            //    _food = Aquarium.I._foodList[0].gameObject;
            //}

            //if (gameObject.tag == "Piranha")
            //{
            //    List<Fish> fili = Aquarium.I._fishList;
            //    if (fili == null) return;

            //    for (int i = 0; i <= fili.Count; i++)
            //    {
            //        if (fili[i].gameObject.tag == "Guppy")
            //        {
            //            _food = fili[i].gameObject;
            //        }
            //    }
            //}
        }

        void Eat(GameObject food)
        {
            Destroy(food);
            _hungry = 100;
        }

        IEnumerator Die()
        {
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
            Destroy(gameObject);
        }
    }
}