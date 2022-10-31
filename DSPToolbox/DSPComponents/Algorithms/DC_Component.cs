using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sum = 0;
            for(int i=0;i< InputSignal.Samples.Count;i++)
            {
                sum += InputSignal.Samples[i];
            }
            float mean = sum / InputSignal.Samples.Count;
            List<float> ans = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                ans.Add(InputSignal.Samples[i] - mean);
            }
            OutputSignal = new Signal(ans, false);
        }
    }
}
