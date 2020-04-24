using Cw5.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Services
{
    public interface IStudentDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(int semester, string studies);
    }
}