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
    public class ReservationSqlDAL
    {
        private string connectionString;
        private const string SQL_AllReservationsForCampground = @"SELECT r.site_id, r.from_date, r.to_date 
                                                                  FROM reservtion r 
                                                                  JOIN site s ON r.site_id = s.site_id 
                                                                  WHERE s.campground_id = @selected_campground_id 
                                                                  ORDER BY reservation_id;";
        private const string SQL_SpecificReservation = @"SELECT * FROM reservation WHERE site_id = @site_id AND name = @name;";


        public ReservationSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public List<Reservation> GetListOfAllReservationsForCampground(int selectedCampgroundId)
        {
            List<Reservation> result = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AllReservationsForCampground, conn);
                    cmd.Parameters.AddWithValue("@selected_campground_id", selectedCampgroundId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation r = new Reservation();
                        r.Reservation_Id = Convert.ToInt32(reader["reservation_id"]);
                        r.Site_Id = Convert.ToInt32(reader["site_id"]);
                        r.Name = Convert.ToString(reader["name"]);
                        r.From_Date = Convert.ToDateTime(reader["from_date"]);
                        r.To_Date = Convert.ToDateTime(reader["to_date"]);
                        r.Create_Date = Convert.ToDateTime(reader["create_date"]);

                        result.Add(r);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error on ReservationSqlDAL!!!!");
                Console.WriteLine(ex);
            }
            return result;
        }

        public void MakeReservation(int siteId, string reservationName, DateTime desiredStartDate, DateTime desiredEndDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO reservation VALUES (@siteId, @reservationName, @desiredStartDate, @desiredEndDate, @dateCreated)", conn);
                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@reservationName", reservationName);
                    cmd.Parameters.AddWithValue("@desiredStartDate", desiredStartDate.Date);
                    cmd.Parameters.AddWithValue("@desiredEndDate", desiredEndDate.Date);
                    cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error on ReservationSqlDAL!!!!");
                Console.WriteLine(ex);
            }
        }
        
        public Reservation GetSpecificReservation(int siteId, string reservationName)
        {
            Reservation result = new Reservation();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SpecificReservation, conn);
                    cmd.Parameters.AddWithValue("@site_id", siteId);
                    cmd.Parameters.AddWithValue("@name", reservationName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Reservation_Id = Convert.ToInt32(reader["reservation_id"]);
                        result.Site_Id = Convert.ToInt32(reader["site_id"]);
                        result.Name = Convert.ToString(reader["name"]);
                        result.From_Date = Convert.ToDateTime(reader["from_date"]);
                        result.To_Date = Convert.ToDateTime(reader["to_date"]);
                        result.Create_Date = Convert.ToDateTime(reader["create_date"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error on ReservationSqlDAL!!!!");
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}
