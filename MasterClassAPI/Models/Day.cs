using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterClassAPI.Models
{
    public class Day
    {
        [Key]
        [ForeignKey("Attend")]
        public int Att_Id { get; set; }

        public int Day_Interactions { get; set; }

        public Attend Attend { get; set; }
    }
}