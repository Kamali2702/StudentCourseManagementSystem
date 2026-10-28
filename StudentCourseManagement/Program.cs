using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Services;

namespace StudentCourseManagement;

public class Program
{
    public static async Task Main(string[] args)
    {
        DepartmentRepository departmentRepository = new();
        DepartmentService departmentService =
            new(departmentRepository);

        StudentRepository studentRepository = new();
        StudentService studentService =
            new(studentRepository);

        CourseRepository courseRepository = new();
        CourseService courseService =
            new(courseRepository);

        EnrollmentRepository enrollmentRepository = new();

        EnrollmentService enrollmentService =
            new(
                enrollmentRepository,
                studentRepository,
                courseRepository);

        AuditLogRepository auditLogRepository = new();
        AuditLogService auditLogService =
            new(auditLogRepository);

        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("   STUDENT COURSE MANAGEMENT SYSTEM");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Department Management");
            Console.WriteLine("2. Student Management");
            Console.WriteLine("3. Course Management");
            Console.WriteLine("4. Enrollment Management");
            Console.WriteLine("5. Reports");
            Console.WriteLine("6. View Audit Logs");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await DepartmentMenuAsync(departmentService);
                        break;

                    case "2":
                        await StudentMenuAsync(studentService);
                        break;

                    case "3":
                        await CourseMenuAsync(courseService);
                        break;

                    case "4":
                        await EnrollmentMenuAsync(enrollmentService);
                        break;

                    case "5":
                        await ReportsMenuAsync(
                            departmentService,
                            studentService,
                            courseService);
                        break;

                    case "6":
                        await ViewAuditLogsAsync(auditLogService);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Pause();
            }
        }
    }

    // ============================================================
    // DEPARTMENT MENU
    // ============================================================

    private static async Task DepartmentMenuAsync(
        DepartmentService service)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("        DEPARTMENT MANAGEMENT");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Add Department");
            Console.WriteLine("2. List Departments");
            Console.WriteLine("3. Search Department");
            Console.WriteLine("4. Update Department");
            Console.WriteLine("5. Deactivate Department");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AddDepartmentAsync(service);
                        break;

                    case "2":
                        await ListDepartmentsAsync(service);
                        break;

                    case "3":
                        await SearchDepartmentAsync(service);
                        break;

                    case "4":
                        await UpdateDepartmentAsync(service);
                        break;

                    case "5":
                        await DeactivateDepartmentAsync(service);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Pause();
            }
        }
    }

    private static async Task AddDepartmentAsync(
        DepartmentService service)
    {
        Console.Write("\nDepartment Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Location: ");
        string location = Console.ReadLine() ?? "";

        Department department = new()
        {
            DepartmentName = name,
            Location = location
        };

        int id = await service.AddAsync(department);

        Console.WriteLine(
            $"\nDepartment added successfully. ID: {id}");

        Pause();
    }

    private static async Task ListDepartmentsAsync(
        DepartmentService service)
    {
        List<Department> departments =
            await service.GetAllAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("          DEPARTMENT LIST");
        Console.WriteLine("========================================");

        foreach (Department department in departments)
        {
            Console.WriteLine(
                $"ID: {department.DepartmentId} | " +
                $"Name: {department.DepartmentName} | " +
                $"Location: {department.Location} | " +
                $"Active: {department.IsActive}");
        }

        Pause();
    }

    private static async Task SearchDepartmentAsync(
        DepartmentService service)
    {
        Console.Write("\nEnter department name: ");
        string name = Console.ReadLine() ?? "";

        List<Department> departments =
            await service.SearchAsync(name);

        foreach (Department department in departments)
        {
            Console.WriteLine(
                $"ID: {department.DepartmentId} | " +
                $"Name: {department.DepartmentName} | " +
                $"Location: {department.Location} | " +
                $"Active: {department.IsActive}");
        }

        Pause();
    }

    private static async Task UpdateDepartmentAsync(
        DepartmentService service)
    {
        Console.Write("\nDepartment ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("New Department Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("New Location: ");
        string location = Console.ReadLine() ?? "";

        Department department = new()
        {
            DepartmentId = id,
            DepartmentName = name,
            Location = location
        };

        await service.UpdateAsync(department);

        Console.WriteLine(
            "\nDepartment updated successfully.");

        Pause();
    }

    private static async Task DeactivateDepartmentAsync(
        DepartmentService service)
    {
        Console.Write("\nDepartment ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        await service.DeactivateAsync(id);

        Console.WriteLine(
            "\nDepartment deactivated successfully.");

        Pause();
    }

    // ============================================================
    // STUDENT MENU
    // ============================================================

    private static async Task StudentMenuAsync(
        StudentService service)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("          STUDENT MANAGEMENT");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Register Student");
            Console.WriteLine("2. List Students");
            Console.WriteLine("3. Search Student by ID");
            Console.WriteLine("4. Search Student by Name");
            Console.WriteLine("5. Filter Students by Department");
            Console.WriteLine("6. Update Student");
            Console.WriteLine("7. Deactivate Student");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await RegisterStudentAsync(service);
                        break;

                    case "2":
                        await ListStudentsAsync(service);
                        break;

                    case "3":
                        await SearchStudentByIdAsync(service);
                        break;

                    case "4":
                        await SearchStudentByNameAsync(service);
                        break;

                    case "5":
                        await FilterStudentsByDepartmentAsync(service);
                        break;

                    case "6":
                        await UpdateStudentAsync(service);
                        break;

                    case "7":
                        await DeactivateStudentAsync(service);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Pause();
            }
        }
    }

    private static async Task RegisterStudentAsync(
        StudentService service)
    {
        Console.Write("\nName: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Age: ");
        int age = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Department ID: ");
        int departmentId =
            int.Parse(Console.ReadLine() ?? "0");

        Student student = new()
        {
            Name = name,
            Email = email,
            Age = age,
            DepartmentId = departmentId
        };

        int id = await service.RegisterAsync(student);

        Console.WriteLine(
            $"\nStudent registered successfully. ID: {id}");

        Pause();
    }

    private static async Task ListStudentsAsync(
        StudentService service)
    {
        List<Student> students =
            await service.GetAllAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("            STUDENT LIST");
        Console.WriteLine("========================================");

        foreach (Student student in students)
        {
            DisplayStudent(student);
        }

        Pause();
    }

    private static async Task SearchStudentByIdAsync(
        StudentService service)
    {
        Console.Write("\nStudent ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Student? student =
            await service.GetByIdAsync(id);

        if (student == null)
        {
            Console.WriteLine("\nStudent not found.");
        }
        else
        {
            DisplayStudent(student);
        }

        Pause();
    }

    private static async Task SearchStudentByNameAsync(
        StudentService service)
    {
        Console.Write("\nStudent name: ");
        string name = Console.ReadLine() ?? "";

        List<Student> students =
            await service.SearchByNameAsync(name);

        foreach (Student student in students)
        {
            DisplayStudent(student);
        }

        Pause();
    }

    private static async Task FilterStudentsByDepartmentAsync(
        StudentService service)
    {
        Console.Write("\nDepartment ID: ");
        int departmentId =
            int.Parse(Console.ReadLine() ?? "0");

        List<Student> students =
            await service.GetByDepartmentAsync(departmentId);

        foreach (Student student in students)
        {
            DisplayStudent(student);
        }

        Pause();
    }

    private static async Task UpdateStudentAsync(
        StudentService service)
    {
        Console.Write("\nStudent ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("New Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("New Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("New Age: ");
        int age = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("New Department ID: ");
        int departmentId =
            int.Parse(Console.ReadLine() ?? "0");

        Student student = new()
        {
            StudentId = id,
            Name = name,
            Email = email,
            Age = age,
            DepartmentId = departmentId
        };

        bool updated = await service.UpdateAsync(student);

        Console.WriteLine(
            updated
                ? "\nStudent updated successfully."
                : "\nStudent not found.");

        Pause();
    }

    private static async Task DeactivateStudentAsync(
        StudentService service)
    {
        Console.Write("\nStudent ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        bool deactivated =
            await service.DeactivateAsync(id);

        Console.WriteLine(
            deactivated
                ? "\nStudent deactivated successfully."
                : "\nStudent not found or already inactive.");

        Pause();
    }

    private static void DisplayStudent(Student student)
    {
        Console.WriteLine(
            $"ID: {student.StudentId} | " +
            $"Name: {student.Name} | " +
            $"Email: {student.Email} | " +
            $"Age: {student.Age} | " +
            $"Department ID: {student.DepartmentId} | " +
            $"Active: {student.IsActive}");
    }

    // ============================================================
    // COURSE MENU
    // ============================================================

    private static async Task CourseMenuAsync(
        CourseService service)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("           COURSE MANAGEMENT");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Add Course");
            Console.WriteLine("2. List Courses");
            Console.WriteLine("3. Search Course");
            Console.WriteLine("4. Filter Courses by Department");
            Console.WriteLine("5. Update Course");
            Console.WriteLine("6. Deactivate Course");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AddCourseAsync(service);
                        break;

                    case "2":
                        await ListCoursesAsync(service);
                        break;

                    case "3":
                        await SearchCourseAsync(service);
                        break;

                    case "4":
                        await FilterCoursesByDepartmentAsync(service);
                        break;

                    case "5":
                        await UpdateCourseAsync(service);
                        break;

                    case "6":
                        await DeactivateCourseAsync(service);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Pause();
            }
        }
    }

    private static async Task AddCourseAsync(
        CourseService service)
    {
        Console.Write("\nCourse Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Duration in Months: ");
        int duration = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Fee: ");
        decimal fee = decimal.Parse(Console.ReadLine() ?? "0");

        Console.Write("Department ID: ");
        int departmentId =
            int.Parse(Console.ReadLine() ?? "0");

        Course course = new()
        {
            CourseName = name,
            DurationMonths = duration,
            Fee = fee,
            DepartmentId = departmentId
        };

        int id = await service.AddAsync(course);

        Console.WriteLine(
            $"\nCourse added successfully. ID: {id}");

        Pause();
    }

    private static async Task ListCoursesAsync(
        CourseService service)
    {
        List<Course> courses =
            await service.GetAllAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("             COURSE LIST");
        Console.WriteLine("========================================");

        foreach (Course course in courses)
        {
            DisplayCourse(course);
        }

        Pause();
    }

    private static async Task SearchCourseAsync(
        CourseService service)
    {
        Console.Write("\nCourse name: ");
        string name = Console.ReadLine() ?? "";

        List<Course> courses =
            await service.SearchAsync(name);

        foreach (Course course in courses)
        {
            DisplayCourse(course);
        }

        Pause();
    }

    private static async Task FilterCoursesByDepartmentAsync(
        CourseService service)
    {
        Console.Write("\nDepartment ID: ");
        int departmentId =
            int.Parse(Console.ReadLine() ?? "0");

        List<Course> courses =
            await service.GetByDepartmentAsync(departmentId);

        foreach (Course course in courses)
        {
            DisplayCourse(course);
        }

        Pause();
    }

    private static async Task UpdateCourseAsync(
        CourseService service)
    {
        Console.Write("\nCourse ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("New Course Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("New Duration in Months: ");
        int duration = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("New Fee: ");
        decimal fee = decimal.Parse(Console.ReadLine() ?? "0");

        Console.Write("New Department ID: ");
        int departmentId =
            int.Parse(Console.ReadLine() ?? "0");

        Course course = new()
        {
            CourseId = id,
            CourseName = name,
            DurationMonths = duration,
            Fee = fee,
            DepartmentId = departmentId
        };

        bool updated = await service.UpdateAsync(course);

        Console.WriteLine(
            updated
                ? "\nCourse updated successfully."
                : "\nCourse not found.");

        Pause();
    }

    private static async Task DeactivateCourseAsync(
        CourseService service)
    {
        Console.Write("\nCourse ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        bool deactivated =
            await service.DeactivateAsync(id);

        Console.WriteLine(
            deactivated
                ? "\nCourse deactivated successfully."
                : "\nCourse not found or already inactive.");

        Pause();
    }

    private static void DisplayCourse(Course course)
    {
        Console.WriteLine(
            $"ID: {course.CourseId} | " +
            $"Name: {course.CourseName} | " +
            $"Duration: {course.DurationMonths} months | " +
            $"Fee: {course.Fee} | " +
            $"Department ID: {course.DepartmentId} | " +
            $"Active: {course.IsActive}");
    }

    // ============================================================
    // ENROLLMENT MENU
    // ============================================================

    private static async Task EnrollmentMenuAsync(
        EnrollmentService service)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("         ENROLLMENT MANAGEMENT");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Enroll Student in Course");
            Console.WriteLine("2. List Enrollments");
            Console.WriteLine("3. View Student's Courses");
            Console.WriteLine("4. View Course's Students");
            Console.WriteLine("5. Complete Enrollment");
            Console.WriteLine("6. Cancel Enrollment");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await EnrollStudentAsync(service);
                        break;

                    case "2":
                        await ListEnrollmentsAsync(service);
                        break;

                    case "3":
                        await ViewStudentCoursesAsync(service);
                        break;

                    case "4":
                        await ViewCourseStudentsAsync(service);
                        break;

                    case "5":
                        await CompleteEnrollmentAsync(service);
                        break;

                    case "6":
                        await CancelEnrollmentAsync(service);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Pause();
            }
        }
    }

    private static async Task EnrollStudentAsync(
        EnrollmentService service)
    {
        Console.Write("\nStudent ID: ");
        int studentId =
            int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Course ID: ");
        int courseId =
            int.Parse(Console.ReadLine() ?? "0");

        int enrollmentId =
            await service.EnrollAsync(studentId, courseId);

        Console.WriteLine(
            $"\nEnrollment successful. Enrollment ID: {enrollmentId}");

        Pause();
    }

    private static async Task ListEnrollmentsAsync(
        EnrollmentService service)
    {
        List<Enrollment> enrollments =
            await service.GetAllAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("          ENROLLMENT LIST");
        Console.WriteLine("========================================");

        foreach (Enrollment enrollment in enrollments)
        {
            DisplayEnrollment(enrollment);
        }

        Pause();
    }

    private static async Task ViewStudentCoursesAsync(
        EnrollmentService service)
    {
        Console.Write("\nStudent ID: ");
        int studentId =
            int.Parse(Console.ReadLine() ?? "0");

        List<Enrollment> enrollments =
            await service.GetByStudentIdAsync(studentId);

        Console.WriteLine("\nStudent Enrollments:");

        foreach (Enrollment enrollment in enrollments)
        {
            DisplayEnrollment(enrollment);
        }

        Pause();
    }

    private static async Task ViewCourseStudentsAsync(
        EnrollmentService service)
    {
        Console.Write("\nCourse ID: ");
        int courseId =
            int.Parse(Console.ReadLine() ?? "0");

        List<Enrollment> enrollments =
            await service.GetByCourseIdAsync(courseId);

        Console.WriteLine("\nCourse Enrollments:");

        foreach (Enrollment enrollment in enrollments)
        {
            DisplayEnrollment(enrollment);
        }

        Pause();
    }

    private static async Task CompleteEnrollmentAsync(
        EnrollmentService service)
    {
        Console.Write("\nEnrollment ID: ");
        int enrollmentId =
            int.Parse(Console.ReadLine() ?? "0");

        await service.CompleteAsync(enrollmentId);

        Console.WriteLine(
            "\nEnrollment completed successfully.");

        Pause();
    }

    private static async Task CancelEnrollmentAsync(
        EnrollmentService service)
    {
        Console.Write("\nEnrollment ID: ");
        int enrollmentId =
            int.Parse(Console.ReadLine() ?? "0");

        await service.CancelAsync(enrollmentId);

        Console.WriteLine(
            "\nEnrollment cancelled successfully.");

        Pause();
    }

    private static void DisplayEnrollment(
        Enrollment enrollment)
    {
        Console.WriteLine(
            $"ID: {enrollment.EnrollmentId} | " +
            $"Student ID: {enrollment.StudentId} | " +
            $"Course ID: {enrollment.CourseId} | " +
            $"Enrolled At: {enrollment.EnrolledAt} | " +
            $"Status: {enrollment.Status}");
    }

    // ============================================================
    // REPORTS MENU
    // ============================================================

    private static async Task ReportsMenuAsync(
        DepartmentService departmentService,
        StudentService studentService,
        CourseService courseService)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("                REPORTS");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Student Count by Department");
            Console.WriteLine("2. Enrollment Count per Course");
            Console.WriteLine("3. Students Without Enrollments");
            Console.WriteLine("4. Courses Without Enrollments");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await StudentCountByDepartmentAsync(
                            departmentService);
                        break;

                    case "2":
                        await EnrollmentCountPerCourseAsync(
                            courseService);
                        break;

                    case "3":
                        await StudentsWithoutEnrollmentsAsync(
                            studentService);
                        break;

                    case "4":
                        await CoursesWithoutEnrollmentsAsync(
                            courseService);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Pause();
            }
        }
    }

    private static async Task StudentCountByDepartmentAsync(
        DepartmentService service)
    {
        List<(int DepartmentId, string DepartmentName, int StudentCount)>
            results =
                await service.GetStudentCountByDepartmentAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("      STUDENT COUNT BY DEPARTMENT");
        Console.WriteLine("========================================");

        if (results.Count == 0)
        {
            Console.WriteLine("\nNo departments found.");
        }
        else
        {
            foreach (var result in results)
            {
                Console.WriteLine(
                    $"ID: {result.DepartmentId} | " +
                    $"Department: {result.DepartmentName} | " +
                    $"Students: {result.StudentCount}");
            }
        }

        Pause();
    }

    private static async Task EnrollmentCountPerCourseAsync(
        CourseService service)
    {
        List<(int CourseId, string CourseName, int EnrollmentCount)>
            results =
                await service.GetEnrollmentCountPerCourseAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("       ENROLLMENT COUNT PER COURSE");
        Console.WriteLine("========================================");

        if (results.Count == 0)
        {
            Console.WriteLine("\nNo courses found.");
        }
        else
        {
            foreach (var result in results)
            {
                Console.WriteLine(
                    $"ID: {result.CourseId} | " +
                    $"Course: {result.CourseName} | " +
                    $"Enrollments: {result.EnrollmentCount}");
            }
        }

        Pause();
    }

    private static async Task StudentsWithoutEnrollmentsAsync(
        StudentService service)
    {
        List<(int StudentId, string Name)> results =
            await service.GetStudentsWithoutEnrollmentsAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("      STUDENTS WITHOUT ENROLLMENTS");
        Console.WriteLine("========================================");

        if (results.Count == 0)
        {
            Console.WriteLine(
                "\nAll students have enrollments.");
        }
        else
        {
            foreach (var result in results)
            {
                Console.WriteLine(
                    $"Student ID: {result.StudentId} | " +
                    $"Name: {result.Name}");
            }
        }

        Pause();
    }

    private static async Task CoursesWithoutEnrollmentsAsync(
        CourseService service)
    {
        List<(int CourseId, string CourseName)> results =
            await service.GetCoursesWithoutEnrollmentsAsync();

        Console.WriteLine("\n========================================");
        Console.WriteLine("       COURSES WITHOUT ENROLLMENTS");
        Console.WriteLine("========================================");

        if (results.Count == 0)
        {
            Console.WriteLine(
                "\nAll courses have enrollments.");
        }
        else
        {
            foreach (var result in results)
            {
                Console.WriteLine(
                    $"Course ID: {result.CourseId} | " +
                    $"Course: {result.CourseName}");
            }
        }

        Pause();
    }

    // ============================================================
    // AUDIT LOGS
    // ============================================================

    private static async Task ViewAuditLogsAsync(
        AuditLogService service)
    {
        List<AuditLog> logs =
            await service.GetRecentAsync(10);

        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("          RECENT AUDIT LOGS");
        Console.WriteLine("========================================");

        if (logs.Count == 0)
        {
            Console.WriteLine("\nNo audit logs found.");
        }
        else
        {
            foreach (AuditLog log in logs)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Audit ID    : {log.AuditId}");
                Console.WriteLine(
                    $"Entity      : {log.EntityName}");
                Console.WriteLine(
                    $"Entity ID   : {log.EntityId}");
                Console.WriteLine(
                    $"Action      : {log.Action}");
                Console.WriteLine(
                    $"Details     : {log.Details}");
                Console.WriteLine(
                    $"Created At  : {log.CreatedAt}");
                Console.WriteLine("----------------------------------------");
            }
        }

        Pause();
    }

    // ============================================================
    // COMMON
    // ============================================================

    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
}
