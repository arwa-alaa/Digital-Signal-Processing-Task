using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            int sz = InputTimeDomainSignal.Samples.Count;

            List<float> Amplitudes = new List<float>();
            List<float> PhaseShifts = new List<float>();
            List<float> Frequencies = new List<float>();

            for (int k=0 ; k < sz ; k++)
            {
                Complex sum = new Complex();
                for (int n = 0 ; n<sz ; n++)
                {                
                    float ceta = ( k * n * 2 * (float)Math.PI) / sz;
                    if (ceta == 0)
                        sum += InputTimeDomainSignal.Samples[n];
                    else
                        sum +=(InputTimeDomainSignal.Samples[n] * new Complex(Math.Cos(ceta),  Math.Sin(ceta)*(-1) ));
                }

                Frequencies.Add(k);
                Amplitudes.Add((float)(sum.Magnitude));
                PhaseShifts.Add((float)sum.Phase);
                
            }

            OutputFreqDomainSignal.FrequenciesAmplitudes = Amplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = PhaseShifts;
            OutputFreqDomainSignal.Frequencies = Frequencies;

        }
    }
}
