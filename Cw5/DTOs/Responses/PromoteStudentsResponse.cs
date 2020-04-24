using System;

namespace Cw5.DTOs.Responses
{
    public class PromoteStudentsResponse
    {
        public DateTime StartDate { get; set; }
        public int Semester { get; set; }
        public string Studies { get; set; }
        public int IdEnrollment { get; set; }

    }
}