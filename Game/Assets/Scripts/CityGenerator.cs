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

        public int[] GenerateMap(int streetValue, int houseValueLowerBound, int houseValueUpperBound)
        {

            var map = GenerateMapFromSeed(streetValue, houseValueLowerBound, houseValueUpperBound);
            var result = new int[map.GetLength(1) * map.GetLength(0)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    result[x + y * map.GetLength(0)] = map[x, y];
                }
            }
            return result;
        }
        private int[,] GenerateMapFromSeed(int streetValue, int houseValueLowerBound, int houseValueUpperBound)
        {

            var map = new int[Width, Height];
            var mapLength = Width * Height;

            map = FillMapWithBackGround(map, houseValueLowerBound, houseValueUpperBound);

            map = CreatePath(map, Width / 2, Height / 2, streetValue);

            return map;
        }

        private int[,] CreatePath(int[,] map, int startX, int startY, int streetValue)
        {

            var punctuation = Seed.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = Seed.Split().Select(x => x.Trim(punctuation));

            var currentCursor = new Tuple(startX, startY);

            foreach (var word in words)
            {
                currentCursor = DrawThePath(map, currentCursor, word,streetValue);

            }

            return map;
        }

        private Tuple DrawThePath(int[,] map, Tuple start, string word, int streetColor)
        {
//            var color = ChooseColor(word);

            var color =streetColor;

            var direction = ChooseDirection(word);
            var length = ChooseLength(word);

            var endCursor = DrawLine(map, start, color, direction, length);

            return endCursor;
        }

        private Tuple DrawLine(int[,] map, Tuple start, int color, byte direction, int length)
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

        private int[,] FillMapWithBackGround(int[,] map, int houseValueLowerBound, int houseValueUpperBound)
        {
            var r = new Random();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    var tileValue = r.Next(houseValueLowerBound, houseValueUpperBound + 1);
                    map[x, y] = tileValue;
                }
            }
            return map;
        }
    }
}
