using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class Coin : MonoBehaviour
    {
        int _coinDeleteTime = 1;
        float _coinDown = 0.01f;
        int _silverCoin = 20;
        int _goldCoin = 300;

        public bool _move = true;
        bool _floor = false;
        SpriteRenderer _sprRen;

        void Start()
        {
            _sprRen = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (_move && _floor == false)
                transform.Translate(0, -_coinDown, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Floor")
            {
                _floor = true;
                StartCoroutine(FadeAway());
            }
        }

        IEnumerator FadeAway()
        {
            yield return new WaitForSeconds(_coinDeleteTime);
            while (_sprRen.color.a > 0)
            {
                var color = _sprRen.color;
                color.a -= (.5f * Time.deltaTime);

                _sprRen.color = color;
                yield return null;
            }
            DeleteCoinList();
        }

        private void OnMouseDown()
        {
            if (gameObject.tag == "SilverCoin")
            {
                Aquarium.I._money += _silverCoin;
            }
            if (gameObject.tag == "GoldCoin")
            {
                Aquarium.I._money += _goldCoin;
            }
            DeleteCoinList();
        }

        void DeleteCoinList()
        {
            List<Coin> coli = Aquarium.I._coinList;

            for (int i = 0; i < coli.Count; i++)
            {
                if (coli[i] == this)
                {
                    coli.RemoveAt(i);
                    Destroy(gameObject);
                }
            }
        }
    }
}