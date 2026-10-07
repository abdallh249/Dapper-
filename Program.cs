using DapperApp;
DapperRepository repository =
    new DapperRepository();




for (int i = 1; i <= 10; i++)
{
    await repository.AddStudent(
        new Student
        {
            Id = i,
            Name = i == 1
                ? "Ahmad"
                : "Student" + i,

            Email = "student" + i + "@gmail.com",

            IsActive = true
        }
    );
}




await repository.AddCourse(
    new Course
    {
        Id = 1,
        Name = "Database Systems",
        IsActive = true
    }
);

await repository.AddCourse(
    new Course
    {
        Id = 2,
        Name = "C#",
        IsActive = true
    }
);

await repository.AddCourse(
    new Course
    {
        Id = 3,
        Name = "Networks",
        IsActive = true
    }
);

await repository.AddCourse(
    new Course
    {
        Id = 4,
        Name = "Algorithms",
        IsActive = true
    }
);



Console.WriteLine(
    "\n--- Normal Enrollment ---"
);

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 1,
        CourseId = 1
    }
);




Console.WriteLine(
    "\n--- Duplicate Enrollment ---"
);

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 1,
        CourseId = 1
    }
);




Console.WriteLine(
    "\n--- Student Course Limit ---"
);

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 1,
        CourseId = 2
    }
);

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 1,
        CourseId = 3
    }
);


// Ahmad already has 3 courses.
// This should fail.

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 1,
        CourseId = 4
    }
);




Console.WriteLine(
    "\n--- Course Capacity ---"
);




for (int i = 2; i <= 8; i++)
{
    await repository.AddEnrollment(
        new Enrollment
        {
            StudentId = i,
            CourseId = 1
        }
    );
}




await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 9,
        CourseId = 1
    }
);




Console.WriteLine(
    "\n--- Ahmad Courses ---"
);

await repository.GetEnrollments(
    "Ahmad",
    null,
    true
);




Console.WriteLine(
    "\n--- Delete Ahmad ---"
);

await repository.DeleteStudent(1);



Console.WriteLine(
    "\n--- Inactive Student Test ---"
);

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 1,
        CourseId = 4
    }
);




Console.WriteLine(
    "\n--- Delete Database Systems ---"
);

await repository.DeleteCourse(1);




Console.WriteLine(
    "\n--- Inactive Course Test ---"
);

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 9,
        CourseId = 1
    }
);



Console.WriteLine(
    "\n--- Active Enrollments ---"
);

await repository.GetEnrollments(
    null,
    null,
    true
);
