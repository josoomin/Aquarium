using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace josoomin
{
    public class FoodArea : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Aquarium.I.CreateFood();

            List<Fish> fili = Aquarium.I._fishList;

            for (int i = 0; i < fili.Count; i++)
            {
                if (fili[i].gameObject.tag == "Guppy" && fili[i]._hungry <= fili[i]._hungryLow)
                {
                    fili[i]._food = null;
                    fili[i].SearchFood_Guppy();
                }
            }
        }
    }
}