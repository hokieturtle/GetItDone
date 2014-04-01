using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DateTime Created { get; set; }
        public Nullable<DateTime> Due { get; set; }
        public Nullable<int> Priority { get; set; }

    }
}
