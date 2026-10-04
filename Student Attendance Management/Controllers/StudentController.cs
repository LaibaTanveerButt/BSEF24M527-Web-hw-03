using Microsoft.AspNetCore.Mvc;
using Student_Attendance_Management.Models;

namespace Student_Attendance_Management.Controllers
{
    public class StudentController : Controller
    {
        StudentRepository repo = new StudentRepository();


        // Simple checks used by Create and Edit
        private void CheckStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.StudentName))
            {
                ModelState.AddModelError("StudentName", "Name is required");
            }

            if (string.IsNullOrWhiteSpace(student.RegistrationNumber))
            {
                ModelState.AddModelError("RegistrationNumber", "Registration number is required");
            }

            if (string.IsNullOrWhiteSpace(student.Department))
            {
                ModelState.AddModelError("Department", "Department is required");
            }

            if (student.AttendancePercentage < 0 || student.AttendancePercentage > 100)
            {
                ModelState.AddModelError("AttendancePercentage", "Attendance must be between 0 and 100");
            }
        }


        // VIEW ALL STUDENTS
        public async Task<IActionResult> Index()
        {
            List<Student> students = await repo.GetAllStudentsAsync();

            return View(students);
        }


        // ADD - show empty form
        public IActionResult Create()
        {
            return View();
        }


        // ADD - save the form data
        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            CheckStudent(student);

            if (ModelState.IsValid == false)
            {
                return View(student);
            }

            await repo.AddStudentAsync(student);

            return RedirectToAction("Index");
        }


        // EDIT - show form with old data
        public async Task<IActionResult> Edit(int id)
        {
            Student student = await repo.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // EDIT - save the changes
        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            CheckStudent(student);

            if (ModelState.IsValid == false)
            {
                return View(student);
            }

            await repo.UpdateStudentAsync(student);

            return RedirectToAction("Index");
        }


        // DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await repo.DeleteStudentAsync(id);

            return RedirectToAction("Index");
        }
    }
}
