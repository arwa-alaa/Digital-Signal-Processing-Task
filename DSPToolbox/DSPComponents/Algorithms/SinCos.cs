using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
           double w = (2 * (Math.PI) * (AnalogFrequency / SamplingFrequency));
            float Xi;
            samples = new List<float>();
            

            for (int i=0;i< SamplingFrequency; i++)
            {
                    if (type == "sin")
                    {
                        Xi = (float)(A * Math.Sin(w * i + PhaseShift));
                    }
                    else
                    {
                        Xi = (float)(A * Math.Cos(w * i + PhaseShift));
                    }
                        
                    samples.Add(Xi);
            }
          
        }
    }
}
