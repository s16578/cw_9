using cw9.DTOs;
using cw9.DTOs.Request;
using cw9.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw9.Services
{
    public interface IStudentService
    {
        public List<StudentResponse> AllStudents();

        public void ModifyStudentDB(StudentModifyRequest studentModifyRequest);

        public bool DeleteStudentDB(string id);
    }
}
