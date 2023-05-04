using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace josoomin
{
    public class TitlePage : MonoBehaviour
    {
        public GameObject _howToPlayPage;

        private void Start()
        {
            _howToPlayPage.SetActive(false);
        }

        public void StartButton()
        {
            SceneManager.LoadScene("AquaGame");
        }

        public void HowToPlay()
        {
            _howToPlayPage.SetActive(true);
        }

        public void CloseHowToPlay()
        {
            _howToPlayPage.SetActive(false);
        }

        public void ExitButton()
        {
            Application.Quit();
        }


    }
}