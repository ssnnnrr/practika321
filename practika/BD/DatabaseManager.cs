using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Windows.Documents;

namespace practika.BD
{
    public class DatabaseManager
    {
        private static BDEntities _context = new BDEntities();

        public static BDEntities GetContext()
        {
            if (_context == null)
                _context = new BDEntities();
            return _context;
        }

        public static bool UpdateDatabase()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static (string, Department) GetRoleAndDepartmentByTabNumber(int tabNumber)
        {
            var employee = _context.Employee.FirstOrDefault(e => e.tab_number == tabNumber);
            if (employee == null)
                return (null, null);

            string role = null;
            switch (employee.post)
            {
                case "инженер":
                    role = Role.Инженер;
                    break;
                case "зав. кафедрой":
                    role = Role.ЗавКафедрой;
                    break;
                case "преподаватель":
                    role = Role.Преподаватель;
                    break;
            }

            var department = _context.Department.FirstOrDefault(d => d.code == employee.code);
            return (role, department);
        }

        public static List<Employee> GetEmployee(string post = null)
        {
            if (!string.IsNullOrEmpty(post))
            {
                return _context.Employee.Where(e => e.post == post).ToList();
            }

            return _context.Employee.ToList();
        }

        public static List<Discipline> GetDiscipline(Department userDepartment = null)
        {
            if (userDepartment != null)
            {
                return _context.Discipline.Where(d => d.executor == userDepartment.code).ToList();
            }
            return _context.Discipline.ToList();
        }

        public static List<Student> GetStudent()
        {
            return _context.Student.ToList();
        }

        public static List<Faculty> GetFaculty()
        {
            return _context.Faculty.ToList();
        }

        public static List<Exam> GetExam(Department userDepartment = null)
        {
            if (userDepartment != null&&int.TryParse(userDepartment.code, out int departmentCode))
            {
                return _context.Exam.Where(e => e.kod == departmentCode).ToList();
            }
            return _context.Exam.ToList();
        }

        public static List<Department> GetDepartment(Department userDepartment = null)
        {
            if (userDepartment != null)
            {
                return _context.Department.Where(d => d.code == userDepartment.code).ToList();
            }
            return _context.Department.ToList();
        }

        public static List<Teacher> GetTeacher(Department userDepartment = null)
        {
            if (userDepartment != null && int.TryParse(userDepartment.code, out int departmentCode))
            {
                return _context.Teacher.Where(t => t.tab_num == departmentCode).ToList();
            }
            return _context.Teacher.ToList();
        }
        public static void AddDepartment(Department department, string userRole)
        {
            if (userRole == Role.ЗавКафедрой)
            {
                throw new InvalidOperationException("Зав. кафедрой не может добавлять новые кафедры.");
            }
            _context.Department.Add(department);
            _context.SaveChanges();
            UpdateDatabase();
        }

        public static void UpdateDepartment(Department department)
        {
            var existingDepartment = _context.Department.FirstOrDefault(d => d.code == department.code);
            if (existingDepartment != null)
            {
                existingDepartment.name = department.name;
                existingDepartment.id_faculty = department.id_faculty;
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

        public static void DeleteDepartment(string code)
        {
            var department = _context.Department
                .Include(d => d.Employee)
                .FirstOrDefault(d => d.code == code);

            if (department != null)
            {
                _context.Employee.RemoveRange(department.Employee);
                _context.Department.Remove(department);
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

        public static void AddDiscipline(Discipline discipline)
        {
            _context.Discipline.Add(discipline);
            _context.SaveChanges();
            UpdateDatabase();
        }

        public static void UpdateDiscipline(Discipline discipline)
        {
            var existingDiscipline = _context.Discipline.FirstOrDefault(d => d.kod == discipline.kod);
            if (existingDiscipline != null)
            {
                existingDiscipline.name = discipline.name;
                existingDiscipline.volume = discipline.volume;
                existingDiscipline.executor = discipline.executor;
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

        //public static void UpdateExam(Exam exam)
        //{
        //    var existingExam = _context.Exam
        //        .Where(e => e.id_exam == exam.id_exam)
        //        .FirstOrDefault();

        //    if (existingExam != null)
        //    {
        //        existingExam.date = exam.date;
        //        existingExam.kod = exam.kod;
        //        existingExam.reg_num = exam.reg_num;
        //        existingExam.auditorium = exam.auditorium;
        //        existingExam.estimation = exam.estimation;
        //        _context.SaveChanges();
        //        UpdateDatabase();
        //    }
        //}

        public static void UpdateTeacher(Teacher teacher)
        {
            var existingTeacher = _context.Teacher.FirstOrDefault(t => t.tab_num == teacher.tab_num);
            if (existingTeacher != null)
            {
                existingTeacher.rank = teacher.rank;
                existingTeacher.degree = teacher.degree;
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

        public static void DeleteDiscipline(int kod)
        {
            var discipline = _context.Discipline.FirstOrDefault(d => d.kod == kod);
            if (discipline != null)
            {
                _context.Discipline.Remove(discipline);
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

        public static void AddExam(Exam exam)
        {
            _context.Exam.Add(exam);
            _context.SaveChanges();
            UpdateDatabase();
        }

       

        public static void DeleteExam(int id_exam)
        {
            var exam = _context.Exam.FirstOrDefault(e => e.id_exam == id_exam);
            if (exam != null)
            {
                _context.Exam.Remove(exam);
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

        public static void AddTeacher(Teacher teacher)
        {
            _context.Teacher.Add(teacher);
            _context.SaveChanges();
            UpdateDatabase();
        }

       

        public static void DeleteTeacher(int tab_num)
        {
            var teacher = _context.Teacher.FirstOrDefault(t => t.tab_num == tab_num);
            if (teacher != null)
            {
                _context.Teacher.Remove(teacher);
                _context.SaveChanges();
                UpdateDatabase();
            }
        }

    }
}