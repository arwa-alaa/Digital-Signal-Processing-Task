using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            
            // new lists
            OutputQuantizedSignal = new Signal(new List<float>(), false);
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();

            //num of levels & bits
            if (InputLevel <= 0)
            {
                InputLevel = 1;
                 for (int i=1;i<= InputNumBits;i++)
                {
                    InputLevel *= 2;
                }
                
            }
            if(InputNumBits <= 0)
            {
                InputNumBits = (int) Math.Log(InputLevel, 2);
            }


            //find max , min and resultion
            float mx = -10000000.0f;
            float mn =  10000000.0f;

            for(int i =0 ; i< InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] > mx)
                    mx = InputSignal.Samples[i];

                if (InputSignal.Samples[i] < mn)
                    mn = InputSignal.Samples[i];
            }

 
            float resultion = (mx - mn) / InputLevel;

            //find midpoints 
            var range = new List<KeyValuePair<float, float>>();

            Dictionary< float,int> midpoints = new Dictionary<float,int>();

            float a, b = mn , midpoint;
            for(int i =0; i< InputLevel; i++)
            {
                a = b + resultion;
                midpoint = (a + b) / 2;
                midpoints.Add(midpoint,i+1);

                range.Add(new KeyValuePair<float, float>(b,a));
               
                b = a;
            }

            //add in lists
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < range.Count; j++)
                {
                    if (InputSignal.Samples[i] >= range[j].Key && InputSignal.Samples[i] < range[j].Value)
                    {
                        float midp = (range[j].Key + range[j].Value) / 2;
                        OutputQuantizedSignal.Samples.Add(midp);
                        OutputIntervalIndices.Add(midpoints[midp]);
         
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        OutputSamplesError.Add ( OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);
                 
                        break;
                    }
                    else if (InputSignal.Samples[i] >= range[range.Count-1].Value)
                    {
                        float midp = (range[range.Count - 1].Key + range[range.Count - 1].Value) / 2;

                        OutputQuantizedSignal.Samples.Add(midp);
                        OutputIntervalIndices.Add(InputLevel);

                        OutputEncodedSignal.Add(Convert.ToString(InputLevel-1, 2).PadLeft(InputNumBits, '0'));
                        OutputSamplesError.Add(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);

                        break;
                    }
                }
            }
        }
    }
}
