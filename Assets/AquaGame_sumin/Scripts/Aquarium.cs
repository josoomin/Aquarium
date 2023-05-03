//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace josoomin
{
    public class Aquarium : MonoBehaviour
    {
        public static Aquarium I;

        public List<Fish> _fishList;
        public List<Food> _foodList;
        public List<Coin> _coinList;

        public GameObject _allFish;
        public GameObject _allFood;
        public GameObject _allCoin;

        public GameObject _clearScreen;

        public Guppy _tempGuppy;
        public Piranha _tempPiranha;

        public int _money;
        int _startMoney = 1000;
        int _foodPrice = 5;
        int _clearMoney = 10000;

        public Text _moneyText;

        public GameObject _foodPrefab;

        public RectTransform _noClickArea;

        public void Awake()
        {
            I = this;
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            _money = _startMoney;
            _clearScreen.SetActive(false);
        }

        void Update()
        {
            _moneyText.text =  _money.ToString();
            GameClear();
        }

        public void CreateFood()
        {
            if (Input.GetMouseButtonDown(0) && _money >= _foodPrice)
            {
                Vector3 mPosition = Input.mousePosition;
                Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);
                GameObject clonefood = Instantiate(_foodPrefab, target, Quaternion.identity);

                Vector3 newtarget = new Vector3(target.x, target.y, target.z + 1);
                clonefood.transform.position = newtarget;
                clonefood.transform.parent = _allFood.transform;

                Food food = clonefood.GetComponent<Food>();

                _foodList.Add(food);

                _money -= _foodPrice;
            }
        }

        void GameClear()
        {
            if (_money >= _clearMoney)
            {
                Time.timeScale = 0;
                _clearScreen.SetActive(true);

                if (Input.GetKey(KeyCode.R))
                {
                    SceneManager.LoadScene("AquaGame");
                }
            }
        }
    }
}