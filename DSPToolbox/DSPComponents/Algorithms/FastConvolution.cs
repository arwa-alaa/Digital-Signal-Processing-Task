using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;



namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), false);

            int sz1 = InputSignal1.Samples.Count;
            int sz2 = InputSignal2.Samples.Count;
            int sz = sz1 + sz2 - 1;

            for (int i = sz1; i < sz; i++)
                InputSignal1.Samples.Add(0);

            for (int i = sz2; i < sz; i++)
                InputSignal2.Samples.Add(0);

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            dft1.InputTimeDomainSignal = new Signal(InputSignal1.Samples, false);
            dft1.Run();
            Signal output1 = dft1.OutputFreqDomainSignal;

            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = new Signal(InputSignal2.Samples, false);
            dft2.Run();
            Signal output2 = dft2.OutputFreqDomainSignal;



            Complex[] components1 = new Complex[sz];
            Complex[] components2 = new Complex[sz];

            for (int i = 0; i < sz; i++)
            {
                float A1 = output1.FrequenciesAmplitudes[i];
                float PhaseShift1 = output1.FrequenciesPhaseShifts[i];
                components1[i] = new Complex(A1 * Math.Cos(PhaseShift1), A1 * Math.Sin(PhaseShift1));

                float A2 = output2.FrequenciesAmplitudes[i];
                float PhaseShift2 = output2.FrequenciesPhaseShifts[i];
                components2[i] = new Complex(A2 * Math.Cos(PhaseShift2), (A2 * Math.Sin(PhaseShift2)));


            }

            double sum1_sqr = 0.000f, sum2_sqr = 0.000f;
            output1.FrequenciesAmplitudes = new List<float>();
            output1.FrequenciesPhaseShifts = new List<float>();
            for (int j = 0; j < sz; j++)
            {
                sum1_sqr += (InputSignal1.Samples[j] * InputSignal1.Samples[j]);
                sum2_sqr += (InputSignal2.Samples[j] * InputSignal2.Samples[j]);

                Complex c = components1[j] * components2[j];
                output1.FrequenciesPhaseShifts.Add((float)c.Phase);
                output1.FrequenciesAmplitudes.Add((float)c.Magnitude);

            }
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = output1;
            idft.Run();

            OutputConvolvedSignal = idft.OutputTimeDomainSignal;
        }
    }
}
