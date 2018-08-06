using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.UserInterface;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string connectionString;
        private decimal Daily_Fee;
        private const string SQL_Top5AvailableSitesAtCampground = @"SELECT DISTINCT TOP 5 s.site_id, campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities
                                                                    FROM site s 
                                                                    JOIN reservation r ON s.site_id = r.site_id
                                                                    WHERE s.campground_id = @campground_id AND (r.from_date > @end_date OR r.to_date < @start_date);";
        
        public SiteSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public List<Site> GetListOfTop5AvailableSitesAtCampground(int selectedCampgroundId, DateTime desiredStartDate, DateTime desiredEndDate)
        {
            List<Site> result = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Top5AvailableSitesAtCampground, conn);
                    cmd.Parameters.AddWithValue("@campground_id", selectedCampgroundId);
                    cmd.Parameters.AddWithValue("@start_date", desiredStartDate.Date);
                    cmd.Parameters.AddWithValue("@end_date", desiredEndDate.Date);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Site_Id = Convert.ToInt32(reader["site_id"]);
                        s.Campground_Id= Convert.ToInt32(reader["campground_id"]);
                        s.Site_Number = Convert.ToInt32(reader["site_number"]);
                        s.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Accessible = Convert.ToBoolean(reader["accessible"]);
                        s.Max_RV_Length = Convert.ToInt32(reader["max_rv_length"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]);
                        result.Add(s);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error on SiteSqlDAL!!!!");
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}