using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _30_Data.Entities
{
    public class Favorite
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;

        public User? User { get; set; } = null!;
    }
}
