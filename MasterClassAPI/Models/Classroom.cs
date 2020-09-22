using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterClassAPI.Models
{
    public class Classroom
    {
        [Key]
        public int Cls_Id { get; set; }

        [Required]
        public string Cls_Name { get; set; }

        [ForeignKey("ApplicationUser")]
        public string User_Id { get; set; }

        public ICollection<Attend> Attends { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}