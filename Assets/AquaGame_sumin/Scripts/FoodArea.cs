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
        }
    }
}