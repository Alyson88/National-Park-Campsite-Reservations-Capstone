using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.UserInterface;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sample Code to get a connection string from the
            // App.Config file
            // Use this so that you don't need to copy your connection string all over your code!
            //string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
            string connectionString = @"Data Source =.\sqlexpress;Initial Catalog = NationalParks; Integrated Security = True";
            NationalParksCLI cli = new NationalParksCLI();
            cli.RunCLI(connectionString);
            
            // Main should only have a few lines of code
            
        }
    }
}
