using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();

            List<float> ans = new List<float>(); ;

            for (int i=0; i< InputSignals[0].Samples.Count;i++)
            {
                ans.Add(InputSignals[0].Samples[i] + InputSignals[1].Samples[i]);
            }

            OutputSignal  = new Signal(ans,false);

        }
    }
}