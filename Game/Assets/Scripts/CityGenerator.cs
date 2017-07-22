using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game
{
    public class CityGenerator
    {
        public string Seed { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }


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
                currentCursor = DrawThePath(map, currentCursor, word, streetValue);

            }

            return map;
        }

        private Tuple DrawThePath(int[,] map, Tuple start, string word, int streetColor)
        {
            //var color = ChooseColor(word);

            var color = streetColor;

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
                        map = SetColorOnMap(map, x, y, color);

                    }

                    break;
                case 1:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        x--;
                        map = SetColorOnMap(map, x, y, color);
                    }

                    break;

                case 2:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        y++;
                        map = SetColorOnMap(map, x, y, color);
                    }

                    break;
                case 3:
                    counter = 0;
                    for (counter = 0; counter <= length; counter++)
                    {
                        x++;
                        map = SetColorOnMap(map, x, y, color);
                    }

                    break;

                default:
                    UnityEngine.Debug.LogFormat("Wie auch immer, modulo4 sollte keine Zahl größer 3 ausgeben: {0}", direction);
                    break;
            }
            return new Tuple(x, y);
        }

        private int[,] SetColorOnMap(int[,] map, int x, int y, int color)
        {
            x = BorderControllWidth(x);

            y = BorderControllHeight(y);

            map[x, y] = color;
            return map;
        }

        private int BorderControllHeight(int y)
        {
            if (y < 0)
            {
                y = Height - 1;
            }
            if (y >= Height)
            {
                y = 0;
            }
            return y;
        }

        private int BorderControllWidth(int x)
        {

            if (x < 0)
            {
                x = Width - 1;
            }
            if (x >= Width)
            {
                x = 0;
            }
            return x;
        }

        private int ChooseLength(string word)
        {
            return word.Length;
        }

        public byte ChooseDirection(string word)
        {
            var wordLength = word.Length;
            var result = wordLength % 4;
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
                    //DEBUG
                    //map[x, y] = 1;
                }
            }
            return map;
        }
    }
}
