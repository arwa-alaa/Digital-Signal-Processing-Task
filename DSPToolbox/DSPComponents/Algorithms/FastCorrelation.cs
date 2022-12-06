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
      
            output1.FrequenciesAmplitudes = new List<float>();
            output1.FrequenciesPhaseShifts = new List<float>();

            for (int i = 0; i < n; i++)
            {
                //to calculate normalize equation
                sum1_sqr += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
                sum2_sqr += (InputSignal2.Samples[i] * InputSignal2.Samples[i]);

                //calculate complex using phaseShift , amplitude
                float A1 = x1.FrequenciesAmplitudes[i];
                float PhaseShift1 = x1.FrequenciesPhaseShifts[i];
                Complex comp1 = new Complex(A1 * Math.Cos(PhaseShift1), A1 * Math.Sin(PhaseShift1));
                comp1 = Complex.Conjugate(comp1);

                float A2 = x2.FrequenciesAmplitudes[i];
                float PhaseShift2 = x2.FrequenciesPhaseShifts[i];
                Complex comp2 = new Complex(A2 * Math.Cos(PhaseShift2), (A2 * Math.Sin(PhaseShift2)));



                // calculate complex multiply and save phase &magnitude to send it to idft
                Complex c = comp1 * comp2;
                output1.FrequenciesPhaseShifts.Add((float)c.Phase);
                output1.FrequenciesAmplitudes.Add((float)c.Magnitude);
            }
        
          
            normalize = Math.Sqrt(sum1_sqr * sum2_sqr) / n;
            idft.InputFreqDomainSignal = output1; //Get samples list from idft
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