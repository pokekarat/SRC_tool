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

namespace ShowResultsProject
{
    partial class ShowResults : Form
    {
        private string _workingDir;

        public ShowResults(string workingDir)
        {
            InitializeComponent();
            _workingDir = workingDir;

            updateData();
        }

        private void ReadDatas(string path, ref double appPower, ref double asyncPower, ref double totalPower)
        {
            string[] energy, model;
			model = File.ReadAllLines(path + @"\model.txt");
			energy = File.ReadAllLines(path + @"\energy.txt");
            appPower = Convert.ToDouble(energy[16].Split(' ')[4]);
            asyncPower = Convert.ToDouble(energy[17].Split(' ')[4]);
            totalPower = Convert.ToDouble(energy[18].Split(' ')[4]);
			
			updateStatus(path);
			updateStatus("==> " + model[0]);
        }

        private void updateData()
        {
            skype();
            line();
            game();
        }

        private void skype()
        {
            double appPower = 0.0;
            double asyncPower = 0.0;
            double totalPower = 0.0;
            string path;

            // Skype Idle
            path = _workingDir + @"\skype\idle";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppSkypeIdle.Text = appPower.ToString();
                textAsyncSkypeIdle.Text = asyncPower.ToString();
                textTotalSkypeIdle.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }

            // Skype Voice
            path = _workingDir + @"\skype\voice";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppSkypeVoice.Text = appPower.ToString();
                textAsyncSkypeVoice.Text = asyncPower.ToString();
                textTotalSkypeVoice.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }

            // Skype Video
            path = _workingDir + @"\skype\video";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppSkypeVideo.Text = appPower.ToString();
                textAsyncSkypeVideo.Text = asyncPower.ToString();
                textTotalSkypeVideo.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
        }

        private void line()
        {
            double appPower = 0.0;
            double asyncPower = 0.0;
            double totalPower = 0.0;
            string path;

            // Line Idle
            path = _workingDir + @"\line\idle";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppLineIdle.Text = appPower.ToString();
                textAsyncLineIdle.Text = asyncPower.ToString();
                textTotalLineIdle.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }

            // Line Voice
            path = _workingDir + @"\line\voice";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppLineVoice.Text = appPower.ToString();
                textAsyncLineVoice.Text = asyncPower.ToString();
                textTotalLineVoice.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }

            // Line Video
            path = _workingDir + @"\line\video";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppLineVideo.Text = appPower.ToString();
                textAsyncLineVideo.Text = asyncPower.ToString();
                textTotalLineVideo.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
        }

        private void game()
        {
            double appPower = 0.0;
            double asyncPower = 0.0;
            double totalPower = 0.0;
            string path;

            // Candy Crush
            path = _workingDir + @"\candycrush";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppCandyCrush.Text = appPower.ToString();
                textAsyncCandyCrush.Text = asyncPower.ToString();
                textTotalCandyCrush.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }

            // PokoPang
            path = _workingDir + @"\pokopang";
            try
            {
                ReadDatas(path, ref appPower, ref asyncPower, ref totalPower);
                textAppPokoPang.Text = appPower.ToString();
                textAsyncPokoPang.Text = asyncPower.ToString();
                textTotalPokoPang.Text = totalPower.ToString();
            }
            catch (IOException msg)
            {
                updateStatus(msg.Message);
            }
            catch (FormatException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
            catch (IndexOutOfRangeException msg)
            {
                updateStatus(msg.Message);
                updateStatus("Probabilily your \"energy.txt\" needs to be updated.");
                updateStatus("Probabilily your \"" + path + "\\energy.txt\" needs to be updated.");
            }
        }

        private void updateStatus(string status)
        {
            this.listBoxStatus.Items.Add(status);
            listBoxStatus.SelectedIndex = listBoxStatus.Items.Count - 1;
            listBoxStatus.SelectedIndex = -1;
        }
    }
}
