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
    public class NationalParksCLI
    {
        private const string Command_ViewListOfParks = "1";
        private const string Command_ViewListOfCampgrounds = "2";
        private const string Command_ViewListOfSites = "3";
        private const string Command_MakeAReservation = "4";
        private const string Command_Quit = "q";

        private string connectionString = "";

        private int selectedParkId = 0;
        private int selectedCampgroundId = 0;
        private int selectedSiteId = 0;
        private DateTime desiredStartDate = new DateTime();
        private DateTime desiredEndDate = new DateTime();


        public void RunCLI(string DatabaseConnectionString)
        {
            connectionString = DatabaseConnectionString;
            
            PrintMainMenu();

            while (true)
            {
                string command = Console.ReadLine().ToLower();
                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_ViewListOfParks:
                        PrintParkList();
                        SelectAPark();
                        Console.Clear();
                        PrintParkInfo();
                        PromptAfterParkInfo();
                        break;

                    case Command_ViewListOfCampgrounds:
                        PrintParkList();
                        SelectAPark();
                        Console.Clear();
                        PrintCampgroundList();
                        PromptAfterCampgroundList();
                        break;

                    case Command_ViewListOfSites:
                        PrintParkList();
                        SelectAPark();
                        Console.Clear();
                        PrintCampgroundList();
                        PromptForSiteSearchCriteria();
                        Console.Clear();
                        PrintAvailableSitesList();
                        PromptAfterSitesList();
                        break;

                    case Command_MakeAReservation:
                        PrintParkList();
                        SelectAPark();
                        Console.Clear();
                        PrintCampgroundList();
                        PromptForSiteSearchCriteria();
                        Console.Clear();
                        PrintAvailableSitesList();
                        CompleteReservation();
                        break;

                    case Command_Quit:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Thank you for using our National Parks Information and Reservation System!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your selection was not valid, please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }

                selectedParkId = 0;
                selectedCampgroundId = 0;
                desiredStartDate = new DateTime();
                desiredEndDate = new DateTime();

                PrintMainMenu();
            }
        }


        private void PrintMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   NATIONAL PARKS MAIN MENU\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   1 - View park information");
            Console.WriteLine("   2 - View a list of campgrounds");
            Console.WriteLine("   3 - View a list of available campsites");
            Console.WriteLine("   4 - Make a reservation");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Q - Quit\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("   Please make a selection to continue: ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        private void PrintParkList()
        {
            ParkSqlDAL parkDal = new ParkSqlDAL(connectionString);
            List<Park> parksAlphabetical = parkDal.GetAlphabeticalListOfAllParks();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   LIST OF NATIONAL PARKS\n");
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < parksAlphabetical.Count; i++)
            {
                Console.WriteLine("   " + parksAlphabetical[i].Park_Id + " - " + parksAlphabetical[i].Name);
            }
            Console.WriteLine();
        }
        
        private void SelectAPark()
        {
            selectedParkId= CLIHelper.GetParkInteger("   Please select a park ID to continue: ", connectionString);
            Console.WriteLine();
        }
        
        private void PrintParkInfo()
        {
            ParkSqlDAL parkDal = new ParkSqlDAL(connectionString);
            List<Park> parksById = parkDal.GetAllParksById();
            const int columnWidth = 21;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   " + parksById[selectedParkId - 1].Name + " Park Information:   \n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   Location:".PadRight(columnWidth) + parksById[selectedParkId - 1].Location);
            Console.WriteLine("   Established:".PadRight(columnWidth) + parksById[selectedParkId - 1].Establish_Date.Date.ToString("M/d/yyyy"));
            Console.WriteLine("   Area:".PadRight(columnWidth) + parksById[selectedParkId - 1].Area.ToString("###,###") + " sq km");
            Console.WriteLine("   Visitors:".PadRight(columnWidth) + parksById[selectedParkId - 1].Visitors.ToString("#,###,###") + " Annually\n");

            var words = parksById[selectedParkId - 1].Description.Split(' ');
            var lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
            {
                if (l.Last().Length + w.Length >= Console.WindowWidth - 1)
                    l.Add(w);
                else
                    l[l.Count - 1] += " " + w;
                return l;
            });
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        
        private void PromptAfterParkInfo()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Would you like to:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   1 - View information for a different park");
            Console.WriteLine("   2 - View a list of campgrounds");
            Console.WriteLine("   3 - Return to the Main Menu\n");
        
            int choice = CLIHelper.GetSubMenuInteger("   Selection: ");
            
            if (choice == 1)
            {
                Console.Clear();
                PrintParkList();
                SelectAPark();
                PrintParkInfo();
                PromptAfterParkInfo();
            }
            else if (choice == 2)
            {
                Console.Clear();
                PrintParkList();
                SelectAPark();
                PrintCampgroundList();
                PromptAfterCampgroundList();
            }
            else
            {
                Console.Clear();
            }
            Console.WriteLine();
        }
        
        private void PrintCampgroundList()
        {
            CampgroundSqlDAL campgroundDal = new CampgroundSqlDAL(connectionString);
            List<Campground> campgrounds = campgroundDal.GetListOfCampgrounds(selectedParkId);

            ParkSqlDAL parkDal = new ParkSqlDAL(connectionString);
            List<Park> parksById = parkDal.GetAllParksById();

            DateTimeFormatInfo monthName = new DateTimeFormatInfo();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"CAMPGROUNDS AT {parksById[selectedParkId - 1].Name} PARK\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   ID".PadRight(6) + "- " + "Name".PadRight(33) + "Open".PadRight(15) + "Close".PadRight(15) + "Daily Fee".PadLeft(10));
            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine("   " + campgrounds[i].Campground_Id.ToString().PadRight(3) + "- "
                    + campgrounds[i].Name.PadRight(33)
                    + monthName.GetMonthName((campgrounds[i].Open_From_MM)).ToString().PadRight(15)
                    + monthName.GetMonthName((campgrounds[i].Open_To_MM)).ToString().PadRight(15)
                    + ("$" + Math.Round(campgrounds[i].Daily_Fee, 2)).PadLeft(10));
            }
            Console.WriteLine();
        }
        
        private void PromptAfterCampgroundList() 
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Would you like to:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   1 - View a list of available campgrounds at a different park");
            Console.WriteLine("   2 - View a list of available campsites");
            Console.WriteLine("   3 - Return to the Main Menu\n");

            int choice = CLIHelper.GetSubMenuInteger("   Selection: ");

            if (choice == 1)
            {
                Console.Clear();
                PrintParkList();
                SelectAPark();
                Console.Clear();
                PrintCampgroundList();
                PromptAfterCampgroundList();
            }
            else if (choice == 2)
            {
                Console.Clear();
                PrintCampgroundList();
                PromptForSiteSearchCriteria();
                Console.Clear();
                PrintAvailableSitesList();
                PromptAfterSitesList();
            }
            else
            {
                Console.Clear();
            }
            Console.WriteLine();
        }
        
        private void PromptForSiteSearchCriteria()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            selectedCampgroundId = CLIHelper.GetCampgroundInteger("   Enter the Campground ID to search for available campsites: ", connectionString, selectedParkId);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            desiredStartDate = CLIHelper.GetReservationDateTime("   Enter the date you would like to arrive (MM/DD/YYYY): ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            desiredEndDate = CLIHelper.GetReservationDateTime("   Enter the date you would like to depart (MM/DD/YYYY): ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }


        private void PrintAvailableSitesList()
        {
            int desiredNumberOfDays = (desiredEndDate.Date - desiredStartDate.Date).Days + 1;

            SiteSqlDAL siteDal = new SiteSqlDAL(connectionString);
            List<Site> sites = siteDal.GetListOfTop5AvailableSitesAtCampground(selectedCampgroundId, desiredStartDate, desiredEndDate);

            if (sites.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("   There are NO campsites available on your desired dates. Would you like to:\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("   1 - Select a new park");
                Console.WriteLine("   2 - Enter new search criteria");
                Console.WriteLine("   3 - Return to the Main Menu\n");
                int noSitesChoice = CLIHelper.GetSubMenuInteger("   Selection: ");

                if (noSitesChoice == 1)
                {
                    Console.Clear();
                    PrintParkList();
                    SelectAPark();
                    Console.Clear();
                    PrintCampgroundList();
                    PromptForSiteSearchCriteria();
                    Console.Clear();
                    PrintAvailableSitesList();
                }
                else if (noSitesChoice == 2)
                {
                    Console.Clear();
                    PrintCampgroundList();
                    PromptForSiteSearchCriteria();
                    Console.Clear();
                    PrintAvailableSitesList();
                }
            }
            else
            {
                CampgroundSqlDAL campgroundDal = new CampgroundSqlDAL(connectionString);
                Campground selectedCampground = campgroundDal.GetSelectedCampground(selectedCampgroundId);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   Campsites available at {selectedCampground.Name}   \n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("   Site No.".PadRight(15)
                    + "Max Occup.".PadRight(14)
                    + "Accessible?".PadRight(15)
                    + "Max RV Length".PadRight(17)
                    + "Utility".PadRight(11)
                    + "Cost ".PadLeft(9));

                foreach (Site site in sites)
                {
                    string accessibleYesOrNo = "";
                    string maxRVLengthNumOrNA = "";
                    string utilitiesYesOrNA = "";

                    if (site.Accessible) { accessibleYesOrNo = "Yes"; } else { accessibleYesOrNo = "No"; }
                    if (site.Max_RV_Length == 0) { maxRVLengthNumOrNA = "N/A"; } else { maxRVLengthNumOrNA = site.Max_RV_Length.ToString(); }
                    if (site.Utilities) { utilitiesYesOrNA = "Yes"; } else { utilitiesYesOrNA = "N/A"; }

                    Console.WriteLine("      " + site.Site_Id.ToString().PadRight(9) 
                        + "    " + site.Max_Occupancy.ToString().PadRight(10)
                        + "    " + accessibleYesOrNo.PadRight(11)
                        + "    " + maxRVLengthNumOrNA.PadRight(13)
                        + "  " + utilitiesYesOrNA.PadRight(9)
                        + ("$" + Math.Round((desiredNumberOfDays * selectedCampground.Daily_Fee), 2)).PadLeft(9));
                }
            }
            Console.WriteLine();
        }

        private void PromptAfterSitesList() 
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Would you like to:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   1 - View a list of available campsites at a different campground");
            Console.WriteLine("   2 - Complete this reservation");
            Console.WriteLine("   3 - Return to the Main Menu\n");

            int choice = CLIHelper.GetSubMenuInteger("   Selection: ");

            if (choice == 1)
            {
                Console.Clear();
                PrintCampgroundList();
                PromptForSiteSearchCriteria();
                Console.Clear();
                PrintAvailableSitesList();
                PromptAfterSitesList();
            }
            else if (choice == 2)
            {
                Console.Clear();
                PrintAvailableSitesList();
                CompleteReservation();
            }
            else
            {
                Console.Clear();
            }
            Console.WriteLine();
        }


        private void CompleteReservation()
        {
            int selectedSiteId = CLIHelper.GetSiteInteger("   Which campsite would you like to reserve? ", connectionString, selectedCampgroundId, desiredStartDate, desiredEndDate);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n   What name should the reservation be saved under? ");
            Console.ForegroundColor = ConsoleColor.White;
            string reservationName = Console.ReadLine();
            Console.WriteLine();
            ReservationSqlDAL reservationDal = new ReservationSqlDAL(connectionString);
            reservationDal.MakeReservation(selectedSiteId, reservationName, desiredStartDate, desiredEndDate);
            Reservation thisReservation = reservationDal.GetSpecificReservation(selectedSiteId, reservationName);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"   Thank You! Your reservation has been made! Your confirmation number is: {thisReservation.Reservation_Id}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

