using System;

namespace Cw5.DTOs.Responses
{
    public class EnrollStudentResponse
    {
      
        public DateTime StartDate { get; set; }
        
        public string IndexNumber{ get; set; }
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public string Studies { get; set; }
    }
}