using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumAnalyzerPlus.Algorithms
{
    /// <summary>
    /// 线性代数的算法
    /// </summary>
    public class LinearAlgebra
    {
        /// <summary>
        /// 构建增广矩阵
        /// </summary>
        /// <param name="ranklevel"></param>
        /// <param name="pixellist"></param>
        /// <param name="wavelengthlist"></param>
        /// <returns></returns>
        public static double[,] AnotherBuildAugmentedMatrix(int ranklevel, double[] pixellist, double[] wavelengthlist)
        {
            double[,] matrix = BuildMatrix(ranklevel, pixellist);

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1) + 1; // 常数向量增加一列

            double[,] augmentedMatrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols - 1; j++)
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, cols - 1] = wavelengthlist[i]; // 最后一列是常数向量
            }

            return augmentedMatrix;
        }

        /// <summary>
        /// 根据峰数n构建n*n初始矩阵的函数
        /// </summary>
        /// <param name="pixeLlist">锋对应的像素序号</param>
        /// <returns></returns>
        private static double[,] BuildMatrix(int ranklevel, double[] pixeLlist)
        {
            double[,] result = new double[ranklevel, 1];

            //a0
            for (int i = 0; i < ranklevel; i++)
            {
                result[i, 0] = 1;
            }
            double[] powerlist = new double[ranklevel];
            int j = 1;
            while (j < ranklevel)
            {
                for (int i = 0; i < ranklevel; i++)
                    powerlist[i] = Math.Pow(pixeLlist[i], j);
                result = BuildAugmentedMatrix(result, powerlist);
                j++;
            }

            return result;
        }

        /// <summary>
        /// 构建增广矩阵，或者说给矩阵增加一列
        /// </summary>
        /// <param name="coefficients"></param>
        /// <param name="constants"></param>
        /// <returns></returns>
        private static double[,] BuildAugmentedMatrix(double[,] coefficients, double[] constants)
        {
            int rows = coefficients.GetLength(0);
            int cols = coefficients.GetLength(1) + 1; // 常数向量增加一列

            double[,] augmentedMatrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols - 1; j++)
                {
                    augmentedMatrix[i, j] = coefficients[i, j];
                }
                augmentedMatrix[i, cols - 1] = constants[i]; // 最后一列是常数向量
            }

            return augmentedMatrix;
        }

        /// <summary>
        /// 高斯消元法
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static double[] GaussianElimination(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[] solution = new double[rows];

            for (int i = 0; i < rows; i++)
            {
                // Find pivot row and swap
                int maxRowIndex = i;
                for (int k = i + 1; k < rows; k++)
                {
                    if (Math.Abs(matrix[k, i]) > Math.Abs(matrix[maxRowIndex, i]))
                    {
                        maxRowIndex = k;
                    }
                }

                // Swap rows i and maxRowIndex
                if (maxRowIndex != i)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double temp = matrix[i, j];
                        matrix[i, j] = matrix[maxRowIndex, j];
                        matrix[maxRowIndex, j] = temp;
                    }
                }

                // Check if the pivot element is zero (singular matrix)
                if (Math.Abs(matrix[i, i]) < double.Epsilon)
                {
                    throw new InvalidOperationException("Singular matrix");
                }

                // Reduce to triangular form
                for (int j = i + 1; j < rows; j++)
                {
                    double factor = matrix[j, i] / matrix[i, i];
                    for (int k = i; k < cols; k++)
                    {
                        matrix[j, k] -= factor * matrix[i, k];
                    }
                }
            }

            // Back substitution to solve the equations
            for (int i = rows - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < rows; j++)
                {
                    sum += matrix[i, j] * solution[j];
                }
                solution[i] = (matrix[i, cols - 1] - sum) / matrix[i, i];
            }

            return solution;
        }
    }
}
