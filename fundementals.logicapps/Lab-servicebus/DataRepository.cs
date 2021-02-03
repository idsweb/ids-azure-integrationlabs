using System;
using System.Collections.Generic;
using Lab_servicebus.Models;

namespace Lab_servicebus
{
    public class DataRepository
    {
        public List<Name> Names;

        public List<string> Makes;

        public string[] Colours;

        public string[] CarTypes;

        public string[] Damages;

        public string[] RegNumbers;

        private Random rand;

        public string[] Emails;

        public string[] PhoneNumbers;

        private string letters = "A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,R,S,T,U,V,W,X,Y";


        public DataRepository()
        {
            Names = new List<Name>();
            Makes = new List<string>();
            rand = new Random();
        }

        public void Initialize()
        {
            InitializeNames();
            InitializeMakes();
            InitializeColours();
            InitializeDamages();
            InitializeRegNumbers();
            InitializeEmails();
            InitializePhoneNumbers();
        }

        private void InitializePhoneNumbers()
        {
            PhoneNumbers = new string[1];
            PhoneNumbers[0] = "077777777777";
        }

        private void InitializeEmails()
        {
            Emails = new string[1];
            Emails[0] = "someguy@googlemail.com";
        }

        private void InitializeRegNumbers()
        {
            RegNumbers = new string[] { CreateRegNumber() };
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
            var n = new Name() { LastName = "Disney", FirstName = "Walt" };
            Names.Add(n);
        }
        private void InitializeMakes()
        {
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
            string[] Colours = {
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
            string[] CarTypes = {
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
            string[] Damages = {
            "burnt out",
            "none",
            "crashed into",
            "scraped",
            "vandalized"
            };
        }
    }
}

