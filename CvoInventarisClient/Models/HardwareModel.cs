using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class HardwareModel
    {
        public int IdHardware { get; set; }
        public CpuModel Cpu { get; set; }
        public DeviceModel Device { get; set; }
        public GrafischeKaartModel GrafischeKaart { get; set; }
        public HarddiskModel Harddisk { get; set; }
    }
}