
using cw9.DTOs.Request;
using cw9.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw9.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService _service;
        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [Route("api/students")]
        [HttpGet]
        public IActionResult getStudents()
        {
            var studentList = _service.AllStudents();
            if (studentList.Count < 1)
            {
                return new NotFoundResult();
            }
            return Ok(studentList);
        }
        
        [HttpPost]
        [Route("api/students/update")]
        public IActionResult updateStudent(StudentModifyRequest student)
        {
            if (student.FirstName == null && student.LastName == null)
                return BadRequest("Missing arguments");

            _service.ModifyStudent(student);

            return Ok(student);
        }

        [HttpGet]
        [Route("api/test/post")]
        public IActionResult test(StudentModifyRequest student)
        {
            return Ok();
        }
    }
}
