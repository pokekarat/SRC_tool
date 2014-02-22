using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eureqa;
using System.Threading;
using System.IO;
using System.Collections;


namespace EureqaTestProject
{
    class EureqaTest
    {
		public EureqaTest(string WorkingDir, string SampleNumber, string EureqaServerIP, int loops = 10000){
			_loops = loops;
            srcFile = WorkingDir + @"\" + SampleNumber + @"\modifyPower.txt";
			saveModelPath = WorkingDir + @"\model.txt";
            srcIP = EureqaServerIP;
            srcModel = "power = f0(cpu1)+f1(cpu2)+f2(cpu3)+f3(cpu4)+f4(cpu5)+f5(cpu6)+f6(cpu7)+f7(cpu8)+f8(freq1)+f9(freq2)+f10(freq3)+f11(freq4)+f12(freq5)+f13(freq6)+f14(freq7)+f15(freq8)+f16(bright)+f17(rx_pk)+f18(tx_pk)";
			StartProcessing();
		}
		
        private int _loops = 0;
        private string srcFile = @"";
        private string saveModelPath = @"";
        private string srcIP = @"";
        private string srcModel = @"";

        static void HandleLastResult(Connection conn, string message)
        {
            HandleLastResult(conn, message, message);
        }
        /// <summary>
        /// Handles the result, by examining and writing logs.
        /// </summary>
        /// <param name="conn">A <see cref="Connection"/> object.</param>
        /// <param name="message">The start of the message when it was failed by communication.</param>
        /// <param name="successMessage">The start of the message when there was no problems.</param>
        static void HandleLastResult(Connection conn, string message, string successMessage)
        {
            using (var result = conn.last_result())
            {
                if (result.value_ != 0)
                {
                    Console.WriteLine(message + " successfully, but the server sent back an error message:");
                    Console.WriteLine(result.message_);
                    throw new ApplicationException(result.message_);
                }
                else
                {
                    Console.WriteLine(message + " successfully, and the server sent back a success message:");
                    Console.WriteLine(result.message_);
                }
            }
        }

        public bool StartProcessing()
        {
            using (DataSet data = new DataSet())
            {
                //clean data
                string[] datas = File.ReadAllLines(srcFile);
                string headLine = datas[0];
                string newHeadlLine = headLine.Replace('"', ' ');
                TextWriter tw = new StreamWriter(srcFile);
                tw.WriteLine(newHeadlLine);

                for (int i = 1; i < datas.Length; i++)
                {
                    tw.WriteLine(datas[i]);
                }
                tw.Close();

                if (!data.import_ascii(srcFile))
                {
                    throw new ArgumentException("Unable to import this file");
                }

                Console.WriteLine("Data imported successfully");
                Console.WriteLine(data.summary());

                using (SearchOptions options = new SearchOptions(srcModel))
                {

                    Console.WriteLine("> Setting the search options");
                    //options.building_blocks_.Remove("a-b");
                    for (int i = 0; i < options.building_blocks_.Count; i++)
                    {
                        Console.WriteLine(options.building_blocks_[i]);
                    }

                    using(Connection conn = new Connection()){
						try
						{
							//140.113.88.194
							Console.WriteLine("> Connecting to a eureqa server at " + srcIP);
							if (!conn.connect(srcIP))
							{
								Console.WriteLine("Unable to connect to server at " + srcIP);
								Console.WriteLine(@"Try running the eureqa_server binary provided with the Eureqa API (""server"" sub-directory) first.");
								throw new ApplicationException("Unable to connect to server at " + srcIP);
							}
							else HandleLastResult(conn, "Connection made", "Connected to server");
							
							Console.WriteLine("> Querying the server systems information");
							using (ServerInfo serv = new ServerInfo())
							{
								if (!conn.query_server_info(serv))
								{
									Console.WriteLine("Unable to recieve the server information");
									throw new ApplicationException("Unable to recieve the server information");
								}
								else
								{
									Console.WriteLine("Recieved server information successfully:");
									Console.WriteLine(serv.summary());
								}
							}
							
							Console.WriteLine("> Sending the data set to the server");
							if (!conn.send_data_set(data))
							{
								Console.WriteLine("Unable to transfer the data set");
								throw new ArgumentException("Unable to transfer the data set");
							}
							else HandleLastResult(conn, "Data set transferred");

							Console.WriteLine("> Sending search options to the server");
							if (!conn.send_options(options))
							{
								Console.WriteLine("Unable to transfer the search options");
							}
							else HandleLastResult(conn, "Search options transferred");

							Console.WriteLine("> Telling server to start searching");
							if (!conn.start_search())
							{
								Console.WriteLine("Unable to send the start command");
								throw new ApplicationException("Unable to send the start command");
							}
							else HandleLastResult(conn, "Start command sent");

							Console.WriteLine("> Monitoring the search progress");
							Dictionary<float, Tuple<float, string>> fitSize = new Dictionary<float, Tuple<float, string>>();
							ArrayList models = new ArrayList();

							using (SearchProgress progress = new SearchProgress())
							{
								using (SolutionFrontier bestSolutions = new SolutionFrontier())
								{
                                    int c = 0;
                                    string output;

                                    Console.WriteLine("Debug 1");
									while (conn.query_progress(progress))
									{
                                        //Console.WriteLine("> " + progress.summary());
                                        Console.WriteLine("Debug 2");
										using (var solution = progress.solution_)
                                        {
                                            Console.WriteLine("Debug 3");
											if (bestSolutions.add(solution))
											{
												Console.Write("New solution found:");
												Console.WriteLine(solution.text_);
											}

                                            //output = " >> fitness >> " + solution.fitness_ + " >> size >> " + solution.complexity_ + " >> equation >> " + solution.text_;
                                            // Console.WriteLine(output);

											fitSize[solution.fitness_] = new Tuple<float,string>(solution.complexity_,solution.text_);
											//models.Add(solution.text_);
											
											
										}
										//Console.WriteLine();
										//Console.WriteLine(bestSolutions.to_string());
										//Console.WriteLine();
										//Thread.Sleep(new TimeSpan(0, 0, 1));
										++c;
										if (c > _loops) break;

									}
								   
								}
							}
							var list = fitSize.Keys.ToList();
							list.Sort();

							using (TextWriter tw2 = new StreamWriter(saveModelPath))
							{
								for (int i = 0; i < 1; i++)
								{
									tw2.WriteLine(fitSize[list[i]].Item2);
									Console.WriteLine(fitSize[list[i]].Item2);
								}
								//tw2.Close();
							}
						}
						finally { conn.disconnect(); }
					}
                }
            }
			return true;
        }
    }
}