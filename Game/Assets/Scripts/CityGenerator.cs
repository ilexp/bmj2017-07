using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

namespace Game
{
    public class CityGenerator : MonoBehaviour
    {
        private void CreateCube(Random rnd)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = new Vector3(rnd.Next(-5, 5), rnd.Next(-5, 5), rnd.Next(-5, 5));
        }

        private void Start()
        {
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                this.CreateCube(rnd);
            }
        }
    }
}