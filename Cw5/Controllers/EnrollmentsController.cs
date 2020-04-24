using System;
using System.Data;
using System.Data.SqlClient;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }
       
        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);
            var response = new EnrollStudentResponse();
            
            return Ok(response);
        }
        [HttpPost("/api/enrollments/promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(int semester, string studies)
        {
            var newenrollment = new EnrollStudentResponse();
            return Created("", newenrollment);
        }
    }
}
   
    
    
