using cw9.DTOs;
using cw9.DTOs.Request;
using cw9.DTOs.Response;
using System.Collections.Generic;

namespace cw9.Services
{
    public interface IStudentService
    {
        List<StudentResponse> AllStudents();

        void ModifyStudentDB(StudentModifyRequest studentModifyRequest);

        bool DeleteStudentDB(DeleteStudentRequest student);

        EnrollStudentResponse EnrollStudent(EnrollStudentRequest newStudent);

        PromotionStudentRepsonse PromotionStudents(EnrollStudentPromotionsRequest promotionsRequest);
    }
}
