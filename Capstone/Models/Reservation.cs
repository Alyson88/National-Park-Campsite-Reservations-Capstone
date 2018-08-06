using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.UserInterface;

namespace Capstone.Models
{
   public class Reservation
    {
        public int Reservation_Id { get; set; }
        public int Site_Id { get; set; }
        public string Name { get; set; }
        public DateTime From_Date { get; set; } //GET JUST DATE
        public DateTime To_Date { get; set; } //GET JUST DATE
        public DateTime Create_Date { get; set; } //GET JUST DATE
    }
}
