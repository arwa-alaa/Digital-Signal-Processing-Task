using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            
            int k = InputFreqDomainSignal.Frequencies.Count;
            Complex[] components = new Complex[k];

            for (int i = 0; i < k; i++)
            {
                float A = InputFreqDomainSignal.FrequenciesAmplitudes[i];
                float PhaseShift = InputFreqDomainSignal.FrequenciesPhaseShifts[i];
                components[i] = new Complex(A * Math.Cos(PhaseShift), A * Math.Sin(PhaseShift));
                
            }
            List<float> samples = new List<float>();
            for (int i = 0; i < k; i++)
            {
                Complex sum = new Complex();
                for (int j = 0; j < k; j++)
                {
                    float ceta = (i * j * 2 * (float)Math.PI) / k;
                    if (ceta == 0)
                        sum += components[j];
                    else
                        sum += (components[j] * new Complex(Math.Cos(ceta), Math.Sin(ceta)));

                    
                }
                
                samples.Add((float)(sum.Real)/k);
                //Console.WriteLine(samples[i]);
            }

            OutputTimeDomainSignal = new Signal(samples, false);
        }
    }
}
