using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace InfoGathererConsole.Control
{
    public class Tests
    {
        public void GathererMain()
        {
            IPAddress[] array = Dns.GetHostAddresses("en.wikipedia.org");
            foreach (IPAddress ip in array)
            {
                Console.WriteLine(ip.ToString());
            }


            LocalPing();
            Console.WriteLine("start");
            string t = GetHostnameFromIp("8.8.8.8");
            Console.WriteLine(t);
            Console.WriteLine("slut");
            string adr = GetIpFromHostname(t);
            Console.WriteLine("Weee " + adr);



            string a = Traceroute("8.8.8.8");
            Console.WriteLine("route*** " + a);

            DisplayDhcpServerAddresses();

            Console.ReadKey();
            //WIN-M69SG2Q0732.test.local
            //ZBC-RG01203MKC
            //string hostName = "DESKTOP-BLSK9KA";
            string hostName = Environment.MachineName;
            IPHostEntry hostInfo = Dns.GetHostEntry(hostName);
            // Get the IP address list that resolves to the host names contained in the 
            // Alias property.
            IPAddress[] address = hostInfo.AddressList;
            // Get the alias names of the addresses in the IP address list.
            String[] alias = hostInfo.Aliases;

            Console.WriteLine("Host name : " + hostInfo.HostName);
            Console.WriteLine("\nAliases : ");
            for (int index = 0; index < alias.Length; index++)
            {
                Console.WriteLine(alias[index]);
            }
            Console.WriteLine("\nIP address list : ");
            for (int index = 0; index < address.Length; index++)
            {
                Console.WriteLine(address[index]);
            }
            Console.ReadKey();

        }

        public void LocalPing()
        {
            // Ping's the local machine.
            Ping pingSender = new Ping();
            IPAddress address = IPAddress.Loopback;
            PingReply reply = pingSender.Send(address);

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
            else
            {
                Console.WriteLine(reply.Status);
            }
        }


        public string GetHostnameFromIp(string Ip)
        {
            string hostname = "";
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostByAddress(Ip);
                hostname = ipHostEntry.HostName;
            }
            catch (FormatException)
            {
                hostname = "Please specify a valid IP address.";
                return hostname;
            }
            catch (SocketException)
            {
                hostname = "Unable to perform lookup - a socket error occured.";
                return hostname;
            }
            catch (SecurityException)
            {
                hostname = "Unable to perform lookup - permission denied.";
                return hostname;
            }
            catch (Exception)
            {
                hostname = "An unspecified error occured.";
                return hostname;
            }

            return hostname;
        }

        public string GetIpFromHostname(string Hostname)
        {
            string ip = "";
            try
            {
                IPHostEntry ipHostEntry = Dns.Resolve(Hostname);
                if (ipHostEntry.AddressList.Length > 0)
                {
                    //ip = ipHostEntry.AddressList[0].Address.ToString();
                    ip = ipHostEntry.AddressList[0].ToString();
                }
                else
                {
                    ip = "No information found.";
                }
            }
            catch (SocketException)
            {
                ip = "Unable to perform lookup - a socket error occured.";
                return ip;
            }
            catch (SecurityException)
            {
                ip = "Unable to perform lookup - permission denied.";
                return ip;
            }
            catch (Exception)
            {
                ip = "An unspecified error occured.";
                return ip;
            }

            return ip;
        }

        public string Traceroute(string ipAddressOrHostName)
        {
            IPAddress ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];
            StringBuilder traceResults = new StringBuilder();


            using (Ping pingSender = new Ping())
            {

                PingOptions pingOptions = new PingOptions();
                Stopwatch stopWatch = new Stopwatch();
                byte[] bytes = new byte[32];

                pingOptions.DontFragment = true;
                pingOptions.Ttl = 1;
                int maxHops = 30;

                traceResults.AppendLine(
                    string.Format(
                        "Tracing route to {0} over a maximum of {1} hops:",
                        ipAddress,
                        maxHops));

                traceResults.AppendLine();

                for (int i = 1; i < maxHops + 1; i++)
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                    PingReply pingReply = pingSender.Send(
                        ipAddress,
                        5000,
                        new byte[32], pingOptions);

                    stopWatch.Stop();

                    traceResults.AppendLine(
                        string.Format("{0}\t{1} ms\t{2}",
                        i,
                        stopWatch.ElapsedMilliseconds,
                        pingReply.Address));



                    if (pingReply.Status == IPStatus.Success)
                    {
                        traceResults.AppendLine();
                        traceResults.AppendLine("Trace complete."); break;
                    }

                    pingOptions.Ttl++;

                }
            }
            return traceResults.ToString();
        }

        public void DisplayDhcpServerAddresses()
        {
            Console.WriteLine("DHCP Servers");
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapteradapterProperties = adapter.GetIPProperties();
                IPAddressCollection addresses = adapteradapterProperties.DhcpServerAddresses;
                if (addresses.Count > 0)
                {
                    Console.WriteLine(adapter.Description);
                    foreach (IPAddress address in addresses)
                    {
                        Console.WriteLine("  Dhcp Address ............................ : {0}", address.ToString());
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
