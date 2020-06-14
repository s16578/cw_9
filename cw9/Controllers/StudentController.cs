using cw9.DTOs.Request;
using cw9.DTOs.Response;
using cw9.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;

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
        public IActionResult GetStudents()
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
        public IActionResult UpdateStudent(StudentModifyRequest student)
        {
            if (student.FirstName == null && student.LastName == null)
                return BadRequest("Missing arguments");

            _service.ModifyStudentDB(student);

            return Ok(student);
        }

        [HttpPost]
        [Route("api/students/delete")]
        public IActionResult DeleteStudent(DeleteStudentRequest id)
        {
            if(id.Index == null)
            {
                return BadRequest("Missing id");
            }
            if (_service.DeleteStudentDB(id) == true)
                return Ok("Student has been removed with index: " + id.Index);
            else
                return BadRequest("Student with this index does not exist");
            
        }


        [HttpPost]
        [Route("api/enrollStudent")]
        public IActionResult EnrollStudent(EnrollStudentRequest enrollStudent)
        {
            EnrollStudentResponse response;
            try
            {
                response = _service.EnrollStudent(enrollStudent);
            }catch(InvalidOperationException op)
            {
                return BadRequest(op);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("api/")]

    }
}
