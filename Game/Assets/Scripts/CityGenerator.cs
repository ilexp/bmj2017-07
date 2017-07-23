using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game

{

    /// <summary>
    /// Encapsulates logic to create a map based on text
    /// </summary>
    public class CityGenerator
    {
        /// <summary>
        /// The text on which the map will be based
        /// </summary>
        /// <returns>The unaltered string</returns>
        public string Seed { get; private set; }
        /// <summary>
        /// The width of the map
        /// </summary>
        /// <returns>The width of the map</returns>
        public int Width { get; private set; }
        /// <summary>
        /// The Height of the map
        /// </summary>
        /// <returns>The height of the map</returns>
        public int Height { get; private set; }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="seed">The text on which the map will be based</param>
        /// <param name="width">The width of the map</param>
        /// <param name="height">The height of the map</param>
        public CityGenerator(string seed, int width, int height)
        {
            Seed = seed;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// Executes the calculation and returns the map.
        /// </summary>
        /// <param name="streetValue">The value which should be used for the street</param>
        /// <param name="houseValueLowerBound">The lowest value which should be used for houses</param>
        /// <param name="houseValueUpperBound">The highes value which should be used for houses</param>
        /// <returns>A map with streets and random houses</returns>
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
        /// <summary>
        /// Executes the calculation and return the map.
        /// </summary>
        /// <param name="streetValue">The value which should be used for the street</param>
        /// <param name="houseValueLowerBound">The lowest value which should be used for houses</param>
        /// <param name="houseValueUpperBound">The highes value which should be used for houses</param>
        /// <returns></returns>
        private int[,] GenerateMapFromSeed(int streetValue, int houseValueLowerBound, int houseValueUpperBound)
        {

            var map = new int[Width, Height];

            map = FillMapWithBackGround(map, houseValueLowerBound, houseValueUpperBound);

            map = CreatePath(map, Width / 2, Height / 2, streetValue);

            return map;
        }
        /// <summary>
        /// Based on the seed, this function creates a connected path on the map
        /// </summary>
        /// <param name="map">The map to write the path onto</param>
        /// <param name="startX">The start point, x</param>
        /// <param name="startY">The start point, y</param>
        /// <param name="streetValue">The value of the street</param>
        /// <returns>The map with added path</returns>
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
        /// <summary>
        /// Draws the path
        /// </summary>
        /// <param name="map">The map to write the path onto</param>
        /// <param name="start">The start point</param>
        /// <param name="word">The word on which the next part of the line is based on</param>
        /// <param name="streetColor">The street value</param>
        /// <returns>The last position of the cursor</returns>
        private Tuple DrawThePath(int[,] map, Tuple start, string word, int streetColor)
        {
            //var color = ChooseColor(word);

            var color = streetColor;

            var direction = ChooseDirection(word);
            var length = ChooseLength(word);

            var endCursor = DrawLine(map, start, color, direction, length);

            return endCursor;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map">The map to write the path onto</param>
        /// <param name="start">The start point</param>
        /// <param name="color">The street value</param>
        /// <param name="direction">The direction in which the next part of the street is to draw</param>
        /// <param name="length">How long the next part should be drawn</param>
        /// <returns>The last position of the cursor</returns>
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
                    for (counter = 0; counter <= length; counter++)
                    {
                        y--;
                        var t = SetColorOnMap(map, x, y, color);
                        x = t.Item1;
                        y = t.Item2;

                    }

                    break;
                case 1:
                    for (counter = 0; counter <= length; counter++)
                    {
                        x--;
                        var t = SetColorOnMap(map, x, y, color);
                        x = t.Item1;
                        y = t.Item2;
                    }

                    break;

                case 2:
                    for (counter = 0; counter <= length; counter++)
                    {
                        y++;
                        var t = SetColorOnMap(map, x, y, color);
                        x = t.Item1;
                        y = t.Item2;
                    }

                    break;
                case 3:
                    for (counter = 0; counter <= length; counter++)
                    {
                        x++;
                        var t = SetColorOnMap(map, x, y, color);
                        x = t.Item1;
                        y = t.Item2;

                    }
                    break;

                default:
                    UnityEngine.Debug.LogFormat("Wie auch immer, modulo4 sollte keine Zahl größer 3 ausgeben: {0}", direction);
                    break;
            }
            return new Tuple(x, y);
        }
        /// <summary>
        /// Set the color on one tile the map, with boundary check (Overflowing)
        /// </summary>
        /// <param name="map">The map to draw onto</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y  coordinate</param>
        /// <param name="color">The streetvalue</param>
        /// <returns>The cursor position (could be different from input because of overflow)</returns>
        private Tuple SetColorOnMap(int[,] map, int x, int y, int color)
        {
            x = BorderControllWidth(x);

            y = BorderControllHeight(y);

            map[x, y] = color;
            return new Tuple(x, y);
        }
        /// <summary>
        /// Helper function for overflowing
        /// </summary>
        /// <param name="y">Y coordinate</param>
        /// <returns>new Y coordinate</returns>
        private int BorderControllHeight(int y)
        {
            if (y < 0)
            {
                y = Height - 1;
            }
            else
            {
                if (y >= Height)
                {
                    y = 0;
                }

            }
            return y;
        }
        /// <summary>
        /// Helper function for overflowing
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <returns>new X coordinate</returns>
        private int BorderControllWidth(int x)
        {

            if (x < 0)
            {
                x = Width - 1;

            }
            else
            {
                if (x >= Width)
                {
                    x = 0;
                }

            }
            return x;
        }
        /// <summary>
        /// Helper funktion to choose the length of the next street part based on the string length
        /// </summary>
        /// <param name="word">The word to check</param>
        /// <returns>The length of the next street part</returns>
        private int ChooseLength(string word)
        {
            return word.Length;
        }

/// <summary>
/// The direction of the next street part.
/// Based on the 8 Direction model:
/// 1 | 0 | 7
/// 2 | X | 6
/// 3 | 4 | 5
/// but adapted because we only have four directions:
/// 0 | 0 | 3
/// 1 | X | 3
/// 1 | 2 | 2
/// </summary>
/// <param name="word">The word the direction is based on</param>
/// <returns>The direction. between 0 and 3</returns>
        public byte ChooseDirection(string word)
        {
            var wordLength = word.Length;
            var result = wordLength % 4;
            Debug.WriteLine(word + " = " + wordLength + " = " + (byte)result);

            return (byte)(result);
        }

        /// <summary>
        /// Initializes the map with random values
        /// </summary>
        /// <param name="map">The map to draw onto</param>
        /// <param name="houseValueLowerBound">The lower bound for house values</param>
        /// <param name="houseValueUpperBound">The higher bound for house values</param>
        /// <returns>The map, initialized</returns>
        private int[,] FillMapWithBackGround(int[,] map, int houseValueLowerBound, int houseValueUpperBound)
        {
            var r = new Random();
            var c = 0;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    c++;
                    var tileValue = r.Next(houseValueLowerBound, houseValueUpperBound + 1);

                    map[x, y] = tileValue;
                    //DEBUG
                    //map[x, y] = 1;
                    if (r.Next(1, 100) % 4 == 0)
                    {
                        map[x, y] = 0;
                    }
                }
            }
            return map;
        }
    }
}
