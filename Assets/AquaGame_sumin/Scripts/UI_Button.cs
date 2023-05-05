using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class UI_Button : MonoBehaviour
    {
        public static UI_Button I;

        public GameObject _guppyPrefab;
        public GameObject _piranhaPrefab;

        public int _guppyPrice = 25;
        public int _piranhaPrice = 1000;

        private void Awake()
        {
            I = this;
        }

        public void CreateGuppy()
        {
            if (Aquarium.I._money >= _guppyPrice)
            {
                Debug.Log("Create Guppy");

                _Create(_guppyPrefab);

                Aquarium.I._money -= _guppyPrice;
            }
        }

        public void CreatePiranha()
        {
            if (Aquarium.I._money >= _piranhaPrice)
            {
                Debug.Log("Create Piranha");

                _Create(_piranhaPrefab);

                Aquarium.I._money -= _piranhaPrice;
            }
        }

        void _Create(GameObject prefab)
        {
            float MinX = -5.0f;
            float MaxX = 5.0f;
            float MinY = -3.0f;
            float MaxY = 2.0f;
            float MinZ = -5.0f;
            float MaxZ = 5.0f;

            float RanX = Random.Range(MinX, MaxX);
            float RanY = Random.Range(MinY, MaxY);
            float RanZ = Random.Range(MinZ, MaxZ);

            Vector3 Ran = new Vector3(RanX, RanY, RanZ);

            GameObject clone = Instantiate(prefab);
            clone.transform.position = Ran;
            clone.transform.parent = Aquarium.I._allFish.transform;

            Fish fish = clone.GetComponent<Fish>();

            Aquarium.I._fishList.Add(fish);
        }
    }
}