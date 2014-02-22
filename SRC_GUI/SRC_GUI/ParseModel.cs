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
        private string energyFile;

        private int numOfTest;

        private ArrayList al;
        private string[] sArray;
        private static char[] delimiter = { ' ', '\t' };
        private static char[] operators = { '+', '-', '*', '/', '%' };

        public ParseModel(string WorkingDir)
        {
            InitializeComponent();
            rootPath = WorkingDir;
            modelFile = WorkingDir + @"\model.txt";
            energyFile = WorkingDir + @"\energy.txt";
            textWorkingDir.Text = WorkingDir;
        }

        public ParseModel(string WorkingDir, int testCases)
        {
            InitializeComponent();
            rootPath = WorkingDir;
            modelFile = WorkingDir + @"\model.txt";
            energyFile = WorkingDir + @"\energy.txt";
            textWorkingDir.Text = WorkingDir;
            StartProcessing(testCases);
        }

        private void readModel()
        {
            string input;
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
            sArray = input.Split(operators);
        }

        private void calcSysEnergy(string path, ref double totalEnergy)
        {
            string[] sampleValues;
            string[] sampleFile = File.ReadAllLines(path);

            for (int k = 1; k < sampleFile.Length; k++)
            {
                sampleValues = sampleFile[k].Split(new char[2] { ' ', '\t' });
                totalEnergy += (double.Parse(sampleValues[sampleValues.Length - 1]) / 1000.0);
            }
        }

        private void calcAsyncEnergy(string path, ref double asyncTotal)
        {
            string[] lines;
            string[] asyncFile = File.ReadAllLines(path);

            for (int aNum = 1; aNum < asyncFile.Length; aNum++)
            {
                lines = asyncFile[aNum].Split(delimiter);
                asyncTotal += (double.Parse(lines[1]) / 1000);
            }
        }

        private void calcApplicationEnergy(string path, Expression e, ref double appEnergy)
        {
            string[] lineValue;
            string[] currFile = File.ReadAllLines(path);
            string[] varNames = currFile[0].Split(delimiter);

            for (int i = 1; i < currFile.Length; i++)
            {
                lineValue = currFile[i].Split(delimiter);

                for (int j = 0; j < varNames.Length; j++)
                {
                    double value = Double.Parse(lineValue[j].ToString());
                    if (value < 0) value = 0;

                    e.Parameters[varNames[j]] = value;
                }

                if (e.HasErrors()) updateStatus("e has error " + e.Error);

                try
                {
                    double x = double.Parse(e.Evaluate().ToString());
                    appEnergy += float.Parse(e.Evaluate().ToString()) / 1000;
                }
                catch (EvaluationException ee)
                {
                    updateStatus("Error catched: " + ee.Message);
                }

            }
        }

        private void dataOutput(string equation)
        {
            using (TextWriter outputFile = new StreamWriter(energyFile))
            {
                double sumAppEnergy = 0;
                double sumTotalEnergy = 0;
                double sumAsyncEnergy = 0;

                for (int curSample = 1; curSample <= numOfTest; curSample++)
                {
                    //Calculate System Energy (joule)
                    double totalEnergy = 0;
                    calcSysEnergy(rootPath + @"\" + curSample + @"\sample.txt", ref totalEnergy);

                    //Calculate asynchronous energy (joule)
                    double asyncTotal = 0;
                    if (curSample == 1) calcAsyncEnergy(rootPath + @"\" + curSample + @"\asyncTable.txt", ref asyncTotal);

                    // Calculate Application Energy
                    double appEnergy = 0;
                    Expression e = new Expression(equation);
                    calcApplicationEnergy(rootPath + @"\" + curSample + @"\test.txt", e, ref appEnergy);

                    updateStatus("");
                    updateStatus("Test Case " + curSample);
                    updateStatus("Application Energy " + " = " + appEnergy);
                    outputFile.WriteLine("");
                    outputFile.WriteLine("Test Case " + curSample);
                    outputFile.WriteLine("Application Energy " + " = " + appEnergy);

                    updateStatus("Total Energy = " + totalEnergy);

                    sumAppEnergy += appEnergy;
                    sumTotalEnergy += totalEnergy;
                    sumAsyncEnergy += asyncTotal;
                }

                updateStatus("");
                updateStatus("Average Application Energy = " + sumAppEnergy / numOfTest + " joules");
                updateStatus("Average Async Energy = " + (sumAsyncEnergy / numOfTest) + " joules");
                updateStatus("Average Total Energy = " + (sumTotalEnergy / numOfTest) + " joules");

                outputFile.WriteLine("");
                outputFile.WriteLine("Average Application Energy = " + sumAppEnergy / numOfTest + " joules");
                outputFile.WriteLine("Average Async Energy = " + (sumAsyncEnergy / numOfTest) + " joules");
                outputFile.WriteLine("Average Total Energy = " + (sumTotalEnergy / numOfTest) + " joules");

                textAppPower.Text = string.Format("{0:.####} joules", sumAppEnergy / numOfTest);
                textAsyncPower.Text = string.Format("{0:.####} joules", sumAsyncEnergy / numOfTest);
                textTotalPower.Text = string.Format("{0:.####} joules", sumTotalEnergy / numOfTest);
            }
        }

        public bool StartProcessing(int testCases = 5)
        {
            updateStatus("Process begining...");
            //float offset = float.Parse(sArray[0]);
            numOfTest = testCases;
            al = new ArrayList();

            readModel();

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

            dataOutput(equation);
            updateStatus("Process finished.");

            return true;
        }

        private void updateStatus(string status)
        {
            this.listBoxStatus.Items.Add(status);
            this.listBoxStatus.SelectedIndex = listBoxStatus.Items.Count - 1;
            this.listBoxStatus.SelectedIndex = -1;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                StartProcessing(Convert.ToInt32(numCount.Value));
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
        }
    }
}
