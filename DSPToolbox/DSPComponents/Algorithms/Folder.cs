using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public static int folding = 0;

        public override void Run()
        {
            OutputFoldedSignal = new Signal(new List<float>(), false);

            folding++;

            for (  int i=0;i< InputSignal.Samples.Count; i++)
            {
                OutputFoldedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i]);
            }

            //InputSignal.Samples.Reverse();
            //OutputFoldedSignal.Samples = InputSignal.Samples;

            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
                OutputFoldedSignal.Samples.Add(InputSignal.Samples[i]);
            

            if (InputSignal.Periodic)
                OutputFoldedSignal.Periodic = false;
            else
                OutputFoldedSignal.Periodic = true;

        }
    }
}
