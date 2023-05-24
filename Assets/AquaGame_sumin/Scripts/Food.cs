using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Food : MonoBehaviour
    {
        int _foodDeleteTime = 0;
        float _foodDown = 0.01f;

        public bool _move = true;
        bool _floor = false;
        SpriteRenderer _sprRen;

        public GameObject _eatMe;

        void Start()
        {
            _sprRen = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (_move && _floor == false)
               transform.Translate(0, -_foodDown, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Floor")
            {
                _floor = true;
                StartCoroutine(Delete());
            }
        }

        IEnumerator Delete()
        {
            yield return new WaitForSeconds(_foodDeleteTime);
            while (_sprRen.color.a > 0)
            {
                var color = _sprRen.color;
                color.a -= (.5f * Time.deltaTime);

                _sprRen.color = color;
                yield return null;
            }

            List<Food> foli = Aquarium.I._foodList;

            for (int i = 0; i < foli.Count; i++)
            {
                if (foli[i] == this)
                {
                    EatMe();

                    foli.RemoveAt(i);
                    Destroy(gameObject);
                }
            }
        }

        void EatMe()
        {
            if (_eatMe == null) return;
            Fish _fish = _eatMe.GetComponent<Fish>();
            _fish._food = null;
        }
    }
}