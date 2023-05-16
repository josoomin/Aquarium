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
        public GameObject _gameOverScreen;

        public GameObject _optionPage;

        public Text _guppyPriceText;
        public Text _piranhaPriceText;

        GameObject _foodArea;

        int _foodPrice = 5;

        public int _money;
        int _startMoney = 300;
        int _clearMoney = 5000;

        public Text _moneyText;

        public GameObject _foodPrefab;

        public void Awake()
        {
            I = this;
        }

        void Start()
        {
            Screen.SetResolution(1080, 720, true);
            Application.targetFrameRate = 60;
            _foodArea = transform.Find("FoodArea").gameObject;
            _optionPage.SetActive(false);
            _money = _startMoney;
            _clearScreen.SetActive(false);
            _gameOverScreen.SetActive(false);
            _guppyPriceText.text = UI_Button.I._guppyPrice.ToString();
            _piranhaPriceText.text = UI_Button.I._piranhaPrice.ToString();
        }

        void Update()
        {
            _moneyText.text = _money.ToString();
            GameClear();
            GameOver();
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
                    Restart();
                }
            }
        }

        void GameOver()
        {
            if (_money < 25 && _fishList.Count == 0)
            {
                Time.timeScale = 0;
                _gameOverScreen.SetActive(true);

                if (Input.GetKey(KeyCode.R))
                {
                    Restart();
                }
            }
        }

        public void Option()
        {
            Time.timeScale = 0;
            
            _foodArea.SetActive(false);
            _optionPage.SetActive(true);

            for (int i = 0; i < _foodList.Count; i++)
            {
                _foodList[i].GetComponent<Food>()._move = false;
            }

            for (int i = 0; i < _coinList.Count; i++)
            {
                _coinList[i].GetComponent<Coin>()._move = false;
            }
        }

        public void BackButton()
        {
            Time.timeScale = 1;
            _foodArea.SetActive(true);
            _optionPage.SetActive(false);

            for (int i = 0; i < _foodList.Count; i++)
            {
                 _foodList[i].GetComponent<Food>()._move = true;
            }

            for (int i = 0; i < _coinList.Count; i++)
            {
                _coinList[i].GetComponent<Coin>()._move = true;
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene("AquaGame");
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}