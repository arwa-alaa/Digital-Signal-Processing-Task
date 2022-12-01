using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            
            double sum1_sqr = 0.000f, sum2_sqr = 0.000f, normalize = 0.000f;
            OutputNormalizedCorrelation = new List<float>();
            OutputNonNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;

            }
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            Signal output1 = new Signal(new List<float>(), false);
            

            dft.InputTimeDomainSignal = new Signal(InputSignal1.Samples,false);
            dft.Run();
            Signal x1 = dft.OutputFreqDomainSignal;

            dft.InputTimeDomainSignal = new Signal(InputSignal2.Samples, false);
            dft.Run();
            Signal x2 = dft.OutputFreqDomainSignal;

            int n = InputSignal1.Samples.Count;
            Complex[] components1 = new Complex[n];
            Complex[] components2 = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                float A1 = x1.FrequenciesAmplitudes[i];
                float PhaseShift1 = x1.FrequenciesPhaseShifts[i];
                components1[i] = new Complex(A1 * Math.Cos(PhaseShift1), A1 * Math.Sin(PhaseShift1));
              //  Console.WriteLine(i);
                float A2 = x2.FrequenciesAmplitudes[i];
                float PhaseShift2 = x2.FrequenciesPhaseShifts[i];
                components2[i] = new Complex(A2 * Math.Cos(PhaseShift2), (A2 * Math.Sin(PhaseShift2)));
                components2[i] = Complex.Conjugate(components2[i]);

            }
            output1.FrequenciesAmplitudes = new List<float>();
            output1.FrequenciesPhaseShifts = new List<float>();
            for (int j=0; j< n;j++)
            {
                sum1_sqr += (InputSignal1.Samples[j] * InputSignal1.Samples[j]);
                sum2_sqr += (InputSignal2.Samples[j] * InputSignal2.Samples[j]);

                Complex c = components1[j] * components2[j];
                output1.FrequenciesPhaseShifts.Add((float)c.Phase);
                output1.FrequenciesAmplitudes.Add((float)c.Magnitude);
                
            }
            normalize = Math.Sqrt(sum1_sqr * sum2_sqr) / n;
            idft.InputFreqDomainSignal = output1;
            idft.Run();

            for (int i = 0; i < n; i++)
            {
                double ans = idft.OutputTimeDomainSignal.Samples[i] / n;
                OutputNonNormalizedCorrelation.Add((float)ans);
                OutputNormalizedCorrelation.Add((float)(ans/normalize));
                Console.WriteLine(OutputNormalizedCorrelation[i]);
            }



        }
    }
}