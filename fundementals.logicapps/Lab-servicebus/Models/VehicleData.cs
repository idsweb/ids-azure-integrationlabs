using Lab_servicebus.Models;

namespace Lab_servicebus.Models
{
    public class VehicleData
    {
        public string vehicleType { get; set; }
        public bool hasRoadTax { get; set; }
        public string vehicleReg { get; set; }
        public string color { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string damage { get; set; }
        
        public SafetyRisk safetyRisk { get; set; }
    }
}
