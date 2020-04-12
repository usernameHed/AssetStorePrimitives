using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace hedCommon.extension.runtime
{
    /// <summary>
    /// Random functions
    /// <summary>
    public static class ExtRandom
    {
        // <summary>
        /// Generator for the random number generator
        /// </summary>
        private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();

        /// <summary>
        /// generate a random based on static hash
        /// </summary>
        /// <param name="seed">string based</param>
        /// <returns>rendom generated from the seed</returns>
        public static System.Random Seedrandom(string seed)
        {
            System.Random random = new System.Random(seed.GetHashCode());
            return (random);
        }

        /// <summary>
        /// get a random index. each section have there own chance of beeing pick.
        /// the total have to be equal to 100.
        /// 
        /// For exemple:
        /// List<int> sections = new List<int>() { 15, 5, 25, 5, 35, 15 } -> total: 100
        /// 
        /// we do a random from 0 to 100.
        /// </summary>
        /// <param name="sections">list of section</param>
        /// <returns>index of the section choosen</returns>
        public static int GetRandomSection(List<float> sections, out float randomNumber)
        {
            randomNumber = -1;

            if (sections == null || sections.Count == 0)
            {
                return (-1);
            }

            float total = sections.Sum();
            if (total <= 0)
            {
                return (-1);
            }
            randomNumber = ExtRandom.GetRandomNumber(0, total);
            float current = 0;
            for (int i = 0; i < sections.Count; i++)
            {
                if (randomNumber <= current + sections[i])
                {
                    return (i);
                }
                current += sections[i];
            }
            return (-1);
        }


        /// <summary>
        /// here we have a min & max, we remap the random 0,1 value finded to this min&max
        /// warning: we cast it into an int at the end
        /// use: 
        /// System.Random seed = ExtRandom.Seedrandom("hash");
        /// int randomInt = ExtRandom.RemapFromSeed(0, 50, seed);
        /// </summary>
        public static int RemapFromSeed(double min, double max, System.Random randomSeed)
        {
            double zeroToOneValue = randomSeed.NextDouble();
            int minToMaxValue = (int)ExtMathf.Remap(zeroToOneValue, 0, 1, min, max);
            return (minToMaxValue);
        }
        public static double RemapFromSeedDecimal(double min, double max, System.Random randomSeed)
        {
            double zeroToOneValue = randomSeed.NextDouble();
            double minToMaxValue = ExtMathf.Remap(zeroToOneValue, 0, 1, min, max);
            return (minToMaxValue);
        }
        public static float RemapFromSeedDecimal(float min, float max, System.Random randomSeed)
        {
            float zeroToOneValue = (float)randomSeed.NextDouble();
            float minToMaxValue = ExtMathf.Remap(zeroToOneValue, 0, 1, min, max);
            return (minToMaxValue);
        }

        /// <summary>
        /// Returns a random direction in a cone. a spread of 0 is straight, 0.5 is 180*
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="forward">must be unit</param>
        /// <returns></returns>
        public static Vector3 RandomDirection(float spread, Vector3 forward)
        {
            return Vector3.Slerp(forward, UnityEngine.Random.onUnitSphere, spread);
        }

        /// <summary>
        /// Get random int between min and max
        /// use: int randomInt = ExtRandom.GetRandomNumber(0, 2);
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum">EXCLUSIVE</param>
        /// <returns></returns>
        public static int GetRandomNumber(int minimum, int maximum)
        {
            int number = UnityEngine.Random.Range(minimum, maximum);
            return (number);
        }
        /// <summary>
        /// Get random float between min an dmax included
        /// </summary>
        public static float GetRandomNumber(float minimum, float maximum)
        {
            float number = UnityEngine.Random.Range(minimum, maximum);
            return (number);
        }
        /// <summary>
        /// Get a random from chance / max: 2 / 3
        /// use: ExtRandom.GetRandomNumberProbability(2, 3);
        /// </summary>
        /// <param name="chance"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool GetRandomNumberProbability(int chance, int max)
        {
            float number = GetRandomNumber(0f, 1f);
            return (number < (float)chance / (float)max);
        }
        /// <summary>
        /// do a coin flip, return true or false
        /// </summary>
        public static bool GetRandomBool()
        {
            float number = UnityEngine.Random.Range(0f, 1f);
            return (number > 0.5f);
        }

        /// <summary>
        /// get a random color
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            Color randomColor = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1);
            return (randomColor);
        }
        /// <summary>
        /// get a random color, with alpha 1
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColorSeed(System.Random randomSeed)
        {
            Color randomColor = new Color((float)RemapFromSeedDecimal(0.0f, 1.0f, randomSeed), (float)RemapFromSeedDecimal(0.0f, 1.0f, randomSeed), (float)RemapFromSeedDecimal(0.0f, 1.0f, randomSeed), 1);
            return (randomColor);
        }

        /// <summary>
        /// get a normal random
        /// use: GenerateNormalRandom(0, 0.1);
        /// </summary>
        /// <param name="mu">centre of the distribution</param>
        /// <param name="sigma">Standard deviation (spread or "width") of the distribution</param>
        /// <returns></returns>
        public static float GenerateNormalRandom(float mu, float sigma)
        {
            float rand1 = UnityEngine.Random.Range(0.0f, 1.0f);
            float rand2 = UnityEngine.Random.Range(0.0f, 1.0f);

            float n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

            return (mu + sigma * n);
        }

        /// <summary>
        /// Get a random unitsphere (or donut)
        /// </summary>
        /// <param name="radius">radius of circle</param>
        /// <param name="toreCenter">Excluse center from random</param>
        /// <returns></returns>
        public static Vector3 GetRandomInsideUnitSphere(float radius, float toreCenter = 0)
        {
            if (toreCenter == 0)
                return (UnityEngine.Random.insideUnitSphere * radius);

            if (toreCenter > radius)
            {
                Debug.LogError("radiusCenter can't be superior then radius");
            }

            Vector3 donut = Vector3.zero;
            for (int i = 0; i < 3; i++)
            {
                int absRandom = GetRandomNumber(0, 2);
                absRandom = (absRandom == 0) ? -1 : 1;

                donut.x = GetRandomNumber(toreCenter, radius) * absRandom;
                donut.y = GetRandomNumber(toreCenter, radius) * absRandom;
                donut.z = GetRandomNumber(toreCenter, radius) * absRandom;
            }
            return (donut);
        }
    }
}