using System;
using Lab_servicebus.Models;

namespace Lab_servicebus.Models
{
        public class AbandonedVehicleRequest
        {
            public string JobId { get; set; }
            //public VehicleData Vehicle { get; set; }

            public DateTime incidentDateTime { get; set; }

            public Image[] Images { get; set; }

            public string FullDescrtiption { get; set; }

            public Location Location { get; set; }
            public PersonalDetails PersonalDetails { get; set; }
            public bool ReceiveEmailSummary { get; set; }

            public VehicleData Vehicle { get; set; }

        }
}
