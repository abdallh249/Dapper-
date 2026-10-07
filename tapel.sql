CREATE TABLE Students
(
    id          INT PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,
    email       VARCHAR(150) NOT NULL UNIQUE,
    isactive    BOOLEAN NOT NULL DEFAULT TRUE
);


CREATE TABLE Courses
(
    id          INT PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,
    isactive    BOOLEAN NOT NULL DEFAULT TRUE
);


CREATE TABLE Enrollments
(
    student_id  INT NOT NULL,
    course_id   INT NOT NULL,

    PRIMARY KEY (student_id, course_id),

    CONSTRAINT fk_enrollment_student
        FOREIGN KEY (student_id)
        REFERENCES Students(id),

    CONSTRAINT fk_enrollment_course
        FOREIGN KEY (course_id)
        REFERENCES Courses(id)
);