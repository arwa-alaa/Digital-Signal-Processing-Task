using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            double sum1_sqr = 0.000f, sum2_sqr = 0.000f, normalize = 0.000f;
            float correlation_sum = 0;
            int n = InputSignal1.Samples.Count;
            OutputNormalizedCorrelation = new List<float>();
            OutputNonNormalizedCorrelation= new List<float>();
            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;
               
            }
            for (int i=0;i< n;i++)
            {
               /// Console.WriteLine(InputSignal2.Samples.Count);
                /*if (InputSignal2.Periodic && i != 0) // shift left
                {
                    float temp = InputSignal2.Samples[0];
                    
                    InputSignal2.Samples.RemoveAt(0);
                    
                    InputSignal2.Samples.Add(temp);
                  

                }
                if (!InputSignal2.Periodic && i!=0)//add zeros
                {
                    InputSignal2.Samples.RemoveAt(0);
                    InputSignal2.Samples.Add(0);
                    //Console.WriteLine(InputSignal2.Samples.Count);
                }*/
                correlation_sum= 0;
                for (int j=0;j<n;j++)
                {
                    if(i==0)
                    {
                        sum1_sqr += Math.Pow(InputSignal1.Samples[j],2);
                        sum2_sqr += Math.Pow(InputSignal2.Samples[j],2);
                    }
                    int iplusj = i + j;
                    if (InputSignal2.Periodic) // shift left
                    {
                        if (iplusj >= n)
                            iplusj %=n;
                      
                         correlation_sum += InputSignal1.Samples[j] * InputSignal2.Samples[iplusj];
                    }
                    else
                    {
                        if (iplusj >= n)
                            correlation_sum += 0;
                        else
                           correlation_sum += InputSignal1.Samples[j] * InputSignal2.Samples[iplusj];
                    }
                       
                    //Console.WriteLine(InputSignal2.Samples[j]);
                }
                if(i==0)
                {
                    normalize = (Math.Sqrt(sum1_sqr * sum2_sqr))/n;
                }

               
                correlation_sum /=n;
                OutputNonNormalizedCorrelation.Add((float)correlation_sum);
                OutputNormalizedCorrelation.Add((float)(correlation_sum / normalize));
                
                //Console.WriteLine(OutputNonNormalizedCorrelation[i]);
            }
        }
    }
}