using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCalc;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.ComponentModel; // Needed by BackgroundWorker

namespace ParseModelProject
{
    partial class ParseModel : Form
    {
        private string rootPath;
        private string modelFile;
        private int numOfTest;
        private string[] testFile;
        private string input;

        private ArrayList al;
        private string[] sArray;

        delegate void SetItemCallback(string text);

        public ParseModel(string WorkingDir)
        {
            InitializeComponent();
            rootPath = WorkingDir + @"\";
            modelFile = WorkingDir + @"\model.txt";
        }

        private void parseModel()
        {
            string[] model;
            string[] formula;

            model = File.ReadAllLines(modelFile);
            formula = model[0].Split('=');
            input = formula[1].Replace("sin", "Sin")
                .Replace("cos", "Cos");

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '+' || input[i] == '-' || input[i] == '*' || input[i] == '/' || input[i] == '%')
                {
                    al.Add(input[i]);
                }
            }

            sArray = input.Split(new char[5] { '+', '-', '*', '/', '%' });
        }

        private double calcSysEnergy(string[] sampleFile)
        {
            double totalEnergy = 0.0;
            string[] sampleValues;

            for (int k = 1; k < sampleFile.Length; k++)
            {
                sampleValues = sampleFile[k].Split(new char[2] { ' ', '\t' });
                totalEnergy += (double.Parse(sampleValues[sampleValues.Length - 1]) / 1000.0);
            }

            return totalEnergy;
        }

        private double calcAsyncEnergy(string[] asyncFile)
        {
            double asyncTotal = 0.0;
            string[] lines;

            for (int aNum = 1; aNum < asyncFile.Length; aNum++)
            {
                lines = asyncFile[aNum].Split(new char[2] { ' ', '\t' });
                asyncTotal += (double.Parse(lines[1]) / 1000);
            }

            return asyncTotal;
        }

        private float calcApplicationEnergy(string[] currFile, string[] varNames, Expression e)
        {
            float energy = 0.0f;
			string[] lineValue;
			
            for (int i = 1; i < currFile.Length; i++)
            {
                lineValue = currFile[i].Split(new char[2] { ' ', '\t' });

                for (int j = 0; j < varNames.Length; j++)
                {
                    double value = Double.Parse(lineValue[j].ToString());
                    if (value < 0) value = 0;

                    e.Parameters[varNames[j]] = value;
                }

                if (e.HasErrors())
                {
                    updateStatus("e has error " + e.Error);
                }

                try
                {
                    double x = double.Parse(e.Evaluate().ToString());
                    // Console.WriteLine(e.Evaluate());
                    energy += float.Parse(e.Evaluate().ToString()) / 1000;
                }
                catch (EvaluationException ee)
                {
                    updateStatus("Error catched: " + ee.Message);
                }

            }

            return energy;
        }

        public bool StartProcessing(int testCases = 5)
        {
			updateStatus("Process begining...");
            //float offset = float.Parse(sArray[0]);
            numOfTest = testCases;
            testFile = new string[numOfTest];
            al = new ArrayList();
			
            parseModel();

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

            TextWriter outputFile = new StreamWriter(rootPath + "energy.txt");

            for (int curSample = 1; curSample <= numOfTest; curSample++)
            {
				if (!Directory.Exists(rootPath + curSample))
				{
					updateStatus("Unexpected directory : " + rootPath + curSample);
					outputFile.Close();
					return false;
				}
				
                string[] sampleFile = File.ReadAllLines(rootPath + curSample + @"\sample.txt");
                string[] samVars = sampleFile[0].Split(new char[2] { ' ', '\t' });

                string[] asyncFile;

                string[] currFile = File.ReadAllLines(rootPath + curSample + @"\test.txt");
                string[] varNames = currFile[0].Split(new char[2] { ' ', '\t' });

                Expression e = new Expression(equation);
                float energy = 0;
                double totalEnergy = 0;
                double asyncTotal = 0;

                //Calculate System Energy (joule)
                totalEnergy = calcSysEnergy(sampleFile);

                //Calculate asynchronous energy (joule)
                if (curSample == 1)
                {
                    asyncFile = File.ReadAllLines(rootPath + curSample + @"\asyncTable.txt");
                    asyncTotal = calcAsyncEnergy(asyncFile);
                }

                // Calculate Application Energy
                energy = calcApplicationEnergy(currFile, varNames, e);

                updateStatus("");
                outputFile.WriteLine("");
                updateStatus("Test Case " + curSample);
                outputFile.WriteLine("Test Case " + curSample);
                updateStatus("Application Energy " + " = " + energy);
                outputFile.WriteLine("Application Energy " + " = " + energy);

                updateStatus("Total Energy = " + totalEnergy);

                avgEnergy += energy;
                avgTotalEnergy += totalEnergy;
                avgAsyncEnergy += asyncTotal;
            }

            updateStatus("");
            updateStatus("Average Application Energy " + avgEnergy / numOfTest + " joules");
            updateStatus("Average Async Energy = " + (avgAsyncEnergy / numOfTest) + " joules");
            updateStatus("Average Total Energy = " + (avgTotalEnergy / numOfTest) + " joules");

            outputFile.WriteLine("");
            outputFile.WriteLine("Average Application Energy " + avgEnergy / numOfTest + " joules");
            outputFile.WriteLine("Average Async Energy = " + (avgAsyncEnergy / numOfTest) + " joules");
            outputFile.WriteLine("Average Total Energy = " + (avgTotalEnergy / numOfTest) + " joules");

            outputFile.Close();
			updateStatus("Process finished.");

            return true;
        }

        private void updateStatus(string status)
        {
            if (this.listBoxStatus.InvokeRequired)
            {
                SetItemCallback d = new SetItemCallback(updateStatus);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                this.listBoxStatus.Items.Add(status);
                this.listBoxStatus.SelectedIndex = listBoxStatus.Items.Count - 1;
                this.listBoxStatus.SelectedIndex = -1;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            StartProcessing(Convert.ToInt32(numCount.Value));
        }
    }
}
