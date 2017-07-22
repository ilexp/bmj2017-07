using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game
{
    public class CityGenerator
    {
        public string Seed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public CityGenerator(string seed, int width, int height)
        {
            Seed = seed;
            Width = width;
            Height = height;
        }

        public int[] GenerateMap()
        {

            var map = GenerateMapFromSeed();
            var result = new int[map.GetLength(1) * map.GetLength(0)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    result[x + y * map.GetLength(0)] = map[x,y];
                }
            }
            return result;
        }
        private byte[,] GenerateMapFromSeed()
        {

            var map = new byte[Width, Height];
            var mapLength = Width * Height;

            //map = FillMapWithBackGround(map);

            map = CreatePath(map, Width / 2, Height / 2);

            return map;
        }

        private byte[,] CreatePath(byte[,] map, int startX, int startY)
        {

            var punctuation = Seed.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = Seed.Split().Select(x => x.Trim(punctuation));

            var currentCursor = new Tuple(startX, startY);

            foreach (var word in words)
            {
                currentCursor = DrawThePath(map, currentCursor, word);

            }

            return map;
        }

        private Tuple DrawThePath(byte[,] map, Tuple start, string word)
        {
            var color = ChooseColor(word);

            var direction = ChooseDirection(word);
            var length = ChooseLength(word);

            var endCursor = DrawLine(map, start, color, direction, length);

            return endCursor;
        }

        private Tuple DrawLine(byte[,] map, Tuple start, byte color, byte direction, int length)
        {
            int x = start.Item1;
            int y = start.Item2;
            int counter = 0;
            //horizontal
            switch (direction)
            {
                //hoch
                case 0:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        y--;
                        if (x < 0)
                        {
                            x = 0;
                        }
                        if (x >= Width)
                        {
                            x = Width - 1;

                        }
                        if (y < 0)
                        {
                            y = 0;
                        }
                        if (y > Height)
                        {
                            y = Height;
                        }

                        map[x, y] = color;
                    }

                    break;
                case 4:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        x--;
                        if (x < 0)
                        {
                            x = 0;
                        }
                        if (x >= Width)
                        {
                            x = Width - 1;

                        }
                        if (y < 0)
                        {
                            y = 0;
                        }
                        if (y > Height)
                        {
                            y = Height;
                        }

                        map[x, y] = color;
                    }

                    break;

                case 2:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        y++;
                        if (x < 0)
                        {
                            x = 0;
                        }
                        if (x >= Width)
                        {
                            x = Width - 1;

                        }
                        if (y < 0)
                        {
                            y = 0;
                        }
                        if (y > Height)
                        {
                            y = Height;
                        }

                        map[x, y] = color;
                    }

                    break;
                case 6:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        x++;
                        if (x < 0)
                        {
                            x = 0;
                        }
                        if (x >= Width)
                        {
                            x = Width - 1;
                        }
                        if (y < 0)
                        {
                            y = 0;
                        }
                        if (y > Height)
                        {
                            y = Height;
                        }

                        map[x, y] = color;
                    }

                    break;


                default:
                    break;
            }
            return new Tuple(x, y);
        }



        private int ChooseLength(string word)
        {
            return word.Length;
        }

        public byte ChooseDirection(string word)
        {
            var wordLength = word.Length;
            var result = wordLength % 8;
            Debug.WriteLine(word + " = " + wordLength + " = " + (byte)result);

            return (byte)(result);
        }

        private byte ChooseColor(string word)
        {
            return 1;
        }

        private byte[,] FillMapWithBackGround(byte[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = 0;
                }
            }
            return map;
        }
    }
}
