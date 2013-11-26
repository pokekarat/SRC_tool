using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eureqa;
using System.Threading;

namespace BasicClient
{
    class Program
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

        static void Main(string[] args)
        {
            using (DataSet data = new DataSet())
            {
                string sourcePath = args[0];
                if (!data.import_ascii(sourcePath))
                {
                    throw new ArgumentException("Unable to import this file");
                }
                Console.WriteLine("Data imported successfully");
                Console.WriteLine(data.summary());
                using (SearchOptions options = new SearchOptions("power = f(util,freq,lcd,mem)"))
                {
                    
                    Console.WriteLine("> Setting the search options");
                    Console.WriteLine(options.summary());
                    using (Connection conn = new Connection())
                    {
                        try
                        {
                            //140.113.88.194
                            string ipServer = args[1];
                            Console.WriteLine("> Connecting to a eureqa server at "+ipServer);
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
                            using (SearchProgress progress = new SearchProgress())
                            {
                                using (SolutionFrontier bestSolutions = new SolutionFrontier())
                                {
                                    int count = 5;
                                    while (conn.query_progress(progress))
                                    {
                                        Console.WriteLine("> " + progress.summary());
                                        using (var solution = progress.solution_)
                                        {
                                            if (bestSolutions.add(solution))
                                            {
                                                Console.WriteLine("New solution found:");
                                                Console.WriteLine(solution);
                                            }
                                        }
                                        Console.WriteLine();
                                        Console.WriteLine(bestSolutions.to_string());
                                        Console.WriteLine();
                                        Thread.Sleep(new TimeSpan(0, 0, 1));
                                        --count;
                                        if (count < 0)
                                        {
                                            break;
                                        }
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