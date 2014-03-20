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
            string rootPath = @"D:\[BACKUP][POWER][20140220]\SamsungS3\line\voice\";
            //string rootPath2 = @"D:\skype_idle\";
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

                    if (!pow[0].Contains('('))
                        equation += "Pow(" + pow[0] + "," + pow[1] + ")";
                    else
                    {
                        string[] sub1 = pow[0].Split('(');
                        string[] sub2 = pow[1].Split(')');

                        equation += sub1[0] + "(" + "Pow(" + sub1[1] + "," + sub2[0] + "))";
                    }

                    equation += " " + al[i];
                }
                else
                {
                    equation += sArray[i] + " " + al[i];
                }
            }

            if (sArray[sArray.Length - 1].Contains('^'))
            {
                //Console.WriteLine("modify");
                string[] pow = sArray[sArray.Length - 1].Split('^');

                if (!pow[0].Contains('('))
                    equation += "Pow(" + pow[0] + "," + pow[1] + ")";
                else
                {
                    string[] sub1 = pow[0].Split('(');
                    string[] sub2 = pow[1].Split(')');

                    equation += sub1[0] + "(" + "Pow(" + sub1[1] + "," + sub2[0] + "))";
                }
            }
            else
            {
                equation += sArray[sArray.Length - 1];
            }

            float avgEnergy = 0;
            double avgTotalEnergy = 0;
            double avgAsyncEnergy = 0;

            TextWriter tw = new StreamWriter(rootPath + "energy.txt");
            
            for (int n = 0; n < numOfTest; n++)
            {
                
                string[] sampleFile = File.ReadAllLines(rootPath + (n + 1) + @"\sample.txt");
                string[] samVars = sampleFile[0].Split('\t');
                if (samVars.Length == 1)
                    samVars = sampleFile[0].Split(' ');

                string[] currFile = File.ReadAllLines(rootPath + (n + 1) + @"\test.txt");
                string[] varNames = currFile[0].Split('\t');
                if (varNames.Length == 1)
                    varNames = currFile[0].Split(' ');

                Expression e = new Expression(equation);
                float energy = 0;
                double totalEnergy = 0;
                
                //Calculate total energy (joule)
                for (int k = 1; k < sampleFile.Length; k++)
                {
                    string[] sampleValues = sampleFile[k].Split('\t');
                    if (sampleValues.Length == 1)
                        sampleValues = sampleFile[k].Split(' ');

                    double power = double.Parse(sampleValues[sampleValues.Length - 1]) / 1000.0;
                    totalEnergy += power;
                }

                //Calculate asynchronous energy (joule)
                double asyncTotal = 0;
                if (n == 0)
                {
                    string[] asyncFile = File.ReadAllLines(rootPath + (n + 1) + @"\asyncTable.txt");
                   
                    for (int aNum = 1; aNum < asyncFile.Length; aNum++)
                    {
                        string line = asyncFile[aNum];
                        string[] lines = line.Split('\t');
                        if (lines.Length == 1)
                            lines = line.Split(' ');

                        asyncTotal += (double.Parse(lines[1]) / 1000);
                    }
                }


                for (int i = 1; i < currFile.Length; i++)
                {

                   string[] lineValue = currFile[i].Split('\t');

                   if (lineValue.Length == 1)
                       lineValue = currFile[i].Split(' ');

                    for (int j = 0; j < varNames.Length; j++)
                    {
                        double value = Double.Parse(lineValue[j].ToString());
                        if (value < 0) value = 0;

                        e.Parameters[varNames[j]] = value;
                    }

                    if (e.HasErrors())
                    {
                        Console.WriteLine("e has error " + e.Error);
                    }

                    try
                    {
                        double x = double.Parse(e.Evaluate().ToString());
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

                Console.WriteLine("Total energy = " + totalEnergy); 
                
                avgEnergy += energy;
                avgTotalEnergy += totalEnergy;
                avgAsyncEnergy += asyncTotal;
            }

            Console.WriteLine("average energy = " + (avgEnergy / numOfTest));
            Console.WriteLine("average total energy = " + (avgTotalEnergy / numOfTest));

            tw.WriteLine("");
            tw.WriteLine("Average energy " + avgEnergy / numOfTest + " joules");
            tw.WriteLine("average async energy = " + (avgAsyncEnergy / numOfTest) + " joules");
            tw.WriteLine("average total energy = " + (avgTotalEnergy / numOfTest) + " joules");
            
            tw.Close();

            //Console.ReadKey();
        }
    }
}
