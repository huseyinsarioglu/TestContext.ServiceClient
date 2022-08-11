using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TestContext.ServiceClient.APISandbox.Model;

namespace TestContext.ServiceClient.APISandbox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private const string idParametereTemplate = "{id:int}";

        private readonly IStudentContext _studentContext;

        public StudentController(IStudentContext studentContext)
        {
            _studentContext = studentContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Student>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            var students = await _studentContext.GetStudentsAsync();
            return Ok(students);
        }

        [HttpGet(idParametereTemplate)]
        [ProducesResponseType(typeof(Student), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Student>>> GetById(int id)
        {
            var student = await _studentContext.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Student), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Student>> CreateStudent([Required][FromBody] Student student)
        {
            await _studentContext.AddStudentAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Student), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Student>> Update([Required][FromBody] Student student)
        {
            var result = await _studentContext.UpdateStudentAsync(student);
            if (!result)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpDelete(idParametereTemplate)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Student>>> DeleteById(int id)
        {
            var success = await _studentContext.DeleteStudentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
