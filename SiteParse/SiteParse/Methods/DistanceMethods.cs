using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse.Methods
{
    public class DistanceMethods
    {
        /// <summary>
        /// Евклидово расстояние между векторами
        /// </summary>
        /// <param name="vect1"></param>
        /// <param name="vect2"></param>
        /// <returns></returns>
        public static double EuclideanDistance(double[] vect1, double[] vect2)
        {
            var sumSquaredDiffs = 0.0;
            for (int i = 0; i < vect1.Length; ++i)
            {
                sumSquaredDiffs += Math.Pow((vect1[i] - vect2[i]), 2);
            }

            return Math.Sqrt(sumSquaredDiffs);
        }


        #region Cosine Similarity
        public static float FindCosineSimilarity(float[] vecA, float[] vecB)
        {
            var dotProduct = DotProduct(vecA, vecB);
            var magnitudeOfA = Magnitude(vecA);
            var magnitudeOfB = Magnitude(vecB);
            float result = dotProduct / (magnitudeOfA * magnitudeOfB);
            //when 0 is divided by 0 it shows result NaN so return 0 in such case.
            if (float.IsNaN(result))
                return 0;
            else
                return (float)result;
        }

        #endregion

        public static float DotProduct(float[] vecA, float[] vecB)
        {

            float dotProduct = 0;
            for (var i = 0; i < vecA.Length; i++)
            {
                dotProduct += (vecA[i] * vecB[i]);
            }

            return dotProduct;
        }

        // Magnitude of the vector is the square root of the dot product of the vector with itself.
        public static float Magnitude(float[] vector)
        {
            return (float)Math.Sqrt(DotProduct(vector, vector));
        }
    }
}
