using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumAnalyzerPlus.Algorithms
{
    /// <summary>
    /// 光谱数据的处理算法
    /// </summary>
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

        /// <summary>
        /// 导数寻峰算法
        /// </summary>
        /// <param name="data">原始光谱数据</param>
        /// <returns>峰对应的序号</returns>
        public static List<int> DerivativeFindPeaks(double[] data)
        {
            List<int> peakList = new List<int>();

            // Calculate average intensity
            double averageIntensity = data.Average()+100;

            // Calculate derivative and find peaks
            //for (int i = 0; i < data.Length - 2; i++)
            //{
            //    // Calculate derivative
            //    double derivative = data[i + 1] - data[i];
            //    double nextDerivative = data[i + 2] - data[i + 1];

            //    // Check if it's a peak
            //    if (derivative * nextDerivative < 0 && data[i] > averageIntensity && data[i + 1] > averageIntensity)
            //    {
            //        peakList.Add(i);
            //    }
            //}

            double[] hills = new double[data.Length];
            // Calculate derivative and find peaks
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > averageIntensity) hills[i] = 1;else hills[i] = 0;
            }

            double hillstart = -1;
            double hillend = -1;
            for(int i = 0;i<hills.Length;i++)
            {
                if (hills[i] == 1 && hillstart == -1)
                {
                    hillstart = i;
                }
                if (hills[i]== 0 && hillstart != -1)
                {
                    hillend = i;
                    peakList.Add((int)(hillstart + hillend) / 2);
                    hillstart = -1;
                    hillend = -1;
                }
            }

            return peakList;
        }

        public static List<int> DerivativeFindPeaks(int[] data)
        {
            // Convert int[] to double[]
            double[] doubleData = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                doubleData[i] = data[i];
            }

            // Call existing DerivativeFindPeaks function for double[]
            return DerivativeFindPeaks(doubleData);
        }
    }
}
