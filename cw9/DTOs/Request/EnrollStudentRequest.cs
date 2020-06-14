using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw9.DTOs.Request
{
    public class EnrollStudentRequest
    {
        public string Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Studies { get; set; }
    }
}
