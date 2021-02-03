using System;
using Lab_servicebus.Models;

namespace Lab_servicebus
{
    public class ServiceRequestGenerator
    {
        private  int i = 0;

        public AbandonedVehicleRequest CreateRequest()
        {

            DataRepository repo = new DataRepository();
            Random rand = new Random();

            var vehicle = new VehicleData()
            {
                color = repo.Colours[i],
                damage = repo.Damages[i],
                make = repo.Makes[i],
                hasRoadTax = Convert.ToBoolean(rand.Next(0, 1)),
                vehicleType = repo.CarTypes[i],
                vehicleReg = repo.RegNumbers[i],
                safetyRisk = new SafetyRisk()
                {
                    IsRIsk = Convert.ToBoolean(rand.Next(0, 1)),
                    RiskText = ""
                }
            };

            var loc = new Location();
            loc.Coordinates = "53.8225424, -1.6089916";

            var personalDetails = new PersonalDetails();
            personalDetails.Address = new Address();
            personalDetails.Address.AddressText = "33 Some Street";
            personalDetails.Address.PostCode = "LS5 3RF";
            personalDetails.Contact = new Contact();
            personalDetails.Contact.Email = repo.Emails[i] ;
            personalDetails.Contact.PhoneNumber = repo.PhoneNumbers[i];

            int imageCounter = rand.Next(0, 2);

            Image[] images = new Image[imageCounter];

            for (int i = 0; i < imageCounter; i++)
            {
                images[i] = new Image() { Name = String.Format("abandonedvehicle{0}.jpg", imageCounter.ToString()) };
            }

            // create a message that we can send
            var req = new AbandonedVehicleRequest()
            {
                JobId = Guid.NewGuid().ToString(),
                FullDescrtiption = "",
                Location = loc,
                Images = images,
                PersonalDetails = personalDetails,
                Vehicle = vehicle,
                ReceiveEmailSummary = Convert.ToBoolean(rand.Next(0, 1)),
                incidentDateTime = DateTime.Now.AddDays((0 - rand.Next(0, 28)))
            };

            return req;


        }

    }
}
