using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCalc;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ParseModel
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = @"D:\skype\";
            string modelFile = rootPath + "model.txt";
            int numOfTest = 5;

            string[] testFile = new string[numOfTest];
            string[] model = File.ReadAllLines(modelFile);

            string[] formula = model[0].Split('=');

            string input = formula[1].Replace("sin", "Sin")
                .Replace("cos", "Cos");

            ArrayList al = new ArrayList();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '+' || input[i] == '-' || input[i] == '*' || input[i] == '/' || input[i] == '%')
                {
                    al.Add(input[i]);
                }
            }

            string[] sArray = input.Split(new char[5]{'+','-','*','/','%'});
            //float offset = float.Parse(sArray[0]);

            string equation = "";
            for (int i = 0; i < al.Count; i++)
            {
                if (sArray[i].Contains('^'))
                {
                    //Console.WriteLine("modify");
                    string[] pow = sArray[i].Split('^');
                    equation += "Pow(" + pow[0] + "," + pow[1] + ")" + al[i];
                }
                else
                {
                    equation += sArray[i] + " " + al[i];
                }
            }

            equation += sArray[sArray.Length - 1];
            float avgEnergy = 0;
            TextWriter tw = new StreamWriter(rootPath + "energy.txt");
            for (int n = 0; n < numOfTest; n++)
            {
                
                string[] sampleFile = File.ReadAllLines(rootPath + (n + 1) + @"\sample.txt");
                string[] samVars = sampleFile[0].Split('\t');

                string[] currFile = File.ReadAllLines(rootPath + (n + 1) + @"\test.txt");
                string[] varNames2 = currFile[0].Split('\t');

                string[] varNames = new string[varNames2.Length + 2];
                for (int i = 0; i < varNames2.Length; i++)
                {
                    varNames[i] = varNames2[i];
                }

                varNames[varNames2.Length] = "tx_pk";
                varNames[varNames2.Length + 1] = "rx_pk";

                Expression e = new Expression(equation);
                float energy = 0;

                for (int i = 1; i < currFile.Length; i++)
                {

                    string[] lineValue2 = currFile[i].Split('\t');
                    string[] lineValue = new string[lineValue2.Length + 2];
                    for (int p = 0; p < lineValue2.Length; p++)
                    {
                        lineValue[p] = lineValue2[p];
                    }

                    if (i < sampleFile.Length)
                    {
                        string[] x = sampleFile[i].Split('\t');
                        lineValue[lineValue2.Length] = x[19];
                        lineValue[lineValue2.Length + 1] = x[17];
                    }
                    else
                    {
                        lineValue[lineValue2.Length] = "0";
                        lineValue[lineValue2.Length + 1] = "0";
                    }

                    for (int j = 0; j < varNames.Length; j++)
                    {
                        e.Parameters[varNames[j]] = Double.Parse(lineValue[j].ToString());
                    }

                    if (e.HasErrors())
                    {
                        Console.WriteLine("e has error " + e.Error);
                    }

                    try
                    {
                        Console.WriteLine(e.Evaluate());
                        energy += float.Parse(e.Evaluate().ToString())/1000;
                    }
                    catch (EvaluationException ee)
                    {
                        Console.WriteLine("Error catched: " + ee.Message);
                    }

                }
                Console.WriteLine("Test " + (n+1) + " = " + energy);
                tw.WriteLine("Test " + (n+1) + " = " + energy);
                //Console.ReadKey();
                avgEnergy += energy;
            }

            Console.WriteLine("average energy = " + (avgEnergy / 5));
            //Console.ReadKey();

           
            tw.WriteLine("Average energy " + avgEnergy / 5 + " joules");
            tw.Close();
        }
    }
}
