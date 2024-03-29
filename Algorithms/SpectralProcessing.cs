using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumAnalyzerPlus.Algorithms
{
    public class SpectralProcessing
    {
        /// <summary>
        /// 移动平均滤波算法
        /// </summary>
        /// <param name="iBoxCar"></param>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public static int[] MovingAverageSmoothedFilter(int iBoxCar, int[] spectrum)
        {
            if (spectrum == null || iBoxCar == 0 || spectrum.Length < iBoxCar)
                return spectrum;

            int[] BoxCarSpectrum = new int[spectrum.Length];
            int windowSize = 2 * iBoxCar + 1;
            int sum = 0;

            // Compute the sum of the first window
            for (int i = 0; i < windowSize; i++)
            {
                sum += spectrum[i];
            }
            BoxCarSpectrum[iBoxCar] = sum / windowSize;

            // Compute the sums for subsequent windows
            for (int i = 1; i <= spectrum.Length - windowSize; i++)
            {
                sum = sum - spectrum[i - 1] + spectrum[i + windowSize - 1];
                BoxCarSpectrum[i + iBoxCar] = sum / windowSize;
            }

            // Fill in the remaining values with the last smoothed value
            for (int i = 0; i < iBoxCar; i++)
            {
                BoxCarSpectrum[i] = BoxCarSpectrum[iBoxCar];
                BoxCarSpectrum[spectrum.Length - iBoxCar + i] = BoxCarSpectrum[spectrum.Length - iBoxCar - 1];
            }

            return BoxCarSpectrum;
        }

        /// <summary>
        /// 指数移动平均平滑滤波算法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="alpha">平滑参数，通常是在 0 到 1 之间</param>
        /// <returns></returns>
        public static double[] ExponentialMovingAverageSmoothedFilter(double[] data, double alpha)
        {
            if (data == null || data.Length == 0)
                return new double[0];

            alpha = Math.Max(0, Math.Min(1, alpha)); // 限制 alpha 在 0 到 1 之间

            double[] ema = new double[data.Length];
            ema[0] = data[0]; // 初始值为第一个数据点

            for (int i = 1; i < data.Length; i++)
            {
                ema[i] = alpha * data[i] + (1 - alpha) * ema[i - 1];
            }

            return ema;
        }


    }
}
