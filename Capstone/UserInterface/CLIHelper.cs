using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;
using System.Globalization;

namespace Capstone.UserInterface
{
    public class CLIHelper
    {
        public static int GetParkInteger(string message, string connectionString)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            ParkSqlDAL parkDal = new ParkSqlDAL(connectionString);
            List<Park> parksAlphabetical = parkDal.GetAlphabeticalListOfAllParks();

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Invalid input format. Please try again\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.White;
                userInput = Console.ReadLine();
                numberOfAttempts++;
                Console.WriteLine();

            }
            while (!int.TryParse(userInput, out intValue) || intValue > parksAlphabetical.Count);

            return intValue;
        }

        public static int GetSubMenuInteger(string message)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Invalid input format. Please try again\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.White;
                userInput = Console.ReadLine();
                numberOfAttempts++;
                Console.WriteLine();

            }
            while (!int.TryParse(userInput, out intValue) || intValue > 3 || intValue < 1);
            
            return intValue;
        }

        public static int GetCampgroundInteger(string message, string connectionString, int selectedParkId)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            CampgroundSqlDAL campgroundDal = new CampgroundSqlDAL(connectionString);
            List<Campground> campgroundsForSelectedPark = campgroundDal.GetListOfCampgrounds(selectedParkId);

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Invalid input format. Please try again\n");
                    Console.ForegroundColor = ConsoleColor.White;

                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.White;

                userInput = Console.ReadLine();
                numberOfAttempts++;
                Console.WriteLine();

            }
            while (!int.TryParse(userInput, out intValue) || intValue > campgroundsForSelectedPark.Count);
            
            return intValue;
        }

        public static int GetSiteInteger(string message, string connectionString, int selectedCampgroundId, DateTime desiredStartDate, DateTime desiredEndDate)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            SiteSqlDAL siteDal = new SiteSqlDAL(connectionString);
            List<Site> sitesForSelectedCampground = siteDal.GetListOfTop5AvailableSitesAtCampground(selectedCampgroundId, desiredStartDate, desiredEndDate);
            List<int> availableSiteIds = new List<int>();

            foreach (Site site in sitesForSelectedCampground)
            {
                availableSiteIds.Add(site.Site_Id);
            }
            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Invalid input. Please try again\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.White;
                userInput = Console.ReadLine();
                numberOfAttempts++;
                Console.WriteLine();

            }
            while (!int.TryParse(userInput, out intValue) || (!availableSiteIds.Contains(intValue)));

            return intValue;
        }

        public static DateTime GetReservationDateTime(string message)
        {
            string userInput = String.Empty;
            DateTime dateValue = DateTime.MinValue;
            int numberOfAttempts = 0;
            
            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Invalid date format. Please try again");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(message + " ");
                Console.ForegroundColor = ConsoleColor.White;
                userInput = Console.ReadLine();
                numberOfAttempts++;
                Console.WriteLine();
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }

    }
}
