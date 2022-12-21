using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {

            float delta_F = InputTransitionBand / InputFS;

            //first Know which window we will use & calculate N
            string window_name = "";
            int N = 0;
            
            if (InputStopBandAttenuation <= 21)
            {
                window_name = "rectangular";
                N = (int)Math.Ceiling(0.9f / (InputTransitionBand/ InputFS));
            }
            else if (InputStopBandAttenuation <= 44)
            {
                window_name = "hannen";
                N = (int)Math.Ceiling(3.1f / (InputTransitionBand / InputFS));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                window_name = "hammen";
                N = (int)Math.Ceiling(3.3f / (InputTransitionBand / InputFS));
            }
            else
            {
                window_name = "blackman";
                N = (int)Math.Ceiling(5.5f / (InputTransitionBand / InputFS));
            }


            if (N % 2 == 0)
                N += 1;    //round to nearest odd num
          
            ////////////////////////////////////////////////////////////////////////
            //calculate ideal impulse filter

            List<float?> hn = new List<float?>();

            if(InputFilterType == FILTER_TYPES.LOW)
            {
                //get fc dash & normalized it
                InputCutOffFrequency = InputCutOffFrequency + (InputTransitionBand / 2);
                InputCutOffFrequency /= InputFS;
                for(int i =0;i<=N/2;i++)
                {
                    if(i == 0)
                    {
                        hn.Add((InputCutOffFrequency * 2));
                    }
                    else
                    {
                        float wci = (float)(i * 2 * Math.PI * InputCutOffFrequency);
                        hn.Add((float)((2 * InputCutOffFrequency * Math.Sin(wci)) / wci));
                    }
                }

            }
            else if(InputFilterType == FILTER_TYPES.HIGH)
            {
                //get fc dash & normalized it
                InputCutOffFrequency = InputCutOffFrequency - (InputTransitionBand / 2);
                InputCutOffFrequency /= InputFS;
                for (int i = 0; i <= N / 2; i++)
                {
                    if (i == 0)
                    {
                        hn.Add((1 - (InputCutOffFrequency * 2)));
                    }
                    else
                    {
                        float wci = (float)(i * 2 * Math.PI * InputCutOffFrequency);
                        hn.Add((float)(((-2 * InputCutOffFrequency) * Math.Sin(wci) )/ wci));
                    }
                }
            }
            else if(InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                float  fc1_ = (float) (InputF1 - InputTransitionBand / 2) / InputFS;
                float  fc2_ = (float) (InputF2 + InputTransitionBand / 2) / InputFS;
      
                for (int i=0; i<= N/2;i++)
                {
                    if (i == 0)
                    {
                        hn.Add((float)(2 * (fc2_ - fc1_)));
                        continue;
                    }

                    float w1 = (float)( i *2 * Math.PI * fc1_);
                    float w2 = (float)(i * 2 * Math.PI * fc2_);


                    float bp = (float)((2 * fc2_ * Math.Sin(w2) / w2) - (2 * fc1_ * Math.Sin(w1) / w1));
                    hn.Add(bp);
                }
            }
            else
            {
                float fc1_ = (float)((InputF1 + InputTransitionBand / 2) / InputFS);
                float fc2_ = (float)((InputF2 - InputTransitionBand / 2) / InputFS);

                for (int i = 0; i <= N / 2; i++)
                {
                    if (i == 0)
                    {
                        hn.Add((float)(1 - 2 * (fc2_ - fc1_)));
                        continue;
                    }

                    float w1 = (float)(i * 2 * Math.PI * fc1_);
                    float w2 = (float)(i * 2 * Math.PI * fc2_);

                    float bs = (float)((2 * fc1_ * Math.Sin(w1) / w1) - (2 * fc2_ * Math.Sin(w2) / w2));
                    hn.Add(bs);
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////
            //calculate window function

            OutputHn = new Signal(new List<float>(), false);
            float[] samples = new float[N];
            int[] indecies = new int[N];

            if (window_name == "hannen")
            {
                int j = 0;
                for (int i = N / 2; i < N; i++)
                {
                    float ceta = (float)(2 * Math.PI * j) / N;
                    float wn = (float)(0.5 + (0.5 * Math.Cos(ceta)));
                    samples[i] = (float)hn[j] * wn;
                    indecies[i] = j;
                    if (j >= 1)
                    {
                        samples[N / 2 - j] = (float)hn[j] * wn;
                        indecies[N / 2 - j] = -j;
                    }
                    j++;
                }
            }

            else if (window_name== "hammen")
            {
                int j = 0;
                for(int i=N/2;i<N;i++)
                {
                    float ceta = (float)(2 * Math.PI * j) / N;
                    float wn =(float)( 0.54 + (0.46 * Math.Cos(ceta)));
                    samples[i] = (float)hn[j] * wn;
                    indecies[i] = j;
                    if (j >= 1)
                    {
                        samples[N/2 - j] = (float)hn[j] * wn;
                        indecies[N/2 - j] = -j;
                    }
                    j++;
                }
            }
           
            else if(window_name == "blackman")
            {
                int j = 0;
                for (int i = N / 2; i < N; i++)
                {
                    float ceta1 = (float)(2 * Math.PI * j) / (N -1);
                    float ceta2 = (float)(4 * Math.PI * j) / (N - 1);
                    float wn = (float)(0.42 + (0.5 * Math.Cos(ceta1))+(0.08 * Math.Cos(ceta2)));

                    samples[i] = (float)hn[j] * wn;
                    indecies[i] = j;
                    if (j >= 1)
                    {
                        samples[N / 2 - j] = (float)hn[j] * wn;
                        indecies[N / 2 - j] = -j;
                    }

                    j++;

                }
            }


            OutputHn.Samples = samples.ToList();
            OutputHn.SamplesIndices = indecies.ToList();

            DirectConvolution dc = new DirectConvolution();

            dc.InputSignal1 = OutputHn;
            dc.InputSignal2 = InputTimeDomainSignal;
            dc.InputSignal2.SamplesIndices = InputTimeDomainSignal.SamplesIndices;
            
            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;
         
        }
    }
}
