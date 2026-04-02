using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolActivities.Business.Services.Interfaces;
using SchoolActivities.Data.Persistance;
using SchoolActivities.Models.Domain.Entities;
using SchoolActivities.Models.Dtos.Student;

namespace SchoolActivities.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentServicce)
        {
            _studentService = studentServicce;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IEnumerable<StudentResponseDto>> GetStudents()
        {
            return await _studentService.GetAllAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponseDto>> GetStudent(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, StudentRequestDto dto)
        {
            var student = _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            await _studentService.UpdateAsync(id, dto);

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> PostStudent(StudentRequestDto dto)
        {
            
            var student = await _studentService.CreateAsync(dto);
            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            await _studentService.DeleteAsync(id);

            return NoContent();
        }
    }
}
