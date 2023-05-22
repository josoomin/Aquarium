using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class UI_Button : MonoBehaviour
    {
        public static UI_Button I;

        private void Awake()
        {
            I = this;
        }

        public void CreateGuppy()
        {
            if (Aquarium.I._money >= Aquarium.I._guppyPrice)
            {
                Debug.Log("Create Guppy");

                Create(Aquarium.I._guppyPrefab);

                Aquarium.I._money -= Aquarium.I._guppyPrice;
            }
        }

        public void CreatePiranha()
        {
            if (Aquarium.I._money >= Aquarium.I._piranhaPrice)
            {
                Debug.Log("Create Piranha");

                Create(Aquarium.I._piranhaPrefab);

                Aquarium.I._money -= Aquarium.I._piranhaPrice;
            }
        }

        void Create(GameObject prefab)
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