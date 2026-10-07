using DapperApp;
DapperRepository repository =
    new DapperRepository();


// ==================================================
// ADD STUDENTS
// ==================================================

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


// ==================================================
// ADD COURSES
// ==================================================

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


// ==================================================
// TEST 1
// NORMAL ENROLLMENT
// ==================================================

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


// ==================================================
// TEST 2
// DUPLICATE ENROLLMENT
// ==================================================

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


// ==================================================
// TEST 3
// MAXIMUM 3 COURSES
// ==================================================

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


// ==================================================
// TEST 4
// COURSE CAPACITY = 8
// ==================================================

Console.WriteLine(
    "\n--- Course Capacity ---"
);


// Ahmad is already in course 1.
// Add students 2 -> 8.

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


// Student 9 would be number 9.
// This should fail.

await repository.AddEnrollment(
    new Enrollment
    {
        StudentId = 9,
        CourseId = 1
    }
);


// ==================================================
// TEST 5
// SHOW AHMAD COURSES
// ==================================================

Console.WriteLine(
    "\n--- Ahmad Courses ---"
);

await repository.GetEnrollments(
    "Ahmad",
    null,
    true
);


// ==================================================
// TEST 6
// DELETE STUDENT
// ==================================================

Console.WriteLine(
    "\n--- Delete Ahmad ---"
);

await repository.DeleteStudent(1);


// ==================================================
// TEST 7
// TRY INACTIVE STUDENT
// ==================================================

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


// ==================================================
// TEST 8
// DELETE COURSE
// ==================================================

Console.WriteLine(
    "\n--- Delete Database Systems ---"
);

await repository.DeleteCourse(1);


// ==================================================
// TEST 9
// TRY INACTIVE COURSE
// ==================================================

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


// ==================================================
// TEST 10
// SHOW ACTIVE ENROLLMENTS
// ==================================================

Console.WriteLine(
    "\n--- Active Enrollments ---"
);

await repository.GetEnrollments(
    null,
    null,
    true
);