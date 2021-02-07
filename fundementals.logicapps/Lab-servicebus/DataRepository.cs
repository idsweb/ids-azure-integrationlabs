using System;
using System.Collections.Generic;
using Lab_servicebus.Models;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;
using CsvHelper;
using System.Linq;

namespace Lab_servicebus
{
    public class DataRepository
    {
        public List<Name> Names { get; set; }

        public List<string> Makes { get; set; }

        public string[] Colours { get; set; }

        public string[] CarTypes { get; set; }

        public string[] Damages { get; set; }

        public string[] RegNumbers { get; set; }

        private Random rand;

        public string[] Emails { get; set; }

        private string letters = "A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,R,S,T,U,V,W,X,Y";

        public AddressData[] addressData { get; set; }

        private Image[] _images { get; set; }

        public DataRepository()
        {
            Names = new List<Name>();
            Makes = new List<string>();
            rand = new Random();
        }

        public void InitializeAddressData()
        {
            using (var reader = new StreamReader(@"Data\addresses.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<AddressData>();
                addressData = records.ToArray();
            }
        }

        public void Initialize()
        {
            InitializeNames();
            InitializeMakes();
            InitializeCarTypes();
            InitializeColours();
            InitializeDamages();
            InitializeRegNumbers();
            InitializeEmails();
            InitializeAddressData();
        }

        public Image[] GetImages(string messageId, int imageCounter)
        {
            _images = new Image[imageCounter];

            for (int i = 0; i < imageCounter; i++)
            {
                _images[i] = new Image() { Name = String.Format(@"{0}/{1}", messageId, Guid.NewGuid().ToString()) };
            }

            return _images;
        }

        private void InitializeEmails()
        {
            this.Emails = new string[1];
            Emails[0] = "someguy@googlemail.com";
        }

        private void InitializeRegNumbers()
        {
            this.RegNumbers = new string[] { CreateRegNumber() };
        }

        private string CreateRegNumber()
        {
            var lettersArray = letters.Split(',');

            string[] reg = new string[6];
            reg[0] = "Y";
            reg[1] = lettersArray[rand.Next(1, 23) - 1];
            reg[2] = rand.Next(1, 29).ToString();
            reg[3] = lettersArray[rand.Next(1, 23) - 1];
            reg[4] = lettersArray[rand.Next(1, 23) - 1];
            reg[5] = lettersArray[rand.Next(1, 23) - 1];

            return String.Concat(reg);
        }

        private void InitializeNames()
        {
            this.Names = new List<Name>();
            var n = new Name() { LastName = "Disney", FirstName = "Walt" };
            Names.Add(n);
        }
        private void InitializeMakes()
        {
            this.Makes = new List<string>();
            Makes.Add("Land Rover");
            Makes.Add("Jaguar");
            Makes.Add("Mini");
            Makes.Add("Porche");
            Makes.Add("Volkswagen");
            Makes.Add("Audi");
            Makes.Add("Toyota");
            Makes.Add("Vaxhaul");
            Makes.Add("Peugeot");
            Makes.Add("Skoda");
            Makes.Add("Lexus");
        }

        private void InitializeColours()
        {
            this.Colours = new string[]{
            "red",
            "white",
            "yellow",
            "silver",
            "black",
            "green",
            "orange",
            "dark green",
            "unknown"
            };
        }

        private void InitializeCarTypes()
        {
            this.CarTypes = new string[]{
            "Sedan",
            "Hatchback",
            "Saloon",
            "Sports car",
            "Mini van",
            "Family car"
            };
        }

        private void InitializeDamages()
        {
            this.Damages = new string[]{
            "burnt out",
            "none",
            "crashed into",
            "scraped",
            "vandalized"
            };
        }
    }
}

