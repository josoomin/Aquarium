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

                CreateFish(Aquarium.I._guppyPrefab);

                Aquarium.I._money -= Aquarium.I._guppyPrice;
            }

            List<Fish> fili = Aquarium.I._fishList;

            for (int i = 0; i < fili.Count; i++)
            {
                if (fili[i].gameObject.tag == "Piranha" && fili[i]._hungry <= fili[i]._hungryLow)
                {
                    fili[i]._food = null;
                    fili[i].SearchFood_Piranha();
                }
            }
        }

        public void CreatePiranha()
        {
            if (Aquarium.I._money >= Aquarium.I._piranhaPrice)
            {
                Debug.Log("Create Piranha");

                CreateFish(Aquarium.I._piranhaPrefab);

                Aquarium.I._money -= Aquarium.I._piranhaPrice;
            }
        }

        void CreateFish(GameObject prefab)
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