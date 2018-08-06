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
    public class CampgroundSqlDAL
    {
        private string connectionString;
        private const string SQL_Campgrounds = @"SELECT * FROM campground WHERE park_id = @park_id ORDER BY campground_id;";
        private const string SQL_SelectedCampground = @"SELECT * FROM campground WHERE campground_id = @campground_id;";


        public CampgroundSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public List<Campground> GetListOfCampgrounds(int parkId)
        {
            List<Campground> result = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Campgrounds, conn);
                    cmd.Parameters.AddWithValue("@park_id", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground cg = new Campground();
                        cg.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        cg.Park_Id = Convert.ToInt32(reader["park_id"]);
                        cg.Name = Convert.ToString(reader["name"]);
                        cg.Open_From_MM = Convert.ToInt32(reader["open_from_mm"]);
                        cg.Open_To_MM = Convert.ToInt32(reader["open_to_mm"]);
                        cg.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);
                        
                        result.Add(cg);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error on CampgroundSqlDAL!!!!");
                Console.WriteLine(ex);
            }
            return result;
        }

        public Campground GetSelectedCampground(int campgroundId)
        {
            Campground result = new Campground();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectedCampground, conn);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        result.Park_Id = Convert.ToInt32(reader["park_id"]);
                        result.Name = Convert.ToString(reader["name"]);
                        result.Open_From_MM = Convert.ToInt32(reader["open_from_mm"]);
                        result.Open_To_MM = Convert.ToInt32(reader["open_to_mm"]);
                        result.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error on CampgroundSqlDAL!!!!");
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}
