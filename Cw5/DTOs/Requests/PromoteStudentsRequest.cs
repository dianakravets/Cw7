using System;

namespace Cw5.DTOs.Requests
{
    public class PromoteStudentsRequest
    {
        public int Semester { get; set; }
        public DateTime BirthDate { get; set; }
        
        public string Studies { get; set; }
    }
}