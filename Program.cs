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
            double[] pixellist = { 234.5, 541, 1052, 1711 };
            int n = pixellist.Length;
            double[,] matrix = BuildMatrix(n, pixellist);
            //double[] wavelength = { 235, 313, 365, 404,435,546,579 ,763};
            double[] wavelength = { 235, 365, 546, 763 };
            double[,] coefficients = BuildAugmentedMatrix(matrix, wavelength);
            // 打印增广矩阵
            PrintMatrix(coefficients);

            // 使用高斯消元法求解
            double[] solution = GaussianElimination(coefficients);

            //double[] solution = GaussianElimination(coefficients)

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

        #region Linear Algebra

        // 构建矩阵的函数
        static double[,] BuildMatrix(int n, double[] pixeLlist)
        {
            double[,] result = new double[n, 1];
            //a0
            for (int i = 0; i < n; i++)
            {
                result[i, 0] = 1;
            }
            //a1
            result = BuildAugmentedMatrix(result, pixeLlist);
            //a2
            double[] a2list = new double[pixeLlist.Length];
            for (int i = 0; i < pixeLlist.Length; i++)
            {
                a2list[i] = Math.Pow(pixeLlist[i], 2);
            }
            result = BuildAugmentedMatrix(result, a2list);
            //a3
            double[] a3list = new double[pixeLlist.Length];
            for (int i = 0; i < pixeLlist.Length; i++)
            {
                a3list[i] = Math.Pow(pixeLlist[i], 3);
            }
            result = BuildAugmentedMatrix(result, a3list);

            return result;
        }

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

