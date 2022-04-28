using InfoGathererConsole.DataAcquisition;
using InfoGathererConsole.DataAcquisition.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace InfoGathererConsole.Control
{
    public class InfoGatheringControl
    {
        private NetworkLookup lookup;

        public InfoGatheringControl()
        {
            lookup = new NetworkLookup();   
        }

        public PingInformation GetLocalPingInformation()
        {
             return lookup.GetLocalPingInformation();
        }

        public string GetHostnameFromIp(string ip)
        {
            return lookup.GetHostnameFromIp(ip);
        }

        public string TraceRoute(string addressOrHostname)
        {
            return lookup.Traceroute(addressOrHostname);
        }

        public List<string> GetDhcpServerAddresses()
        {
            return lookup.GetDhcpServerAddresses();
        }

        public List<string> GetLocalMachineInfo()
        {
            return lookup.GetLocalMachineInfo();
        }

    }
}
