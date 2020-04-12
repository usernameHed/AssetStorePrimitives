using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace hedCommon.extension.runtime
{
    public static class ExtVector3
    {
        /// <summary>
        /// test if a Vector3 is close to another Vector3 (due to floating point inprecision)
        /// compares the square of the distance to the square of the range as this
        /// avoids calculating a square root which is much slower than squaring the range
        /// </summary>
        /// <param name="val"></param>
        /// <param name="about"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool IsClose(Vector3 val, Vector3 about, float range)
        {
            float close = (val - about).sqrMagnitude;
            return (close < range * range);
        }

        /// <summary>
        /// divide 2 vector together, the first one is the numerator, the second the denominator
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static Vector3 DivideVectors(Vector3 numerator, Vector3 denominator)
        {
            return (new Vector3(numerator.x / denominator.x, numerator.y / denominator.y, numerator.z / denominator.z));
        }

        /// <summary>
        /// get the max lenght of this vector
        /// </summary>
        /// <returns>min lenght of x, y or z</returns>
        public static float Maximum(this Vector3 vector)
        {
            return ExtMathf.Max(vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// get the min lenght of this vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>min lenght of x, y or z</returns>
        public static float Minimum(this Vector3 vector)
        {
            return ExtMathf.Min(vector.x, vector.y, vector.z);
        }

        public static Vector3 GetMiddleOf2VectorNormalized(Vector3 a, Vector3 b)
        {
            return ((a + b).normalized);
        }


        public static Vector3 GetMiddleOfXContactNormal(ContactPoint[] arrayVect)
        {
            Vector3[] arrayTmp = new Vector3[arrayVect.Length];

            Vector3 sum = Vector3.zero;
            for (int i = 0; i < arrayVect.Length; i++)
            {
                arrayTmp[i] = arrayVect[i].normal;
            }
            return (GetMiddleOfXVector(arrayTmp));
        }

        public static Vector3 GetMeanOfXContactPoints(ContactPoint[] arrayContact)
        {
            Vector3[] arrayTmp = new Vector3[arrayContact.Length];

            for (int i = 0; i < arrayContact.Length; i++)
            {
                arrayTmp[i] = arrayContact[i].point;
            }
            return (GetMeanOfXPoints(arrayTmp, out Vector3 sizeBOundingBox, true));
        }

        public static Vector3 GetMiddleOfXVector(Vector3[] arrayVect)
        {
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < arrayVect.Length; i++)
            {
                sum += arrayVect[i];
            }
            return ((sum).normalized);
        }

        public static Vector3 GetMeanOfXPoints(Transform[] arrayVect, out Vector3 sizeBoundingBox, bool middleBoundingBox = true)
        {
            return GetMeanOfXPoints(ExtArray.ToGameObjectsArray(arrayVect), out sizeBoundingBox, middleBoundingBox);
        }

        public static Vector3 GetMeanOfXPoints(GameObject[] arrayVect, out Vector3 sizeBoundingBox, bool middleBoundingBox = true)
        {
            Vector3[] arrayPoint = new Vector3[arrayVect.Length];
            for (int i = 0; i < arrayPoint.Length; i++)
            {
                arrayPoint[i] = arrayVect[i].transform.position;
            }
            return (GetMeanOfXPoints(arrayPoint, out sizeBoundingBox, middleBoundingBox));
        }

        /// <summary>
        /// return the middle of X points (POINTS, NOT vector)
        /// </summary>
        public static Vector3 GetMeanOfXPoints(Vector3[] arrayVect, out Vector3 sizeBoundingBox, bool middleBoundingBox = true)
        {
            return (GetMeanOfXPoints(out sizeBoundingBox, middleBoundingBox, arrayVect));
        }
        public static Vector3 GetMeanOfXPoints(Vector3[] arrayVect, bool middleBoundingBox = true)
        {
            return (GetMeanOfXPoints(arrayVect, out Vector3 sizeBOundingBox, middleBoundingBox));
        }

        public static Vector3 GetMeanOfXPoints(params Vector3[] points)
        {
            return (GetMeanOfXPoints(out Vector3 sizeBoundingBox, false, points));
        }

        public static Vector3 GetMeanOfXPoints(out Vector3 sizeBoundingBox, bool middleBoundingBox = true, params Vector3[] points)
        {
            sizeBoundingBox = Vector2.zero;

            if (points.Length == 0)
            {
                return (Vector3.zero);
            }

            if (!middleBoundingBox)
            {
                Vector3 sum = Vector3.zero;
                for (int i = 0; i < points.Length; i++)
                {
                    sum += points[i];
                }
                return (sum / points.Length);
            }
            else
            {
                if (points.Length == 1)
                    return (points[0]);

                float xMin = points[0].x;
                float yMin = points[0].y;
                float zMin = points[0].z;
                float xMax = points[0].x;
                float yMax = points[0].y;
                float zMax = points[0].z;

                for (int i = 1; i < points.Length; i++)
                {
                    if (points[i].x < xMin)
                        xMin = points[i].x;
                    if (points[i].x > xMax)
                        xMax = points[i].x;

                    if (points[i].y < yMin)
                        yMin = points[i].y;
                    if (points[i].y > yMax)
                        yMax = points[i].y;

                    if (points[i].z < zMin)
                        zMin = points[i].z;
                    if (points[i].z > zMax)
                        zMax = points[i].z;
                }
                Vector3 lastMiddle = new Vector3((xMin + xMax) / 2, (yMin + yMax) / 2, (zMin + zMax) / 2);
                sizeBoundingBox.x = Mathf.Abs(xMin - xMax);
                sizeBoundingBox.y = Mathf.Abs(yMin - yMax);
                sizeBoundingBox.z = Mathf.Abs(zMin - zMax);

                return (lastMiddle);
            }
        }



        /// <summary>
        /// get la bisection de 2 vecteur
        /// </summary>
        public static Vector3 GetbisectionOf2Vector(Vector3 a, Vector3 b)
        {
            return ((a + b) * 0.5f);
        }
    }
}