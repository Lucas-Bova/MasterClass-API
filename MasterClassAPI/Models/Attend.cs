using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterClassAPI.Models
{
    public class Attend
    {
        public Attend()
        {
            Histories = new HashSet<History>();
        }
        [Key]
        public int Att_Id { get; set; }

        [Required]
        [ForeignKey("Student"), Column(Order = 0)]
        public int Stu_Id { get; set; }

        [Required]
        [ForeignKey("Classroom"), Column(Order = 1)]
        public int Cls_Id { get; set; }

        public bool Att_Active { get; set; }

        public Classroom Classroom { get; set; }
        public Student Student { get; set; }
        public Day Day { get; set; }
        //public virtual ICollection<History> Histories { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}