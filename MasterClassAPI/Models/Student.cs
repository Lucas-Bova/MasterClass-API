using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterClassAPI.Models
{
    public class Student
    {
        [Key]
        public int Stu_Id { get; set; }
        public string Stu_FName { get; set; }
        [Required]
        public string Stu_LName { get; set; }
        public string Stu_Pic { get; set; }

        [ForeignKey("ApplicationUser")]
        public string User_Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Attend> Attend { get; set; }
    }
}