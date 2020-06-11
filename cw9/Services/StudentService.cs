using cw9.DTOs;
using cw9.DTOs.Request;
using cw9.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw9.Services
{
    public class StudentService : IStudentService
    {
        s16578Context db = new s16578Context();
        public List<StudentResponse> AllStudents()
        {
            
            var res = db.Student.Select(d => new StudentResponse
            {
                FirstName = d.FirstName,
                LastName = d.LastName,
                BirthDate = d.BirthDate,
                IndexNumber = d.IndexNumber,
                IdEnrollment = d.IdEnrollment
            }).ToList();

            return res;
        }

        public void ModifyStudent(StudentModifyRequest studentModifyRequest)
        {

            var updatedStudent = new Student
            {
                IndexNumber = studentModifyRequest.Index,
                FirstName = studentModifyRequest.FirstName,
                LastName = studentModifyRequest.LastName
            };

            db.Attach(updatedStudent);

            if (updatedStudent.FirstName != null)
            {
                db.Entry(updatedStudent).Property("FirstName").IsModified = true;
            }
            if (updatedStudent.LastName != null)
            {
                db.Entry(updatedStudent).Property("LastName").IsModified = true;
            }
            db.Entry(updatedStudent).State = EntityState.Modified;
            db.SaveChanges();

            
        }
    }
}
