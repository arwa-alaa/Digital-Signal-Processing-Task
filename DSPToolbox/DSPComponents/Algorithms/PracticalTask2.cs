﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            //filter input signal & save it
            FIR fir_obj = new FIR();
            fir_obj.InputTimeDomainSignal = InputSignal;
            fir_obj.InputFS = Fs;
            fir_obj.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir_obj.InputF1 = miniF;
            fir_obj.InputF2 = maxF;
            fir_obj.InputStopBandAttenuation = 50;
            fir_obj.InputTransitionBand = 500;

            fir_obj.Run();
            Signal filtered_signal = fir_obj.OutputYn;
            SaveSignal(filtered_signal, "FIR_output.txt", "time_domain");

            //if new fs doesn't destroy signal then apply sampling to signal
            Signal sampled_signal = null;
            if(newFs >= 2* maxF)
            {
                Sampling sampling = new Sampling();
                sampling.L = L;
                sampling.M = M;
                sampling.InputSignal=filtered_signal;
                sampling.Run();
                sampled_signal = sampling.OutputSignal;
            }

            //remove dc component & save result
            DC_Component dc = new DC_Component();
         
            if(sampled_signal != null)
            {
                dc.InputSignal = sampled_signal;
            }
            else
            {
                dc.InputSignal = filtered_signal;
            }
            dc.Run();
            Signal removed_dc = dc.OutputSignal;
            SaveSignal(removed_dc, "Dc_component_delete.txt", "time_domain");

            //Normalze signal from -1 to 1 & save it
            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = removed_dc;
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;
            normalizer.Run();
            Signal normalized_sig = normalizer.OutputNormalizedSignal;
            SaveSignal(normalized_sig, "OutputNormalizedSignal.txt", "time_domain");

            //apply DFT to signal
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = normalized_sig;
            if(sampled_signal != null)
            {
                dft.InputSamplingFrequency = newFs;
            }
            else
            {
                dft.InputSamplingFrequency = Fs;
            }
            dft.Run();
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;
            SaveSignal(OutputFreqDomainSignal, "OutputFrequencyDomain.txt", "frequency_domain");

        }
        public void SaveSignal(Signal res,string file_name,string type)
        {
            string hoda_path = "C:/Users/Home/Documents/Digital-Single-Processing-Task/DSPToolbox/DSPComponentsUnitTest/TestingSignals/";
            string arwa_path = "E:/7th Term/Digital Single Processing/Task/saved files/";

            string full_path = arwa_path;

            full_path += file_name;
            using(StreamWriter writer = new StreamWriter(full_path))
            {
                int n = 0;
                if(type == "time_domain")
                {
                    writer.WriteLine(0);
                    if(res.Periodic)
                       writer.WriteLine(1);
                    else
                        writer.WriteLine(0);
                    writer.WriteLine(res.Samples.Count);
                    n= res.Samples.Count;
                }
                else
                {
                    writer.WriteLine(1);
                    if (res.Periodic)
                        writer.WriteLine(1);
                    else
                        writer.WriteLine(0);
                    writer.WriteLine(res.Frequencies.Count);
                    n = res.Frequencies.Count;

                }
                for(int i=0;i <n;i++)
                {
                    if (type == "time_domain")
                    {
                        writer.Write(res.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(res.Samples[i]);
                    }
                    else
                    {
                        writer.Write(res.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(res.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(res.FrequenciesPhaseShifts[i]);
                    }

                }
                

            }
            File.Move(full_path, Path.ChangeExtension(full_path, ".ds"));
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
