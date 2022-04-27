using InfoGathererConsole.DataAcquisition;
using InfoGathererConsole.DataAcquisition.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace InfoGathererConsole.Control
{
    internal class InfoGatheringControl
    {
        private string LogPath= @"\Log\InfoLog.txt";

        public void LogInformation()
        {
            string filePath = Directory.GetCurrentDirectory() + LogPath;

            Console.WriteLine(filePath);

            //Creates all missing directories to the filePath. If they already exist, nothing happens.
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            NetworkLookup lookup = new NetworkLookup();

            List<string> content = new List<string>();

            // Wikipedia addresses

            Console.WriteLine("Wikipedia");
            content.Add("Getting Wikipedia DNS info.");

            IPAddress[] array = Dns.GetHostAddresses("en.wikipedia.org");
            foreach (IPAddress ip in array)
            {
                content.Add(ip.ToString());
            }

            content.Add("");
            content.Add("");

            //
            // PING
            //

            Console.WriteLine("Ping");

            content.Add("Local ping information:");
            PingInformation ping = lookup.GetLocalPingInformation();
            content.Add("Address: " + ping.Address);
            content.Add("Round Trip Time: " + ping.RoundTripTime.ToString());
            content.Add("TTL: " + ping.TimeToLive.ToString());
            content.Add($"Dont Fragment: " + (ping.DontFragment ? "TRUE" : "FALSE"));
            content.Add("Buffer lenght: " + ping.BufferLenght.ToString());

            //
            // HOSTNAME
            // 

            Console.WriteLine("Hostname");

            string hostName = lookup.GetHostnameFromIp("8.8.8.8");
            content.Add("Host Name for ip: 8.8.8.8 : " + hostName);

            // 
            // TRACEROUTE
            //

            Console.WriteLine("Traceroute");

            content.Add("Traceroute for IP: 8.8.8.8: ");
            string route = lookup.Traceroute("8.8.8.8");

            //
            // DHCP Server Address
            //

            Console.WriteLine("DHCP");

            List<string> dhcpList = lookup.GetDhcpServerAddresses();
            foreach (string dhcp in dhcpList)
            {
                content.Add(dhcp);
            }

            //
            // LOCAL MACHINE
            //

            Console.WriteLine("Local Machine");

            content.Add("Getting information about local machine...");


            string machineName = Environment.MachineName;
            IPHostEntry hostInfo = Dns.GetHostEntry(machineName);

            // Get the IP address list that resolves to the host names contained in the 
            // Alias property.
            IPAddress[] address = hostInfo.AddressList;

            // Get the alias names of the addresses in the IP address list.
            String[] alias = hostInfo.Aliases;

            content.Add("Host name : " + hostInfo.HostName);
            content.Add("\nAliases : ");
            for (int index = 0; index < alias.Length; index++)
            {
                content.Add(alias[index]);
            }

            content.Add("");
            content.Add("\nIP address list : ");

            for (int index = 0; index < address.Length; index++)
            {
                content.Add(address[index].ToString());
            }


            //Uses StreamWriter to write data to file. 
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (string line in content)
                {
                    writer.WriteLine(line);
                }

                writer.Flush();
                writer.Close();
            }
        }
    }
}
