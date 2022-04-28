using InfoGathererConsole.Control;
using InfoGathererConsole.DataAcquisition;
using InfoGathererConsole.DataAcquisition.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace InfoGathererConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tests i = new Tests();
            //i.GathererMain();
            GathererMain();
        }

        static void GathererMain()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Select which information you would like displayed: ");
                Console.WriteLine("1 - Local Ping");
                Console.WriteLine("2 - Hostname");
                Console.WriteLine("3 - Traceroute");
                Console.WriteLine("4 - DHCP");
                Console.WriteLine("5 - Local Machine");

                int choice = GetUserInputInteger(false, true);

                InfoGatheringControl lookup = new InfoGatheringControl();

                switch (choice)
                {
                    case 1:
                        PingInformation info = lookup.GetLocalPingInformation();
                        Console.WriteLine("Address: {0}", info.Address.ToString());
                        Console.WriteLine("RoundTrip time: {0}", info.RoundTripTime);
                        Console.WriteLine("Time to live: {0}", info.TimeToLive);
                        Console.WriteLine("Don't fragment: {0}", info.DontFragment);
                        Console.WriteLine("Buffer size: {0}", info.BufferLenght);
                        break;

                    case 2:
                        Console.WriteLine("Enter an IP Address to look up...");
                        Console.WriteLine();
                        string input = Console.ReadLine();

                        string result = lookup.GetHostnameFromIp(input);
                        Console.WriteLine($"Result: {result}");
                        break;

                    case 3:
                        Console.WriteLine("Enter an IP Address or hostname to trace...");
                        Console.WriteLine();
                        string traceInput = Console.ReadLine();

                        string traceResult = lookup.TraceRoute(traceInput);
                        Console.WriteLine($"Result: {traceResult}");

                        break;

                    case 4:
                        Console.WriteLine();
                        List<string> dhcps = lookup.GetDhcpServerAddresses();

                        foreach(string dhcp in dhcps)
                        {
                            Console.WriteLine(dhcp);
                        }
                        break;

                    case 5:
                        Console.WriteLine();
                        List<string> localInfo = lookup.GetLocalMachineInfo();

                        foreach (string line in localInfo)
                        {
                            Console.WriteLine(line);
                        }

                        break;
                }

                Console.WriteLine("");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

            }

           

        }

        /// <summary>
        /// Requests the user to enter an integer with the corresponding request string, and
        /// makes sure the input is sanitized
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        static int GetUserInputInteger(bool hideCursor = false, bool printError = false, string phrase = "")
        {
            string userInput = "";
            bool mainLoopRunning = true;

            while (mainLoopRunning)
            {
                if (phrase != "")
                {
                    Console.WriteLine(phrase);
                }

                // If we're hiding the cursor, use another method of collecting the input
                if (hideCursor)
                {
                    bool loopRunning = true;

                    while (loopRunning)
                    {
                        var key = Console.ReadKey(true);

                        if (key.Key == ConsoleKey.Enter)
                        {
                            loopRunning = false;
                        }

                        userInput += key.KeyChar;
                    }
                }
                else
                {
                    userInput = Console.ReadLine();
                }


                // Empty input (only pressed enter for example)
                if (userInput.Length <= 0)
                {
                    if (printError) Console.WriteLine("Invalid input");
                    continue;
                }

                // Check that it only contains numbers
                if (!IsInputOnlyDigits(userInput))
                {
                    if (printError) Console.WriteLine("Invalid input: must only contain numbers");
                    continue;
                }
                else
                {
                    mainLoopRunning = false;
                }
            }

            return int.Parse(userInput);
        }

        /// <summary>
        /// Returns true if the string only contains digits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static bool IsInputOnlyDigits(string input)
        {
            foreach (char c in input)
            {
                // check that it's a number (unicode)
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }






    }
}
