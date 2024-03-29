using Microsoft.VisualBasic;
using System;
using SpectrumAnalyzerPlus.Algorithms;

namespace SpectrumAnalyzerPlus
{
    class TestProgram
    {
        static void Main()
        {
            // 构建增广矩阵

            //double[] pixellist = {234.5,394.5,538.5,648,735,939.5,1052,1145};
            double[] pixellist = { 171.5,564,805,876.5 };
            //double[,] matrix = BuildMatrix(4,pixellist);
            //double[] wavelength = { 235, 313, 365, 404,435,546,579 ,763};
            double[] wavelength = { 253,435,546,576};

            //new way
            int ranklevel = 3;
            double[,] coefficient = LinearAlgebra.AnotherBuildAugmentedMatrix(ranklevel, pixellist, wavelength);
            PrintMatrix(coefficient);

            // 使用高斯消元法求解
            double[] solution = LinearAlgebra.GaussianElimination(coefficient);

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

    }
}

