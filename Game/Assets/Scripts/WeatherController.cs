using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WeatherController : MonoBehaviour
    {

        [SerializeField] public Lightning Zeus;


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {

                Zeus.Flash();
            }
        }
    }
}