using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eureqa;
using System.Threading;
using System.IO;
using System.Collections;

namespace SymbolicRegression
{
    class Eureqa
    {
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

        public Eureqa() { }

        public void Run(string srcFile, string srcModel, string srcIP)
        {
            using (DataSet data = new DataSet())
            {
                string sourcePath = srcFile;

                //clean data
                string[] datas = File.ReadAllLines(sourcePath);
                string headLine = datas[0];
                string newHeadlLine = headLine.Replace('"', ' ');
                TextWriter tw = new StreamWriter(sourcePath);
                tw.WriteLine(newHeadlLine);

                for (int i = 1; i < datas.Length; i++)
                {
                    tw.WriteLine(datas[i]);
                }
                tw.Close();

                if (!data.import_ascii(sourcePath))
                {
                    throw new ArgumentException("Unable to import this file");
                }

                Console.WriteLine("Data imported successfully");
                Console.WriteLine(data.summary());

                string model =  srcModel;
                using (SearchOptions options = new SearchOptions(model))
                {

                    Console.WriteLine("> Setting the search options");
                    for (int i = 0; i < options.building_blocks_.Count; i++)
                    {
                        Console.WriteLine(options.building_blocks_[i]);
                    }
                    using (Connection conn = new Connection())
                    {
                        try
                        {
                            //140.113.88.194
                            string ipServer = srcIP;
                            Console.WriteLine("> Connecting to a eureqa server at " + ipServer);
                            if (!conn.connect(ipServer))
                            {
                                Console.WriteLine("Unable to connect to server");
                                Console.WriteLine(@"Try running the eureqa_server binary provided with the Eureqa API (""server"" sub-directory) first.");
                                throw new ApplicationException("Cannot connect to the local server.");
                            }
                            else HandleLastResult(conn, "Connection made", "Connected to server");
                            using (ServerInfo serv = new ServerInfo())
                            {
                                Console.WriteLine("> Querying the server systems information");
                                if (!conn.query_server_info(serv))
                                {
                                    Console.WriteLine("Unable to recieve the server information");
                                    throw new ApplicationException("No info");
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
                                throw new ArgumentException("Cannot send data set");
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
                            Dictionary<float, float> fitSize = new Dictionary<float, float>();
                            ArrayList models = new ArrayList();

                            using (SearchProgress progress = new SearchProgress())
                            {
                                using (SolutionFrontier bestSolutions = new SolutionFrontier())
                                {
                                    int c = 0;       
                                    while (conn.query_progress(progress))
                                    {
                                        Console.WriteLine("> " + progress.summary());
                                        using (var solution = progress.solution_)
                                        {
                                            if (bestSolutions.add(solution))
                                            {
                                                Console.Write("New solution found:");
                                                Console.WriteLine(solution.text_);
                                            }

                                            Console.WriteLine(" >> fitness >> " + solution.fitness_ + " >> size " + solution.complexity_ + " equation " + solution.text_);
                                            fitSize[solution.fitness_] = solution.complexity_;
                                            models.Add(solution.text_);
                                        }
                                        //Console.WriteLine();
                                        //Console.WriteLine(bestSolutions.to_string());
                                        //Console.WriteLine();
                                        //Thread.Sleep(new TimeSpan(0, 0, 1));
                                        ++c;
                                        if (c > 50000) break;
                                       
                                    }
                                }
                            }
                            Console.ReadKey();
                        }
                        finally
                        {
                            conn.disconnect();
                        }
                    }
                }
            }
        }
    }
}
