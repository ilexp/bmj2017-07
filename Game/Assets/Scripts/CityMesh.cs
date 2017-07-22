using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

namespace Game
{
    public class CityMesh : MonoBehaviour
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

            StaticBatchingUtility.Combine(tilemapObj.ToArray(), this.tilemap.gameObject);

            foreach (GameObject tile in tiles)
            {
                GameObject.Destroy(tile);
            }

            return tilemapObj;
        }


        private void Awake()
        {
            Random rnd = new Random();

            string text = @"1. Und der Haifisch, der hat Zähne
und die trägt er im Gesicht
und Macheath, der hat ein Messer,
doch das Messer sieht man nicht.

2.Ach, es sind des Haifischs Flossen
Rot, wenn dieser Blut vergiesst!
Mackie Messer trägt 'nen Handschuh
Drauf man keine Untat liest.

3.An der Themse grünem Wasser
Fallen plötzlich Leute um!
Es ist weder Pest noch Cholera
Doch es heisst: Maceath geht um.

4.An 'nem schönen blauen Sonntag
Liegt ein toter Mann am Strand
Und ein Mensch geht um die Ecke
Den man Mackie Messer nennt.


5.Und Schmul Meier bleibt verschwunden
Und so mancher reiche Mann
Und sein Geld hat Mackie Messer
Dem man nichts beweisen kann.

6.Jenny Towler ward gefunden
Mit 'nem Messer in der Brust
Und am Kai geht Mackie Messer
Der von allem nichts gewusst.

7.Wo ist Alfons Glite, der Fuhrherr?
Kommt das je ans Sonnenlicht?
Wer es immer wissen könnte-
Mackie Messer weiß es nicht.

8.Und das grosse Feuer in Soho
Sieben Kinder und ein Greis-
In der Menge Mackie Messer, den
Man nicht fragt und der nichts weiss.

9.Und die minderjährige Witwe
deren Namen jeder weiss
Wachte auf und war geschändet,
Mackie, welches war dein Preis";
            CityGenerator generator = new CityGenerator(text, 100, 100);
            int[] map = generator.GenerateMap(0,1,2);

            List<GameObject> tiles = new List<GameObject>();
            Vector2 baseOffset = new Vector2(-50, -50);
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    int tileIndex = (map[x + y * 100]) % this.tilePrefabs.Length;
                    GameObject tile = this.CreateTile(
                        tileIndex,
                        baseOffset + new Vector2(x, y));
                    tiles.Add(tile);
                }
            }

            this.CombineTilemap(tiles);
        }
    }
}