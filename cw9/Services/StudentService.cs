using cw9.DTOs;
using cw9.DTOs.Request;
using cw9.DTOs.Response;
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
        public bool DeleteStudentDB(DeleteStudentRequest student)
        {
            if(db.Student.Where(s => s.IndexNumber == student.Index).Any())
            {
                var res = db.Student.Where(s => s.IndexNumber == student.Index).FirstOrDefault();
                //db.Student.Attach(res);
                //db.Entry(res).State = EntityState.Deleted;
                db.Student.Remove(res);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest newStudent)
        {
            if(db.Student.Where(s => s.IndexNumber == newStudent.Index).Any())
            {
                throw new InvalidOperationException("Student with index already exist");
            }

            if(!db.Studies.Where(st => st.Name == newStudent.Studies).Any())
            {
                throw new InvalidOperationException("Studies does not exist");
            }

            int idstudy = db.Studies.Where(st => st.Name == newStudent.Studies).Select(s => s.IdStudy).FirstOrDefault();
            int idenroll;
            if(!db.Enrollment.Where(en => en.Semester == 1).Where(en => en.IdStudy == idstudy).Any())
            {
                idenroll = db.Enrollment.OrderByDescending(x => x.IdEnrollment).Select(xx => xx.IdEnrollment).First() + 1;
                var insertEnroll = new Enrollment
                {
                    IdEnrollment = idenroll,
                    Semester = 1,
                    IdStudy = idstudy,
                    StartDate = DateTime.Now
                };
                db.Enrollment.Add(insertEnroll);
                db.SaveChanges();
            }

            idenroll = db.Enrollment.Where(en => en.Semester == 1).Where(en => en.IdStudy == idstudy).Select(x => x.IdEnrollment).FirstOrDefault();

            var insertStudent = new Student
            {
                IndexNumber = newStudent.Index,
                FirstName = newStudent.FirstName,
                LastName = newStudent.LastName,
                IdEnrollment = idenroll,
                BirthDate = newStudent.BirthDate,
            };

            db.Student.Add(insertStudent);
            db.SaveChanges();

            return new EnrollStudentResponse { IdEnrollment = idenroll, IdStudy = idstudy, Semester = 1, StartDate = DateTime.Now};
        }

        public PromotionStudentRepsonse PromotionStudents(EnrollStudentPromotionsRequest promotionsRequest)
        {
            var studyName = promotionsRequest.Studies;
            var semester = promotionsRequest.Semester;

            if(!db.Studies.Where(x => x.Name == studyName).Any())
            {
                throw new InvalidOperationException();
            }

            var idstudy = db.Studies.Where(x => x.Name == studyName).Select(x => x.IdStudy).FirstOrDefault();

            var newEnrollment = new Enrollment
            {
                IdEnrollment = (db.Enrollment.Max(x => x.IdEnrollment) + 1),
                IdStudy = idstudy,
                StartDate = DateTime.Now,
                Semester = semester
            };

            if (!db.Enrollment.Where(x => x.IdStudy ==  idstudy).Where(x => x.Semester == semester).Any())
            {
                db.Enrollment.Add(newEnrollment);
                db.SaveChanges();
                return new PromotionStudentRepsonse
                {
                    IdEnrollment = newEnrollment.IdEnrollment,
                    IdStudy = idstudy,
                    StartDate = DateTime.Now,
                    Semester = semester
                };
            }
            else
            {
                var oldIdEnrollment = db.Enrollment.Where(x => x.IdStudy == idstudy).Where(x => x.Semester == semester).Select(x => x.IdEnrollment).FirstOrDefault();
                int newIdEnrollment;
                newEnrollment.Semester += 1;

                if (!db.Enrollment.Where(x => x.IdStudy == idstudy).Where(x => x.Semester == (semester+1)).Any())
                {
                    db.Enrollment.Add(newEnrollment);
                    db.SaveChanges();
                }
                newIdEnrollment = newEnrollment.IdEnrollment;

                var students = db.Student.Where(x => x.IdEnrollment == oldIdEnrollment).ToList();
                students.ForEach(x => x.IdEnrollment = newIdEnrollment);
                db.SaveChanges();

                return new PromotionStudentRepsonse
                {
                    IdEnrollment = newIdEnrollment,
                    IdStudy = idstudy,
                    StartDate = DateTime.Now,
                    Semester = semester
                };
            }
        }
    }
}
