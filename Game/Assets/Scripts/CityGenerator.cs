using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

namespace Game
{
    public class CityGenerator : MonoBehaviour
    {
        [SerializeField] private Transform tilemap;
        [SerializeField] private GameObject[] tiles;


        private void CreateTile(int index, Vector2 pos)
        {
            GameObject prefab = tiles[index];
            GameObject obj = GameObject.Instantiate(prefab, tilemap);
            obj.transform.position = new Vector3(pos.x, 0.0f, pos.y);
        }


        private void Start()
        {
            Random rnd = new Random();
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    this.CreateTile(rnd.Next(tiles.Length), new Vector2(x, y));
                }
            }
        }
    }
}