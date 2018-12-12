using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Models
{
    [Table(name:"CareHome")]
    public class CareHome: BaseEntity
    {
       
        public string Name { get; set; }       
        public string Category { get; set; }        
        public string Location { get; set; }
        public string Address { get; set; }
        public Boolean IsActive { get; set; }
    }
}
