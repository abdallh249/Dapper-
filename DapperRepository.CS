using Dapper;
using DotNetEnv;
using Npgsql;

namespace DapperApp;
public sealed class DapperRepository
{
    private readonly string connectionString;

    public DapperRepository()
    {
        Env.Load();

        connectionString =
            Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
            ?? throw new Exception("Database connection string not found.");
    }


    // ----------------------------
    // ADD STUDENT
    // ----------------------------

    public async Task AddStudent(Student student)
    {
        using var connection =
            new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync(
            """
            INSERT INTO Students
                (id, name, email, isactive)
            VALUES
                (@Id, @Name, @Email, @IsActive)
            """,
            student
        );

        Console.WriteLine($"Student added: {student.Name}");
    }


    // ----------------------------
    // ADD COURSE
    // ----------------------------

    public async Task AddCourse(Course course)
    {
        using var connection =
            new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync(
            """
            INSERT INTO Courses
                (id, name, isactive)
            VALUES
                (@Id, @Name, @IsActive)
            """,
            course
        );

        Console.WriteLine($"Course added: {course.Name}");
    }


    // ----------------------------
    // ADD ENROLLMENT
    // ----------------------------

    public async Task AddEnrollment(
        Enrollment enrollment)
    {
        using var connection =
            new NpgsqlConnection(connectionString);


        // Check active student
        bool studentExists =
            await connection.ExecuteScalarAsync<bool>(
                """
                SELECT EXISTS (
                    SELECT 1
                    FROM Students
                    WHERE id = @StudentId
                    AND isactive = TRUE
                )
                """,
                enrollment
            );

        if (!studentExists)
        {
            Console.WriteLine(
                "Student does not exist or is inactive."
            );

            return;
        }


        // Check active course
        bool courseExists =
            await connection.ExecuteScalarAsync<bool>(
                """
                SELECT EXISTS (
                    SELECT 1
                    FROM Courses
                    WHERE id = @CourseId
                    AND isactive = TRUE
                )
                """,
                enrollment
            );

        if (!courseExists)
        {
            Console.WriteLine(
                "Course does not exist or is inactive."
            );

            return;
        }


        // Check duplicate
        bool duplicate =
            await connection.ExecuteScalarAsync<bool>(
                """
                SELECT EXISTS (
                    SELECT 1
                    FROM Enrollments
                    WHERE student_id = @StudentId
                    AND course_id = @CourseId
                )
                """,
                enrollment
            );

        if (duplicate)
        {
            Console.WriteLine(
                "Student is already enrolled in this course."
            );

            return;
        }


        // Student maximum = 3 active courses
        long studentCourses =
            await connection.ExecuteScalarAsync<long>(
                """
                SELECT COUNT(*)
                FROM Enrollments e

                INNER JOIN Courses c
                    ON e.course_id = c.id

                WHERE e.student_id = @StudentId
                AND c.isactive = TRUE
                """,
                enrollment
            );

        if (studentCourses >= 3)
        {
            Console.WriteLine(
                "Student cannot enroll in more than 3 courses."
            );

            return;
        }


        // Course maximum = 8 students
        long courseStudents =
            await connection.ExecuteScalarAsync<long>(
                """
                SELECT COUNT(*)
                FROM Enrollments
                WHERE course_id = @CourseId
                """,
                enrollment
            );

        if (courseStudents >= 8)
        {
            Console.WriteLine(
                "Course cannot have more than 8 students."
            );

            return;
        }


        // Add enrollment
        await connection.ExecuteAsync(
            """
            INSERT INTO Enrollments
                (student_id, course_id)
            VALUES
                (@StudentId, @CourseId)
            """,
            enrollment
        );

        Console.WriteLine("Enrollment added.");
    }


    // ----------------------------
    // DELETE STUDENT
    // ----------------------------

    public async Task DeleteStudent(int studentId)
    {
        using var connection =
            new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync(
            """
            UPDATE Students
            SET isactive = FALSE
            WHERE id = @studentId;

            DELETE FROM Enrollments
            WHERE student_id = @studentId;
            """,
            new { studentId }
        );

        Console.WriteLine("Student deleted.");
    }


    // ----------------------------
    // DELETE COURSE
    // ----------------------------

    public async Task DeleteCourse(int courseId)
    {
        using var connection =
            new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync(
            """
            UPDATE Courses
            SET isactive = FALSE
            WHERE id = @courseId;

            DELETE FROM Enrollments
            WHERE course_id = @courseId;
            """,
            new { courseId }
        );

        Console.WriteLine("Course deleted.");
    }


    // ----------------------------
    // GET ENROLLMENTS
    // ----------------------------

    public async Task GetEnrollments(
        string? studentName,
        string? courseName,
        bool onlyActive)
    {
        using var connection =
            new NpgsqlConnection(connectionString);

        var result = await connection.QueryAsync(
            """
            SELECT
                Students.name AS "StudentName",
                Courses.name AS "CourseName"

            FROM Enrollments

            INNER JOIN Students
                ON Enrollments.student_id = Students.id

            INNER JOIN Courses
                ON Enrollments.course_id = Courses.id

            WHERE
                (
                    @studentName IS NULL
                    OR Students.name = @studentName
                )

                AND
                (
                    @courseName IS NULL
                    OR Courses.name = @courseName
                )

                AND
                (
                    @onlyActive = FALSE
                    OR
                    (
                        Students.isactive = TRUE
                        AND Courses.isactive = TRUE
                    )
                )
            """,
            new
            {
                studentName,
                courseName,
                onlyActive
            }
        );

        foreach (var enrollment in result)
        {
            Console.WriteLine(
                $"{enrollment.StudentName} | " +
                $"{enrollment.CourseName}"
            );
        }
    }
}