using System;
using System.ComponentModel.DataAnnotations;

namespace Cw5.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        [RegularExpression("^s[0-9]+$")]
        public string  IndexNumber { get; set; }
        [Required(ErrorMessage ="Podaj imię")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Podaj nazwisko")]
        [MaxLength(250)]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage ="Podaj studia")]
        public string Studies { get; set; }
    }
}