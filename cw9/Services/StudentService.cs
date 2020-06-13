using cw9.DTOs;
using cw9.DTOs.Request;
using cw9.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

        public void ModifyStudentDB(StudentModifyRequest studentModifyRequest)
        {

            var updatedStudent = new Student
            {
                IndexNumber = studentModifyRequest.Index,
                FirstName = studentModifyRequest.FirstName,
                LastName = studentModifyRequest.LastName
            };


            var student = db.Student.Where(s => s.IndexNumber == studentModifyRequest.Index).FirstOrDefault();
            if(studentModifyRequest.FirstName != null)
                student.LastName = studentModifyRequest.LastName;
            if(studentModifyRequest.LastName != null)
                student.FirstName = studentModifyRequest.FirstName;
            //db.Student.Attach(updatedStudent);

            //if (updatedStudent.FirstName != null)
            //{
            //    db.Entry(updatedStudent).Property(x => x.FirstName).IsModified = true;
            //}
            //if (updatedStudent.LastName != null)
            //{
            //    db.Entry(updatedStudent).Property(x => x.LastName).IsModified = true;
            //}
            //db.Entry(updatedStudent).State = EntityState.Modified;
            
            db.SaveChanges();
        }
        public bool DeleteStudentDB(string id)
        {
            if(db.Student.Where(s => s.IndexNumber == id).Any())
            {
                var student = db.Student.Where(s => s.IndexNumber == id).FirstOrDefault();
                db.Student.Attach(student);
                db.Entry(student).State = EntityState.Deleted;
                return true;
            }
            return false;
        }
    }
}
