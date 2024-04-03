using MathNet.Numerics;
using SpectrumAnalyzerPlus.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumAnalyzerPlus.Controllers
{
    /// <summary>
    /// 用于光谱的波长校准、强度校准等
    /// </summary>
    public class SpectralCalibration
    {
        //public static (bool,double[]) WavelengthCalibration(int ranklevel,double[] data, List<double> standardWavelength)
        //{
        //    List<int> pixels = SpectralProcessing.DerivativeFindPeaks(data);
        //    double[] pixellist = new double[pixels.Count];
        //    double[] standardWavelengthlist = new double[standardWavelength.Count];
        //    double[,] coefficient = LinearAlgebra.AnotherBuildAugmentedMatrix(ranklevel, pixellist, standardWavelengthlist);
        //    // 使用高斯消元法求解
        //    double[] solution = LinearAlgebra.GaussianElimination(coefficient);

        //    //创建波长检查数组
        //    double[] wavelengthCheck = new double[data.Length];
        //    for(int i = 0; i < data.Length; i++)
        //    {
        //        for (int j = 0; j < solution.Length; j++)
        //        {
        //            wavelengthCheck[i] += solution[j] * Math.Pow(i, j);
        //        }
        //    }

        //    for(int i = 0;i < wavelengthCheck.Length - 1; i++)
        //    {
        //        if (wavelengthCheck[i + 1] - wavelengthCheck[i] < 1)
        //        {
        //            ranklevel -= 1;
        //            double[] realrank = new double[ranklevel];
        //            return (false, realrank);
        //        }
        //    }


        //    return (true,solution);
        //}

        /// <summary>
        /// 通过真实波长和像素位置进行多项式拟合并波长校准
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="pixles"></param>
        /// <param name="standardwavelengths"></param>
        /// <returns></returns>
        public static double[] WavelengthCalibrateCalculation(int rank,List<int> pixles, List<double> standardwavelengths)
        {
            //int rank = Math.Min(pixles.Count, standardwavelengths.Count);
            double[] pixellist = new double[pixles.Count];
            for (int i = 0; i < pixles.Count; i++) pixellist[i] = pixles[i];
            double[] standardwavelength = new double[standardwavelengths.Count];
            for (int i = 0; i < standardwavelengths.Count; i++) standardwavelength[i] = standardwavelengths[i];

            return Fit.Polynomial(pixellist, standardwavelength, rank);
        }

    }
}
