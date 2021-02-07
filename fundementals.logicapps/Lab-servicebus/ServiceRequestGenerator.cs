using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Lab_servicebus.Models;
using Newtonsoft.Json;

namespace Lab_servicebus
{
    public class ServiceRequestGenerator
    {
        private int i = 0;

        private DataRepository repo;
        private Random rand;

        public ServiceRequestGenerator()
        {
            repo = new DataRepository();
            repo.Initialize();
            rand = new Random();
        }

        public T RandomItem<T>(T[] array)
        {
            int r = rand.Next(0, array.Length - 1);
            return array[r];
        }

        public async Task<AbandonedVehicleRequest> CreateRequest()
        {
            string id = Guid.NewGuid().ToString();

            var vehicle = new VehicleData()
            {
                color = RandomItem<String>(repo.Colours),
                damage = RandomItem<String>(repo.Damages),
                make = RandomItem<String>(repo.Makes.ToArray()),
                hasRoadTax = Convert.ToBoolean(rand.Next(0, 1)),
                vehicleType = RandomItem<String>(repo.CarTypes),
                vehicleReg = RandomItem<String>(repo.RegNumbers),
                safetyRisk = new SafetyRisk()
                {
                    IsRIsk = Convert.ToBoolean(rand.Next(0, 1)),
                    RiskText = ""
                }
            };

            var personalDetails = new PersonalDetails();
            var addr = RandomItem<AddressData>(repo.addressData);
            personalDetails.Address = new Address() { AddressText = addr.Address, PostCode = addr.Postcode };
            personalDetails.Contact = new Contact();
            personalDetails.Contact.Email = repo.Emails[i];
            personalDetails.Contact.PhoneNumber = repo.addressData[i].PhoneNumber;
            personalDetails.Contact.PreferredContact = "Email";

            var loc = GetLocation();
            Image[] _images = repo.GetImages(id, rand.Next(1, 3));
            await UploadImagesAsync(_images);

            // create a message that we can send
            var req = new AbandonedVehicleRequest()
            {
                JobId = id,
                FullDescrtiption = "",
                Location = loc,
                Images = _images,
                PersonalDetails = personalDetails,
                Vehicle = vehicle,
                ReceiveEmailSummary = Convert.ToBoolean(rand.Next(0, 1)),
                incidentDateTime = DateTime.Now.AddDays((0 - rand.Next(0, 28)))
            };
            i++;
            return req;
        }

        private Location GetLocation()
        {
            Location loc = new Location();
            loc.Coordinates = "53.8225424, -1.6089916";
            return loc;
        }

        private async Task UploadImagesAsync(Image[] images)
        {
            Secrets secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText(@"secrets.json"));
            string connectionString = secrets.BlobStorageConnectionString;

            //Create a unique name for the container
            string containerName = "abandonedvehicles";

            // Create the container and return a container client object
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            // use some sample images and rename them

            for (int i = 0; i < images.Length; i++)
            {
                int imageIndex = i+1;
                string fileName = String.Format("Images/abandonedvehicle{0}.jpg", imageIndex.ToString());
                string localFilePath = Path.Combine(fileName);

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(images[i].Name);

                Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

                // Open the file and upload its data
                using FileStream uploadFileStream = File.OpenRead(localFilePath);
                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();

            }

        }
    }
}
