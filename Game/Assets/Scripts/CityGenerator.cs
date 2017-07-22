using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

namespace Game
{
    public class CityGenerator : MonoBehaviour
    {
        [SerializeField] private Material tileset;
        [SerializeField] private Transform tilemap;
        [SerializeField] private GameObject[] tilePrefabs;


        private GameObject CreateTile(int index, Vector2 pos)
        {
            GameObject prefab = tilePrefabs[index];
            GameObject obj = GameObject.Instantiate(prefab, tilemap);
            obj.transform.position = new Vector3(pos.x, 0.0f, pos.y);
            return obj;
        }
        private GameObject WrapUpTilemapMesh(CombineInstance[] combine)
        {
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine);

            GameObject obj = new GameObject("Tilemap",
                typeof(MeshFilter),
                typeof(MeshRenderer));
            obj.transform.SetParent(tilemap, true);
            MeshFilter filter = obj.GetComponent<MeshFilter>();
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

            filter.mesh = combinedMesh;
            renderer.sharedMaterial = this.tileset;

            return obj;
        }
        private List<GameObject> CombineTilemap(List<GameObject> tiles)
        {
            List<GameObject> tilemapObj = new List<GameObject>();

            int vertexCount = 0;
            List<CombineInstance> combine = new List<CombineInstance>();
            foreach (GameObject tile in tiles)
            {
                foreach (MeshFilter meshFilter in tile.GetComponentsInChildren<MeshFilter>())
                {
                    if (vertexCount + meshFilter.sharedMesh.vertexCount >= ushort.MaxValue)
                    {
                        tilemapObj.Add(this.WrapUpTilemapMesh(combine.ToArray()));
                        combine.Clear();
                        vertexCount = 0;
                    }

                    combine.Add(new CombineInstance
                    {
                        mesh = meshFilter.sharedMesh,
                        transform = meshFilter.transform.localToWorldMatrix
                    });
                    vertexCount += meshFilter.sharedMesh.vertexCount;
                }
            }

            if (combine.Count > 0)
            {
                tilemapObj.Add(this.WrapUpTilemapMesh(combine.ToArray()));
            }

            foreach (GameObject tile in tiles)
            {
                GameObject.Destroy(tile);
            }

            return tilemapObj;
        }


        private void Awake()
        {
            Random rnd = new Random();

            List<GameObject> tiles = new List<GameObject>();
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    GameObject tile = this.CreateTile(
                        rnd.Next(this.tilePrefabs.Length), 
                        new Vector2(x, y));
                    tiles.Add(tile);
                }
            }

            this.CombineTilemap(tiles);
        }
    }
}