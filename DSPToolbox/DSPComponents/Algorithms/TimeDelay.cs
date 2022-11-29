using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            DirectCorrelation dc = new DirectCorrelation();
            dc.InputSignal1 = new Signal(InputSignal1.Samples, false);
            dc.InputSignal2 = new Signal(InputSignal2.Samples, false);
            dc.Run();

            float mx = -1000.00f;
            int indx_mx = 0;
            for(int i=0; i< dc.OutputNormalizedCorrelation.Count;i++)
            {
                if (dc.OutputNormalizedCorrelation[i] > mx)
                {
                    mx = dc.OutputNormalizedCorrelation[i];
                    indx_mx = i;
                }
            }
            OutputTimeDelay = indx_mx * InputSamplingPeriod;
        }
    }
}
