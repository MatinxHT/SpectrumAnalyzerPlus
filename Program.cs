using Microsoft.VisualBasic;
using System;

namespace PolynomialRegressionSpectrometerCalibration
{
    class Program
    {
        static void Main()
        {
            // 构建增广矩阵

            //double[] pixellist = {234.5,394.5,538.5,648,735,939.5,1052,1145};
            double[] pixellist = { 234.5, 394.5,538.5, 1052, 1711 };
            //double[,] matrix = BuildMatrix(4,pixellist);
            //double[] wavelength = { 235, 313, 365, 404,435,546,579 ,763};
            double[] wavelength = { 235,313, 365, 579, 763 };

            //new way
            int ranklevel = 4;
            double[,] coefficient = AnotherBuildAugmentedMatrix(ranklevel, pixellist, wavelength);
            PrintMatrix(coefficient);

            // 使用高斯消元法求解
            double[] solution = GaussianElimination(coefficient);

            //target a0 = 164
            // 打印解向量
            Console.WriteLine("Solution: ");
            for (int i = 0; i < solution.Length; i++)
            {
                Console.WriteLine($"a{i} = {solution[i]}");
            }


        }
        // 打印矩阵的函数（用于调试）
        static void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // 列名标题
            Console.Write("a0\t");
            for (int i = 1; i < cols - 1; i++) // 修正列名输出范围
            {
                Console.Write($"a{i}\t");
            }
            Console.WriteLine("wavelength");

            // 打印矩阵
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        // 构建矩阵的函数
        #region Linear Algebra

        /// <summary>
        /// 根据峰数n构建n*n初始矩阵的函数
        /// </summary>
        /// <param name="pixeLlist">锋对应的像素序号</param>
        /// <returns></returns>
        static double[,] BuildMatrix(int ranklevel,double[] pixeLlist)
        {
            double[,] result = new double[ranklevel, 1];
            ////a0
            //for (int i = 0; i < n; i++)
            //{
            //    result[i, 0] = 1;
            //}
            ////a1
            //result = BuildAugmentedMatrix(result, pixeLlist);
            ////a2
            //double[] a2list = new double[pixeLlist.Length];
            //for (int i = 0; i < pixeLlist.Length; i++)
            //{
            //    a2list[i] = Math.Pow(pixeLlist[i], 2);
            //}
            //result = BuildAugmentedMatrix(result, a2list);
            ////a3
            //double[] a3list = new double[pixeLlist.Length];
            //for (int i = 0; i < pixeLlist.Length; i++)
            //{
            //    a3list[i] = Math.Pow(pixeLlist[i], 3);
            //}
            //result = BuildAugmentedMatrix(result, a3list);

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
                    powerlist[i]= Math.Pow(pixeLlist[i], j);
                result = BuildAugmentedMatrix(result, powerlist);
                j++;
            }

            return result;
        }

        /// <summary>
        /// 给矩阵增加一列，或者说构建增广矩阵
        /// </summary>
        /// <param name="coefficients"></param>
        /// <param name="constants"></param>
        /// <returns></returns>
        static double[,] BuildAugmentedMatrix(double[,] coefficients, double[] constants)
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

        static double[,] AnotherBuildAugmentedMatrix(int ranklevel,double[] pixellist, double[] wavelengthlist)
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


        // 高斯消元法
        static double[] GaussianElimination(double[,] matrix)
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

        #endregion
    }
}

