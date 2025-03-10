﻿using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace hedCommon.extension.runtime
{
    public static class ExtMathf
    {
        public static float Round(float value, int digits)
        {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return Mathf.Round(value * mult) / mult;
        }

        public static double Abs(double value)
        {
            value = (value < 0) ? value * -1 : value;
            return (value);
        }

        /// <summary>
        /// number convert range (55 from 0 to 100, to a base 0 - 1 for exemple)
        /// </summary>
        public static double Remap(this double value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static bool IsNaN(this float value)
        {
            return (float.IsNaN(value));
        }

        /// <summary>
        /// From a given value (2), in an interval, from 0.5 to 3,
        /// give the mirror of this value in that interval, here: 1.5
        /// </summary>
        /// <param name="x">value to transpose</param>
        /// <param name="minInterval"></param>
        /// <param name="maxInterval"></param>
        /// <returns></returns>
        public static float MirrorFromInterval(float x, float minInterval, float maxInterval)
        {
            float middle = (minInterval + maxInterval) / 2f;
            return (SymetricToPivotPoint(x, middle));
        }
        public static int MirrorFromInterval(int x, int minInterval, int maxInterval)
        {
            int middle = (minInterval + maxInterval) / 2;
            return (SymetricToPivotPoint(x, middle));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">the value to transpose</param>
        /// <param name="a">pivot point</param>
        /// <returns></returns>
        public static float SymetricToPivotPoint(float x, float a)
        {
            return (-x + 2 * a);
        }
        public static int SymetricToPivotPoint(int x, int a)
        {
            return (-x + 2 * a);
        }


        /// <summary>
        /// return the average number between an array of points
        /// </summary>
        public static float GetAverageOfNumbers(float[] arrayNumber)
        {
            if (arrayNumber.Length == 0)
                return (0);

            float sum = 0;
            for (int i = 0; i < arrayNumber.Length; i++)
            {
                sum += arrayNumber[i];
            }
            return (sum / arrayNumber.Length);
        }

        /// <summary>
        /// return the min of 3 value
        /// </summary>
        public static float Min(float value1, float value2, float value3)
        {
            float min = (value1 < value2) ? value1 : value2;
            return (min < value3) ? min : value3;
        }

        /// <summary>
        /// return the max of 3 value
        /// </summary>
        public static float Max(float value1, float value2, float value3)
        {
            float max = (value1 > value2) ? value1 : value2;
            return (max > value3) ? max : value3;
        }

        /// <summary>
        /// return the value clamped between the 2 value
        /// </summary>
        /// <param name="value1">must be less than value2</param>
        /// <param name="currentValue"></param>
        /// <param name="value2">must be more than value1</param>
        /// <returns></returns>
        public static float SetBetween(float currentValue, float value1, float value2)
        {
            if (value1 > value2)
            {
                Debug.LogError("value2 can be less than value1");
                return (0);
            }

            if (currentValue < value1)
            {
                currentValue = value1;
            }
            if (currentValue > value2)
            {
                currentValue = value2;
            }
            return (currentValue);
        }

        /// <summary>
        /// return the value clamped between the 2 value
        /// </summary>
        /// <param name="value1">must be less than value2</param>
        /// <param name="currentValue"></param>
        /// <param name="value2">must be more than value1</param>
        /// <returns></returns>
        public static int SetBetween(int currentValue, int value1, int value2)
        {
            if (value1 > value2)
            {
                Debug.LogError("value2 can be less than value1");
                return (0);
            }

            if (currentValue < value1)
            {
                currentValue = value1;
            }
            if (currentValue > value2)
            {
                currentValue = value2;
            }
            return (currentValue);
        }

        public static float Squared(this float value)
        {
            return value * value;
        }
        public static float Squared(this int value)
        {
            return value * value;
        }

        /// <summary>
        /// get closest point from an array of points
        /// </summary>
        public static Vector3 GetClosestPoint(Vector3 posEntity, Vector3[] arrayPos, out int indexFound)
        {
            float sqrDist = (posEntity - arrayPos[0]).sqrMagnitude;
            indexFound = 0;

            for (int i = 1; i < arrayPos.Length; i++)
            {
                float dist = (posEntity - arrayPos[i]).sqrMagnitude;
                if (dist < sqrDist)
                {
                    sqrDist = dist;
                    indexFound = i;
                }
            }
            return (arrayPos[indexFound]);
        }

        /// <summary>
        /// get closest point from an array of points
        /// </summary>
        public static Vector3 GetClosestPoint(Vector3 posEntity, List<Vector3> arrayPos, out int indexFound)
        {
            float sqrDist = (posEntity - arrayPos[0]).sqrMagnitude;
            indexFound = 0;

            for (int i = 1; i < arrayPos.Count; i++)
            {
                float dist = (posEntity - arrayPos[i]).sqrMagnitude;
                if (dist < sqrDist)
                {
                    sqrDist = dist;
                    indexFound = i;
                }
            }
            return (arrayPos[indexFound]);
        }

        /// <summary>
        /// get closest point from an array of points
        /// </summary>
        public static Vector3 GetClosestPointArray(Vector3 posEntity, Vector3[] arrayPos, out int indexFound)
        {
            float sqrDist = (posEntity - arrayPos[0]).sqrMagnitude;
            indexFound = 0;

            for (int i = 1; i < arrayPos.Length; i++)
            {
                float dist = (posEntity - arrayPos[i]).sqrMagnitude;
                if (dist < sqrDist)
                {
                    sqrDist = dist;
                    indexFound = i;
                }
            }
            return (arrayPos[indexFound]);
        }

        /// <summary>
        /// get closest point from an array of points
        /// </summary>
        public static Vector3 GetClosestPointArray(Vector3 posEntity, out int indexFound, params Vector3[] arrayPos)
        {
            return (GetClosestPointArray(posEntity, arrayPos, out indexFound));
        }

        /// <summary>
        /// from a given valueToTest, find the closest point
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="allValues"></param>
        /// <returns></returns>
        public static float GetClosestValueFromAnother(float targetNumber, float[] allValues)
        {
            float nearest = allValues.Min(x => Math.Abs((long)x - targetNumber));
            return (nearest);
        }
        public static float GetClosestValueFromAnother(float targetNumber, float[] allValues, out int indexFound)
        {
            indexFound = -1;
            if (allValues.Length == 0)
            {
                return (0);
            }

            Vector2 closestFound = new Vector2(0, Mathf.Abs(targetNumber - allValues[0]));
            for (int i = 1; i < allValues.Length; i++)
            {
                float diff = Mathf.Abs(targetNumber - allValues[i]);
                if (diff < closestFound.y)
                {
                    closestFound = new Vector2(i, diff);
                }
            }
            indexFound = (int)closestFound.x;
            return (closestFound.y);
        }

        /// <summary>
        /// from a given paths (sets of points), return the lenght of the path
        /// </summary>
        /// <param name="chunkPath"></param>
        /// <returns></returns>
        public static float GetLenghtOfPath(Vector3[] chunkPath)
        {
            float lenghtPath = 0f;

            for (int i = 0; i < chunkPath.Length - 1; i++)
            {
                Vector3 posCurrent = chunkPath[i];
                Vector3 posNext = chunkPath[i + 1];
                lenghtPath += (posNext - posCurrent).magnitude;
            }
            return (lenghtPath);
        }

        #region series

        /// <summary>
        /// Sums a series of numeric values passed as a param array...
        /// 
        /// MathUtil.Sum(1,2,3,4) == 10
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static short Sum(params short[] arr)
        {
            short result = 0;

            foreach (short value in arr)
            {
                result += value;
            }

            return result;
        }

        /// <summary>
        /// Sums a series of numeric values passed as a param array...
        /// 
        /// MathUtil.Sum(1,2,3,4) == 10
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int Sum(params int[] arr)
        {
            int result = 0;

            foreach (int value in arr)
            {
                result += value;
            }

            return result;
        }

        public static int Sum(int[] arr, int startIndex, int endIndex)
        {
            int result = 0;

            for (int i = startIndex; i <= Math.Min(endIndex, arr.Length - 1); i++)
            {
                result += arr[i];
            }

            return result;
        }

        public static int Sum(int[] arr, int startIndex)
        {
            return Sum(arr, startIndex, int.MaxValue);
        }

        /// <summary>
        /// Sums a series of numeric values passed as a param array...
        /// 
        /// MathUtil.Sum(1,2,3,4) == 10
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long Sum(params long[] arr)
        {
            long result = 0;

            foreach (long value in arr)
            {
                result += value;
            }

            return result;
        }

        /// <summary>
        /// Sums a series of numeric values passed as a param array...
        /// 
        /// MathUtil.Sum(1,2,3,4) == 10
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float Sum(params float[] arr)
        {
            float result = 0;

            foreach (float value in arr)
            {
                result += value;
            }

            return result;
        }


        /// <summary>
        /// Multiplies a series of numeric values passed as a param array...
        /// 
        /// MathUtil.Product(2,3,4) == 24
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float Product(params short[] arr)
        {
            if (arr == null || arr.Length == 0)
                return float.NaN;

            float result = 1;

            foreach (short value in arr)
            {
                result *= value;
            }

            return result;
        }

        /// <summary>
        /// Multiplies a series of numeric values passed as a param array...
        /// 
        /// MathUtil.Product(2,3,4) == 24
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float Product(params int[] arr)
        {
            if (arr == null || arr.Length == 0)
                return float.NaN;

            float result = 1;

            foreach (int value in arr)
            {
                result *= value;
            }

            return result;
        }

        /// <summary>
        /// Multiplies a series of numeric values passed as a param array...
        /// 
        /// MathUtil.ProductSeries(2,3,4) == 24
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float Product(params long[] arr)
        {
            if (arr == null || arr.Length == 0)
                return float.NaN;

            float result = 1;

            foreach (long value in arr)
            {
                result *= value;
            }

            return result;
        }

        /// <summary>
        /// Multiplies a series of numeric values passed as a param array...
        /// 
        /// MathUtil.ProductSeries(2,3,4) == 24
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float Product(params float[] arr)
        {
            if (arr == null || arr.Length == 0)
                return float.NaN;

            float result = 1f;

            foreach (float value in arr)
            {
                result *= value;
            }

            return result;
        }

        public static float Product(this IEnumerable<float> coll)
        {
            if (coll == null) return float.NaN;

            float result = 1f;
            foreach (float value in coll)
            {
                result *= value;
            }
            return result;
        }

        #endregion

        #region "Advanced Math"

        /// <summary>
        /// Compute the logarithm of any value of any base
        /// </summary>
        /// <param name="value"></param>
        /// <param name="base"></param>
        /// <returns></returns>
        /// <remarks>
        /// a logarithm is the exponent that some constant (base) would have to be raised to 
        /// to be equal to value.
        /// 
        /// i.e.
        /// 4 ^ x = 16
        /// can be rewritten as to solve for x
        /// logB4(16) = x
        /// which with this function would be 
        /// LoDMath.logBaseOf(16,4)
        /// 
        /// which would return 2, because 4^2 = 16
        /// </remarks>
        public static float LogBaseOf(float value, float @base)
        {
            return (float)(Math.Log(value) / Math.Log(@base));
        }

        /// <summary>
        /// Check if a value is prime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// In this method to increase speed we first check if the value is ltOReq 1, because values ltOReq 1 are not prime by definition. 
        /// Then we check if the value is even but not equal to 2. If so the value is most certainly not prime. 
        /// Lastly we loop through all odd divisors. No point in checking 1 or even divisors, because if it were divisible by an even 
        /// number it would be divisible by 2. If any divisor existed when i > value / i then its compliment would have already 
        /// been located. And lastly the loop will never reach i == val because i will never be > sqrt(val).
        /// 
        /// proof of validity for algorithm:
        /// 
        /// all trivial values are thrown out immediately by checking if even or less then 2
        /// 
        /// all remaining possibilities MUST be odd, an odd is resolved as the multiplication of 2 odd values only. (even * anyValue == even)
        /// 
        /// in resolution a * b = val, a = val / b. As every compliment a for b, b and a can be swapped resulting in b being ltOReq a. If a compliment for b 
        /// exists then that compliment would have already occured (as it is odd) in the swapped addition at the even split.
        /// 
        /// Example...
        /// 
        /// 16
        /// 1 * 16
        /// 2 * 8
        /// 4 * 4
        /// 8 * 2
        /// 16 * 1
        /// 
        /// checks for 1, 2, and 4 would have already checked the validity of 8 and 16.
        /// 
        /// Thusly we would only have to loop as long as i ltOReq val / i. Once we've reached the middle compliment, all subsequent factors have been resolved.
        /// 
        /// This shrinks the number of loops for odd values from [ floor(val / 2) - 1 ] down to [ ceil(sqrt(val) / 2) - 1 ]
        /// 
        /// example, if we checked EVERY odd number for the validity of the prime 7927, we'd loop 3962 times
        /// 
        /// but by this algorithm we loop only 43 times. Significant improvement!
        /// </remarks>
        public static bool IsPrime(long value)
        {
            // check if value is in prime number range
            if (value < 2)
                return false;

            // check if even, but not equal to 2
            if ((value % 2) == 0 & value != 2)
                return false;

            // if 2 or odd, check if any non-trivial divisors exist
            long sqrrt = (long)Math.Floor(Math.Sqrt(value));
            for (long i = 3; i <= sqrrt; i += 2)
            {
                if ((value % i) == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Relative Primality between two integers
        /// 
        /// By definition two integers are considered relatively prime if their 
        /// 'greatest common divisor' is 1. So thusly we simply just check if 
        /// the GCD of m and n is 1.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsRelativelyPrime(short m, short n)
        {
            return GCD(m, n) == 1;
        }

        public static bool IsRelativelyPrime(int m, int n)
        {
            return GCD(m, n) == 1;
        }

        public static bool IsRelativelyPrime(long m, long n)
        {
            return GCD(m, n) == 1;
        }

        public static int[] FactorsOf(int value)
        {
            value = Math.Abs(value);
            List<int> arr = new List<int>();
            int sqrrt = (int)Math.Sqrt(value);
            int c = 0;

            for (int i = 1; i <= sqrrt; i++)
            {
                if ((value % i) == 0)
                {
                    arr.Add(i);
                    c = value / i;
                    if (c != i)
                        arr.Add(c);
                }
            }

            arr.Sort();

            return arr.ToArray();
        }

        public static int[] CommonFactorsOf(int m, int n)
        {
            int i = 0;
            int j = 0;
            if (m < 0) m = -m;
            if (n < 0) n = -n;

            if (m > n)
            {
                i = m;
                m = n;
                n = i;
            }

            var set = new HashSet<int>(); //ensures no duplicates

            int r = (int)Math.Sqrt(m);
            for (i = 1; i <= r; i++)
            {
                if ((m % i) == 0 && (n % i) == 0)
                {
                    set.Add(i);
                    j = m / i;
                    if ((n % j) == 0) set.Add(j);
                    j = n / i;
                    if ((m % j) == 0) set.Add(j);
                }
            }

            int[] arr = System.Linq.Enumerable.ToArray(set);
            System.Array.Sort(arr);
            return arr;



            //more loops
            /*
            List<int> arr = new List<int>();

            int i = 0;
            if (m < 0) m = -m;
            if (n < 0) n = -n;

            //make sure m is < n
            if (m > n)
            {
                i = m;
                m = n;
                n = i;
            }

            //could be sped up by looping to sqrt(m), but then would have to do extra work to make sure duplicates don't occur
            for (i = 1; i <= m; i++)
            {
                if ((m % i) == 0 && (n % i) == 0)
                {
                    arr.Add(i);
                }
            }

            return arr.ToArray();
            */
        }

        /// <summary>
        /// Greatest Common Divisor using Euclid's algorithm
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GCD(int m, int n)
        {
            int r = 0;

            // make sure positive, GCD is always positive
            if (m < 0) m = -m;
            if (n < 0) n = -n;

            // m must be >= n
            if (m < n)
            {
                r = m;
                m = n;
                n = r;
            }

            // now start loop, loop is infinite... we will cancel out sooner or later
            while (true)
            {
                r = m % n;
                if (r == 0)
                    return n;
                m = n;
                n = r;
            }

            // fail safe
            //return 1;
        }

        public static long GCD(long m, long n)
        {
            long r = 0;

            // make sure positive, GCD is always positive
            if (m < 0) m = -m;
            if (n < 0) n = -n;

            // m must be >= n
            if (m < n)
            {
                r = m;
                m = n;
                n = r;
            }

            // now start loop, loop is infinite... we will cancel out sooner or later
            while (true)
            {
                r = m % n;
                if (r == 0)
                    return n;
                m = n;
                n = r;
            }

            // fail safe
            //return 1;
        }

        public static int LCM(int m, int n)
        {
            return (m * n) / GCD(m, n);
        }

        /// <summary>
        /// Factorial - N!
        /// 
        /// Simple product series
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// By definition 0! == 1
        /// 
        /// Factorial assumes the idea that the value is an integer >= 0... thusly UInteger is used
        /// </remarks>
        public static long Factorial(uint value)
        {
            if (value <= 0)
                return 1;

            long res = value;

            while (--value != 0)
            {
                res *= value;
            }

            return res;
        }

        /// <summary>
        /// Falling facotiral
        /// 
        /// defined: (N)! / (N - x)!
        /// 
        /// written subscript: (N)x OR (base)exp
        /// </summary>
        /// <param name="base"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long FallingFactorial(uint @base, uint exp)
        {
            return Factorial(@base) / Factorial(@base - exp);
        }

        /// <summary>
        /// rising factorial
        /// 
        /// defined: (N + x - 1)! / (N - 1)!
        /// 
        /// written superscript N^(x) OR base^(exp)
        /// </summary>
        /// <param name="base"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long RisingFactorial(uint @base, uint exp)
        {
            return Factorial(@base + exp - 1) / Factorial(@base - 1);
        }

        /// <summary>
        /// binomial coefficient
        /// 
        /// defined: N! / (k!(N-k)!)
        /// reduced: N! / (N-k)! == (N)k (fallingfactorial)
        /// reduced: (N)k / k!
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long BinCoef(uint n, uint k)
        {
            return FallingFactorial(n, k) / Factorial(k);
        }

        /// <summary>
        /// rising binomial coefficient
        /// 
        /// as one can notice in the analysis of binCoef(...) that 
        /// binCoef is the (N)k divided by k!. Similarly rising binCoef 
        /// is merely N^(k) / k! 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long RisingBinCoef(uint n, uint k)
        {
            return RisingFactorial(n, k) / Factorial(k);
        }
        #endregion

        public static bool IsHex(string value)
        {
            int i;
            for (i = 0; i < value.Length; i++)
            {
                if (value[i] == ' ' || value[i] == '+' || value[i] == '-') continue;

                break;
            }

            return (i < value.Length - 1 &&
                    (
                    (value[i] == '#') ||
                    (value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X')) ||
                    (value[i] == '&' && (value[i + 1] == 'h' || value[i + 1] == 'H'))
                    ));
        }

        /// <summary>
        /// Returns true if the value is a numeric type that is a whole round number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bBlankIsZero"></param>
        /// <returns></returns>
        public static bool IsInteger(object value)
        {
            if (value == null) return false;

            if (value is System.IConvertible)
            {
                var conv = value as System.IConvertible;
                if (IsInteger(conv.GetTypeCode())) return true;
                return (conv.ToDouble(null) % 1d) == 0d;
            }

            return false;
        }

        public static bool IsInteger(System.TypeCode code)
        {
            switch (code)
            {
                case System.TypeCode.SByte:
                    //5
                    return true;
                case System.TypeCode.Byte:
                    //6
                    return true;
                case System.TypeCode.Int16:
                    //7
                    return true;
                case System.TypeCode.UInt16:
                    //8
                    return true;
                case System.TypeCode.Int32:
                    //9
                    return true;
                case System.TypeCode.UInt32:
                    //10
                    return true;
                case System.TypeCode.Int64:
                    //11
                    return true;
                case System.TypeCode.UInt64:
                    //12
                    return true;
                default:
                    return false;
            }
        }
    }
}