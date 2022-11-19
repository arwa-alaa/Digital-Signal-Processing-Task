using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            FirstDerivative = new Signal(new List<float>(), false);
            SecondDerivative = new Signal(new List<float>(), false);
            float fderivative = 0, sderivative = 0;
            int n = InputSignal.Samples.Count;
            for (int i =1;i< n - 1;i++)
            {
                fderivative = InputSignal.Samples[i] - InputSignal.Samples[i - 1];
                sderivative = InputSignal.Samples[i + 1] - 2 * InputSignal.Samples[i] + InputSignal.Samples[i - 1];
                FirstDerivative.Samples.Add(fderivative);
                SecondDerivative.Samples.Add(sderivative);
            }
            fderivative= InputSignal.Samples[n-1] - InputSignal.Samples[n - 2];
            FirstDerivative.Samples.Add(fderivative);
        }
    }
}
