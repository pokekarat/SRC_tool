using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCalc;
using System.Diagnostics;
using System.IO;
using System.Collections;

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

            for (int n = 0; n < numOfTest; n++)
            {
                
                string[] currFile = File.ReadAllLines(rootPath+(n+1)+@"\test.txt");
                string[] varNames = currFile[0].Split('\t');

                Expression e = new Expression(model[0]);
                
                for (int i = 1; i < currFile.Length; i++)
                {

                    string[] lineValue = currFile[i].Split('\t');

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
                    }
                    catch (EvaluationException ee)
                    {
                        Console.WriteLine("Error catched: " + ee.Message);
                    }

                }
            }
           
            Console.ReadKey();
        }
    }
}
