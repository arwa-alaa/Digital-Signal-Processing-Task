using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            int count1 = InputSignal1.Samples.Count;
            int count2 = InputSignal2.Samples.Count;
            if (count1 > count2)
            {
                for (int i = count2; i < count1; i++)
                    InputSignal2.Samples.Add(0);
                
            }
            else
            {
                for (int i = count1; i < count2; i++)
                    InputSignal1.Samples.Add(0);
                
            }
            List<float> outsig = new List<float>();
            for(var i=0;i< InputSignal1.Samples.Count;i++)
            {
                outsig.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);
            }
            OutputSignal = new Signal(outsig, false);

            

        }
    }
}