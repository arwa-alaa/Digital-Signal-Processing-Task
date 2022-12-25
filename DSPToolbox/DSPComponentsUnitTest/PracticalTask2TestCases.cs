using DSPAlgorithms.Algorithms;
using DSPAlgorithms.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPComponentsUnitTest
{
    [TestClass]
    public class PracticalTask2TestCases
    {
        [TestMethod]
        public void PracticalTask2TestCaseECG()
        {
            PracticalTask2 T2_Obj = new PracticalTask2();
            string hoda_path = "C:/Users/Home/Documents/Digital-Single-Processing-Task/DSPToolbox/DSPComponentsUnitTest/TestingSignals/ecg400.ds";
            string arwa_path = "E:/7th Term/Digital Single Processing/Task/Digital-Single-Processing-Task/DSPToolbox/DSPComponentsUnitTest/TestingSignals/ecg400.ds";
            T2_Obj.SignalPath = arwa_path;
            T2_Obj.miniF = 150;
            T2_Obj.maxF = 250;
            T2_Obj.Fs = 1000;
            T2_Obj.newFs = 500;
            T2_Obj.L = 0;
            T2_Obj.M = 3;

            var signal = UnitTestUtitlities.LoadSignal(T2_Obj.SignalPath);
            T2_Obj.Run();
            Signal Res = T2_Obj.OutputFreqDomainSignal;

            arwa_path = "E:/7th Term/Digital Single Processing/Task/Digital-Single-Processing-Task/DSPToolbox/DSPComponentsUnitTest/TestingSignals/FileDown.ds";
            hoda_path = "C:/Users/Home/Documents/Digital-Single-Processing-Task/DSPToolbox/DSPComponentsUnitTest/TestingSignals/FileDown.ds";
            var expectedOutput = UnitTestUtitlities.LoadSignal(arwa_path);

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, Res.FrequenciesAmplitudes)
                && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, Res.FrequenciesPhaseShifts));
        }
    }
}