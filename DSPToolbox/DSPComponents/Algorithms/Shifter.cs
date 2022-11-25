using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {

            OutputShiftedSignal = new Signal(new List<float>(),new List<int>(), InputSignal.Periodic);

            int sz = InputSignal.Samples.Count;
               
            if (InputSignal.Periodic)
            {
                //int start =;
                for (int i = 0; i < sz; i++)
                {
                    OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                    
                }
            }
            else 
            {
                //int start = InputSignal.SamplesIndices[0] - ShiftingValue;
                for (int i = 0; i < sz; i++)
                {
                    OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                    
                }
            }



            OutputShiftedSignal.Samples = InputSignal.Samples;

            
        }
    }
}
