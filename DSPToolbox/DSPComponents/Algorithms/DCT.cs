using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);

            int sz = InputSignal.Samples.Count;

            for(int k =0 ; k<sz ; k++)
            {
                float sum = 0;
                for(int n=0 ; n < sz; n++)
                {
                    float ceta = ((float)Math.PI / (4 * sz )) * (2 * n - 1) * (2 * k - 1);
                    sum += (float)(InputSignal.Samples[n] * Math.Cos(ceta));
                }
               OutputSignal.Samples.Add(sum*(float)Math.Sqrt(2.0f/sz)); 
            }
        }
    }
}
