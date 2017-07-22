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

            string text = @"Party girls don't get hurt, can't feel anything
When will I learn? I push it down, push it down
I'm the one for a good time call, phone's blowin' up
They're ringin' my doorbell, I feel the love, feel the love

One, two, three; one, two, three; drink
One, two, three; one, two, three; drink
One, two, three; one, two, three; drink
Throw 'em back till I lose count

I'm gonna swing from the chandelier, from the chandelier
I'm gonna live like tomorrow doesn't exist, like it doesn't exist
I'm gonna fly like a bird through the night, feel my tears as they dry
I'm gonna swing from the chandelier, from the chandelier

I am holding on for dear life
Won't look down, won't open my eyes
Keep my glass full until morning light
'Cause I'm just holding on for tonight


Help me, I'm holding on for dear life
Won't look down, won't open my eyes
Keep my glass full until morning light
'Cause I'm just holding on for tonight, on for tonight

Sun is up, I'm a mess
Gotta get out now, gotta run from this
Here comes the shame, here comes the shame

One, two, three; one, two, three; drink
One, two, three; one, two, three; drink
One, two, three; one, two, three; drink
Throw 'em back till I lose count

I'm gonna swing from the chandelier, from the chandelier
I'm gonna live like tomorrow doesn't exist, like it doesn't exist
I'm gonna fly like a bird through the night, feel my tears as they dry
I'm gonna swing from the chandelier, from the chandelier

I am holding on for dear life
Won't look down, won't open my eyes
Keep my glass full until morning light
'Cause I'm just holding on for tonight

Help me, I'm holding on for dear life
Won't look down, won't open my eyes
Keep my glass full until morning light
'Cause I'm just holding on for tonight, on for tonight, on for tonight
'Cause I'm just holding on for tonight
Oh, I'm just holding on for tonight, on for tonight, on for tonight
'Cause I'm just holding on for tonight
'Cause I'm just holding on for tonight
Oh, I'm just holding on for tonight, on for tonight, on for tonight";
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