using InfoGathererConsole.Control;
using System;
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
            GathererMain();
        }

        static void GathererMain()
        {
            while(true)
            {
                Console.WriteLine("Select which information you would like displayed: ");
                Console.WriteLine("1 - Local Ping");
                Console.WriteLine("2 - Hostname");
                Console.WriteLine("3 - Traceroute");
                Console.WriteLine("4 - DHCP");
                Console.WriteLine("5 - Local Machine");


            }

            Console.ReadKey();

        }

        

        

       
    }
}
