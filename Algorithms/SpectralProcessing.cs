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
        /// 平滑算法
        /// </summary>
        /// <param name="iBoxCar"></param>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public static int[] SpectraBoxCar(int iBoxCar, int[] spectrum)
        {
            if (spectrum == null || iBoxCar == 0 || spectrum.Length < iBoxCar)
                return spectrum;

            int[] BoxCarSpectrum = new int[spectrum.Length];
            int windowSize = 2 * iBoxCar + 1;

            // Compute the sum of the first window
            int sum = 0;
            for (int i = 0; i < windowSize; i++)
            {
                sum += spectrum[i];
            }
            BoxCarSpectrum[iBoxCar] = sum / windowSize;

            // Compute the sums for subsequent windows
            for (int i = iBoxCar + 1; i < spectrum.Length - iBoxCar; i++)
            {
                sum = sum - spectrum[i - iBoxCar - 1] + spectrum[i + iBoxCar];
                BoxCarSpectrum[i] = sum / windowSize;
            }

            // Copy the remaining spectrum values
            for (int i = 0; i < iBoxCar; i++)
            {
                BoxCarSpectrum[i] = spectrum[i];
                BoxCarSpectrum[spectrum.Length - iBoxCar + i] = spectrum[spectrum.Length - iBoxCar + i];
            }

            return BoxCarSpectrum;
        }
    }
}
