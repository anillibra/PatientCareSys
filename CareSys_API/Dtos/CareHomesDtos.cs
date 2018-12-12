using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Dtos
{
    public class CareHomesDtos
    {
        public Int64 Id { get; set; }
       
        public string Name { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public Boolean IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
    }
}
