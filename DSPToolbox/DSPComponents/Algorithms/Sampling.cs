﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            FIR filter = new FIR();

            filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            filter.InputFS = 8000;
            filter.InputStopBandAttenuation = 50;
            filter.InputCutOffFrequency = 1500;
            filter.InputTransitionBand = 500;
            filter.InputTimeDomainSignal = new Signal(new List<float>(), false);
            OutputSignal = new Signal(new List<float>(), false);

            int j;
            if (M != 0 && L != 0)
            {
                j = 0;
                for (int i = 0; i < InputSignal.Samples.Count * L; i++)
                {

                    if (i % L == 0)
                    {
                        filter.InputTimeDomainSignal.Samples.Add(InputSignal.Samples[j]);
                        filter.InputTimeDomainSignal.SamplesIndices.Add(i);
                        j++;
                    }

                    else
                    {
                        filter.InputTimeDomainSignal.Samples.Add(0);
                        filter.InputTimeDomainSignal.SamplesIndices.Add(i);
                    }

                }
                filter.Run();

                j = 0;
                for (int i = 0; i < filter.OutputYn.Samples.Count; i++)
                {
                    if (i % M == 0)
                    {
                        OutputSignal.Samples.Add(filter.OutputYn.Samples[i]);
                        OutputSignal.SamplesIndices.Add(j);
                        j++;
                    }
                }
            }

            else if (L == 0 && M != 0) //downsampling
            {
                filter.InputTimeDomainSignal.Samples = InputSignal.Samples;
                filter.InputTimeDomainSignal.SamplesIndices = InputSignal.SamplesIndices;
                filter.Run();
                 j = 0;
                for (int i = 0; i < filter.OutputYn.Samples.Count; i++)
                { 
                    if (i % M == 0)
                    {
                        OutputSignal.Samples.Add(filter.OutputYn.Samples[i]);
                        OutputSignal.SamplesIndices.Add(j);
                        j++;
                    }
                }
            }

            else if (L != 0 && M == 0)//upsampling
            {
                j = 0;
                for (int i = 0; i < InputSignal.Samples.Count * L; i++)
                {

                    if (i % L == 0)
                    {
                        filter.InputTimeDomainSignal.Samples.Add(InputSignal.Samples[j]);
                        filter.InputTimeDomainSignal.SamplesIndices.Add(i);
                        j++;
                    }

                    else
                    {
                        filter.InputTimeDomainSignal.Samples.Add(0);
                        filter.InputTimeDomainSignal.SamplesIndices.Add(i);
                    }

                }
                filter.Run();
                OutputSignal = filter.OutputYn;
            }

        }
    }
}
