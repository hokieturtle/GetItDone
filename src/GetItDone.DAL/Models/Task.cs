using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetItDone.DAL.Models
{
    public class Task
    {
        public Task()
        {
            Created = DateTime.Now;
        }
        [Key]
        public int TaskID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public virtual User Owner { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }
        [Column(TypeName = "datetime2")]
        
        public Nullable<DateTime> Due { get; set; }
        public Nullable<int> Priority { get; set; }

        public bool Done { get; set; }
    }
}
