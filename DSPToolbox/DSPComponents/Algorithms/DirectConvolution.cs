using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int f_indx=10000;
            bool sample_indecies = false;
            if (InputSignal1.SamplesIndices.Count !=0)
            {
                OutputConvolvedSignal = new Signal(new List<float>(),new List<int>(), false);
                f_indx = InputSignal1.SamplesIndices[0]+ InputSignal2.SamplesIndices[0];
                sample_indecies = true;
            }
            else
            {
                OutputConvolvedSignal = new Signal(new List<float>(), false);
            }
           
            int n1= InputSignal1.Samples.Count,n2 = InputSignal2.Samples.Count;
            for(int i =0;i< (n1+n2) - 1;i++)
            {
                float convolve = 0;
                for(int j=0;j<n1;j++)
                {
                    if( i - j>=0 && i - j<n2)
                    {
                        convolve += InputSignal1.Samples[j] * InputSignal2.Samples[i - j];
                    }
                }
                if (i == (n1 + n2) - 2 && convolve == 0)
                    continue;
                OutputConvolvedSignal.Samples.Add(convolve);
                if(sample_indecies)
                {
                    OutputConvolvedSignal.SamplesIndices.Add(f_indx + i);
                }
            }
        }
    }
}
