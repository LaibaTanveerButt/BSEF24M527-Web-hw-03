namespace Student_Attendance_Management.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Department { get; set; }
        public decimal AttendancePercentage { get; set; }

        // Empty constructor (needed by MVC for forms)
        public Student()
        {
        }

        // Constructor used by StudentRepository
        public Student(int id, string name, string regNo, string dept, decimal attendance)
        {
            StudentId = id;
            StudentName = name;
            RegistrationNumber = regNo;
            Department = dept;
            AttendancePercentage = attendance;
        }
    }
}
