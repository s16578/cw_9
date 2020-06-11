using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw9.DTOs.Request
{
    public class StudentModifyRequest
    {
        [Required]
        public string Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
