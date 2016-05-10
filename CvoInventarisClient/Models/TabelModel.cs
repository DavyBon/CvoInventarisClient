using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CvoInventarisClient.Models
{
    public class TabelModel
    {
        public List<CpuModel> cpus { get; set; }
        public List<VerzekeringModel> verzekeringen { get; set; }
        public List<DeviceModel> devices { get; set; }
        public List<FactuurModel> facturen { get; set; }
        public List<GrafischeKaartModel> grafischeKaarten { get; set; }
        public List<HarddiskModel> harddisks { get; set; }
        public List<HardwareModel> hardwares { get; set; }
        public List<InventarisModel> inventarissen { get; set; }
        public List<LeverancierModel> leveranciers { get; set; }
        public List<LokaalModel> lokalen { get; set; }
        public List<NetwerkModel> netwerken { get; set; }
        public List<ObjectModel> objecten { get; set; }
        public List<ObjectTypeModel> objectTypes { get; set; }
    }
}