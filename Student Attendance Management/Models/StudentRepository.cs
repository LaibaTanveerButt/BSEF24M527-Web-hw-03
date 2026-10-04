using Microsoft.Data.SqlClient;
using System.Data;

namespace Student_Attendance_Management.Models
{
    public class StudentRepository
    {
        private string connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=AttendanceDB;Trusted_Connection=True;TrustServerCertificate=True";

        SqlDataAdapter da = new SqlDataAdapter();


        // Fill in-memory DataTable from database
        public async Task<DataTable> FillTable()
        {
            DataTable table = new DataTable();

            string query = "SELECT * FROM Students";

            SqlConnection conn =
                new SqlConnection(connectionString);

            SqlCommand selectCommand =
                new SqlCommand(query, conn);

            da.SelectCommand = selectCommand;

            await Task.Run(() =>
            {
                da.Fill(table);
            });

            // Set StudentId as primary key of in-memory table
            DataColumn[] primaryKey = new DataColumn[1];

            primaryKey[0] = table.Columns["StudentId"];

            table.PrimaryKey = primaryKey;

            // Let the in-memory table create a temporary id for new rows
            table.Columns["StudentId"].AutoIncrement = true;
            table.Columns["StudentId"].AutoIncrementSeed = -1;
            table.Columns["StudentId"].AutoIncrementStep = -1;

            return table;
        }


        // View all students
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            DataTable table = await FillTable();

            List<Student> students = new List<Student>();

            foreach (DataRow row in table.Rows)
            {
                Student student = new Student(
                    Convert.ToInt32(row["StudentId"]),
                    row["StudentName"].ToString(),
                    row["RegistrationNumber"].ToString(),
                    row["Department"].ToString(),
                    Convert.ToDecimal(row["AttendancePercentage"])
                );

                students.Add(student);
            }

            return students;
        }


        // Get one student by ID
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            DataTable table = await FillTable();

            DataRow row = table.Rows.Find(id);

            if (row != null)
            {
                Student student = new Student(
                    Convert.ToInt32(row["StudentId"]),
                    row["StudentName"].ToString(),
                    row["RegistrationNumber"].ToString(),
                    row["Department"].ToString(),
                    Convert.ToDecimal(row["AttendancePercentage"])
                );

                return student;
            }

            return null;
        }


        // ADD STUDENT
        public async Task AddStudentAsync(Student student)
        {
            // Get database data into memory
            DataTable table = await FillTable();


            // INSERT command
            string query =
                "INSERT INTO Students " +
                "(StudentName, RegistrationNumber, Department, AttendancePercentage) " +
                "VALUES (@sname, @regis, @depart, @attendance)";

            SqlConnection conn =
                new SqlConnection(connectionString);

            SqlCommand insertCommand =
                new SqlCommand(query, conn);


            // Parameters
            SqlParameter name =
                new SqlParameter(
                    "sname",
                    SqlDbType.NVarChar,
                    100,
                    "StudentName");

            SqlParameter registration =
                new SqlParameter(
                    "regis",
                    SqlDbType.NVarChar,
                    30,
                    "RegistrationNumber");

            SqlParameter department =
                new SqlParameter(
                    "depart",
                    SqlDbType.NVarChar,
                    50,
                    "Department");

            SqlParameter attendance =
                new SqlParameter(
                    "attendance",
                    SqlDbType.Decimal,
                    0,
                    "AttendancePercentage");

            attendance.Precision = 5;
            attendance.Scale = 2;


            insertCommand.Parameters.Add(name);
            insertCommand.Parameters.Add(registration);
            insertCommand.Parameters.Add(department);
            insertCommand.Parameters.Add(attendance);


            // Give INSERT command to DataAdapter
            da.InsertCommand = insertCommand;


            // Add student to in-memory table
            DataRow row = table.NewRow();

            row["StudentName"] =
                student.StudentName;

            row["RegistrationNumber"] =
                student.RegistrationNumber;

            row["Department"] =
                student.Department;

            row["AttendancePercentage"] =
                student.AttendancePercentage;

            table.Rows.Add(row);


            // Send in-memory changes to database
            await Task.Run(() =>
            {
                da.Update(table);
            });
        }


        // UPDATE STUDENT
        public async Task UpdateStudentAsync(Student student)
        {
            // Get database data into memory
            DataTable table = await FillTable();


            // UPDATE command
            string query =
                "UPDATE Students SET " +
                "StudentName=@sname, " +
                "RegistrationNumber=@regis, " +
                "Department=@depart, " +
                "AttendancePercentage=@attendance " +
                "WHERE StudentId=@sid";

            SqlConnection conn =
                new SqlConnection(connectionString);

            SqlCommand updateCommand =
                new SqlCommand(query, conn);


            // Parameters
            SqlParameter name =
                new SqlParameter(
                    "sname",
                    SqlDbType.NVarChar,
                    100,
                    "StudentName");

            SqlParameter registration =
                new SqlParameter(
                    "regis",
                    SqlDbType.NVarChar,
                    30,
                    "RegistrationNumber");

            SqlParameter department =
                new SqlParameter(
                    "depart",
                    SqlDbType.NVarChar,
                    50,
                    "Department");

            SqlParameter attendance =
                new SqlParameter(
                    "attendance",
                    SqlDbType.Decimal,
                    0,
                    "AttendancePercentage");

            attendance.Precision = 5;
            attendance.Scale = 2;

            SqlParameter id =
                new SqlParameter(
                    "sid",
                    SqlDbType.Int,
                    0,
                    "StudentId");


            updateCommand.Parameters.Add(name);
            updateCommand.Parameters.Add(registration);
            updateCommand.Parameters.Add(department);
            updateCommand.Parameters.Add(attendance);
            updateCommand.Parameters.Add(id);


            // Give UPDATE command to DataAdapter
            da.UpdateCommand = updateCommand;


            // Find student in memory
            DataRow row = table.Rows.Find(student.StudentId);

            if (row != null)
            {
                row["StudentName"] =
                    student.StudentName;

                row["RegistrationNumber"] =
                    student.RegistrationNumber;

                row["Department"] =
                    student.Department;

                row["AttendancePercentage"] =
                    student.AttendancePercentage;
            }


            // Send changes to database
            await Task.Run(() =>
            {
                da.Update(table);
            });
        }


        // DELETE STUDENT
        public async Task DeleteStudentAsync(int id)
        {
            // Get database data into memory
            DataTable table = await FillTable();


            // DELETE command
            string query =
                "DELETE FROM Students " +
                "WHERE StudentId=@sid";

            SqlConnection conn =
                new SqlConnection(connectionString);

            SqlCommand deleteCommand =
                new SqlCommand(query, conn);


            SqlParameter studentId =
                new SqlParameter(
                    "sid",
                    SqlDbType.Int,
                    0,
                    "StudentId");

            deleteCommand.Parameters.Add(studentId);


            // Give DELETE command to DataAdapter
            da.DeleteCommand = deleteCommand;


            // Find student in memory
            DataRow row = table.Rows.Find(id);

            if (row != null)
            {
                row.Delete();
            }


            // Send changes to database
            await Task.Run(() =>
            {
                da.Update(table);
            });
        }
    }
}
