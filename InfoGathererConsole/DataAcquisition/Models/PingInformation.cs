using System;
using System.Collections.Generic;
using System.Text;

namespace InfoGathererConsole.DataAcquisition.Models
{
    public class PingInformation
    {
        public string Address { get; set; }
        public long RoundTripTime { get; set; }
        public int TimeToLive { get; set; }

        public bool DontFragment { get; set; }
        public int BufferLenght { get; set; }

    }
}
