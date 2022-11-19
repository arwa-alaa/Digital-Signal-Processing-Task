using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            OutputAverageSignal = new Signal(new List<float>(), false);
            float avg = 0;
            bool out_of_range = false;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = i; j < i + InputWindowSize; j++)
                {
                    if(j == InputSignal.Samples.Count)
                    {
                        out_of_range = true;
                        break;
                    }
                    avg += InputSignal.Samples[j];
                }
                if (out_of_range)
                    break;
                avg = avg / InputWindowSize;
                OutputAverageSignal.Samples.Add(avg);
                avg = 0;

            }
          
            Console.WriteLine(OutputAverageSignal.Samples.Count);
        }  
    }
}
