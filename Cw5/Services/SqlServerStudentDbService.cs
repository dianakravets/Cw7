

using System;
using System.Data.SqlClient;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    

    {
            public void EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.IndexNumber = request.IndexNumber;
            st.FirstName = request.FirstName;
            st.LastName = request.LastName;
            st.BirthDate = request.BirthDate;
            var response = new EnrollStudentResponse();
            bool success;
            //int success;
            DateTime date = DateTime.Today;
            const string ConString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19054;User ID=apbds19054;Password=admin";
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                
                com.CommandText = "select IdStudy from studies where name = @studies";
                com.Parameters.AddWithValue("studies", request.Studies);
                com.Transaction = tran;
                var dr = com.ExecuteScalar();
                if (dr == null)
                {
                    tran.Rollback();
                    success = false;
                }
                int idStudy = (int)dr;
                int idEnroll;
                int idEnrollMax;
                com.CommandText = "select MAX(IdEnrollment) from Enrollment";
                com.Transaction = tran;
                dr = com.ExecuteScalar();
                idEnrollMax = (int)dr;
                com.CommandText = "select IdEnrollment from Enrollment where Semester = 1 and IdStudy = @idstudy";
                com.Parameters.AddWithValue("idstudy", idStudy);
                com.Transaction = tran;
                dr = com.ExecuteScalar();
                if (dr == null)
                {
                    idEnroll = idEnrollMax + 1;
                    DateTime start = DateTime.Today;
                    com.CommandText = "insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate) values (@idenroll, 1, @idstudy, @date)";
                    com.Parameters.AddWithValue("idenroll", idEnroll);
                    com.Parameters.AddWithValue("date", start);
                    com.Transaction = tran;
                    com.ExecuteNonQuery();
                }
                else
                {
                    idEnroll = (int)dr;
                }
                com.CommandText = "select lastname from student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", st.IndexNumber);
                com.Transaction = tran;
                dr = com.ExecuteScalar();
                if (dr != null)
                {
                    tran.Rollback();
                    success = false;
                }
                com.CommandText = "insert into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values(@index, @fn, @ln, @birth, @idenroll2)";
                com.Parameters.AddWithValue("fn", st.FirstName);
                com.Parameters.AddWithValue("ln", st.LastName);
                com.Parameters.AddWithValue("birth", st.BirthDate);
                com.Parameters.AddWithValue("idenroll2", idEnroll);
                com.Transaction = tran;
                com.ExecuteNonQuery();
                response.IndexNumber = st.IndexNumber;
                response.IdEnrollment = idEnroll;
                response.Semester = 1;
                response.Studies = request.Studies;
                response.StartDate = date;
                tran.Commit();
                success = true;
            }
          
           
        }

            public void PromoteStudents(int semester, string studies)
            {
                throw new NotImplementedException();
            }

            public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {
            var response = new PromoteStudentsResponse();
            DateTime date = DateTime.Today;
            int success;
            const string ConString =  "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19054;User ID=apbds19054;Password=admin";
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.CommandText = "execute PromoteStudents @study, @semester";
                com.Parameters.AddWithValue("study", request.Studies);
                com.Parameters.AddWithValue("semester", request.Semester);
                com.Transaction = tran;
                success = com.ExecuteNonQuery();
                var nextSemestr = request.Semester + 1;
                com.CommandText = "select IdEnrollment from enrollment where semester = @nextSemester and idstudy = (select idstudy from studies where name = @study)";
                com.Parameters.AddWithValue("nextSemester", nextSemestr);
                com.Transaction = tran;
                var dr = com.ExecuteScalar();
                response. IdEnrollment=(int) dr;
                com.CommandText = "select StartDate from enrollment where semester = @nextSemester and idstudy = (select idstudy from studies where name = @study)";
                com.Transaction = tran;
                dr = com.ExecuteScalar();
                response.StartDate = (DateTime) dr;
                tran.Commit();
                response.Studies = request.Studies;
                response.Semester = request.Semester + 1;
                
                if (success>0)
                {
                    return response;
                }
                else
                {
                    return null;
                }
                
            }
        }
        public Student GetStudent(string IndexNumer)
        {
            const string ConString =  "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19054;User ID=apbds19054;Password=admin";
            var stud = new Student();
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "select * from Student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", IndexNumer);
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return null;
                }
                else
                {
                    while (dr.Read())
                    {
                        stud.IndexNumber = dr["IndexNumber"].ToString();
                        stud.FirstName = dr["FirstName"].ToString();
                        stud.LastName = dr["LastName"].ToString();
                        stud.BirthDate = (DateTime)dr["BirthDate"];
                    }
                    return stud;
                }
                
            }
        }


        public Request_log Login(Request_log request)
        {
            const string ConString =  "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19054;User ID=apbds19054;Password=admin";
            string Login = request.Login;
            string Password = request.Password;
            Request_log response = new Request_log();

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "select * from student where IndexNumber = @index;";
                com.Parameters.AddWithValue("index", request.Login);
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    Login = dr["IndexNumber"].ToString();
                    Password = dr["Password"].ToString();
                }

            }

            if (request.Login == Login && request.Password == Password)
            {
                response.Login = Login;
                response.Password = Password;

                return response;
            }



            return null;

        }

    }

    
}
    
