namespace Demo2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? StudentName { get; set; }
        public string? Department { get; set; }
        public Student() { }
        public Student(int id, string studentname, string dept)
        {
            this.Id = id;
            this.StudentName = studentname;
            this.Department = dept;

        }


    }
}