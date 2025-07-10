using Demo2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Demo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection _connection;
        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }
        [HttpGet]
        public IActionResult Get(int id)
        {
            string query = "SELECT * FROM studentdb";
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Student> students = new List<Student>();

            while (reader.Read())
            {
                var student = new Student
                {
                    Id = reader.GetInt32("Id"),
                    StudentName = reader.GetString("StudentName"),
                    Department = reader.GetString("Department")
                };
                students.Add(student);
            }

            return Ok(students);
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            string query = "Insert into studentdb(Id,StudentName,Department) values (@id,@name,@dept)";
            var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", student.Id);
            cmd.Parameters.AddWithValue("@name", student.StudentName);
            cmd.Parameters.AddWithValue("@dept", student.Department);
            int count = cmd.ExecuteNonQuery();
            if (count == 1)
            {
                return Ok(student);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            string query = "Delete from studentdb where Id=@id";
            var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);
            int count = cmd.ExecuteNonQuery();
            if (count == 1)
            {
                return Ok($"Deleted student with ID: {id}");
            }
            else
            {
                return NotFound($"Student with ID {id} not found.");
            }
        }
        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            string query = "Update studentdb set StudentName=@name, Department=@dept where Id=@id";
            var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", student.Id);
            cmd.Parameters.AddWithValue("@name", student.StudentName);
            cmd.Parameters.AddWithValue("@dept", student.Department);
            int count = cmd.ExecuteNonQuery();
            if (count == 1)
            {
                return Ok("Updated successfully");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}