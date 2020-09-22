using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterClassAPI.Models
{
    public class History
    {
        [Key]
        public int Hist_Id { get; set; }

        [ForeignKey("Attend")]
        public int Att_Id { get; set; }
        public DateTime Hist_Date { get; set; }
        public int Hist_Interactions { get; set; }

        //public virtual Attend Attend { get; set; }
        public Attend Attend { get; set; }
    }
}