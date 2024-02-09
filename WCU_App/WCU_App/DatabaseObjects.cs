using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;



namespace WCU_App
{

    [SQLite.Table("Terms")]
    public class Term
    {
        public Term() { }
        public Term(string termName, DateTime start, DateTime end)
        {
            this.termName = termName;
            this.start = start;
            this.end = end;
        }
        [PrimaryKey, AutoIncrement]
        public int termID { get; set; }
        public string termName { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }


    [SQLite.Table("Courses")]
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int courseID { get; set; }
        public int termID { get; set; }
        public Course() { }
        public Course(int termID, int instructorID, string courseName, DateTime start, DateTime end, string status, string courseDetails, int pa, int oa)
        {
            this.termID = termID;
            this.instructorID = instructorID;
            this.courseName = courseName;
            this.start = start;
            this.end = end;
            this.status = status;
            this.courseDetails = courseDetails;
            this.pa = pa;
            this.oa = oa;
        }

        public int instructorID { get; set; }
        public string courseName { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string status { get; set; }
        public string courseDetails { get; set; }
        public int pa { get; set; }
        public int oa { get; set; }
        public int startNotification { get; set; }
        public int endNotification { get; set; }

    }

    

    [SQLite.Table("Instructors")]
    public class Instructor
    {
        public Instructor() { }
        public Instructor(string instructorName, string instructorPhone, string instructorEmail)
        {
            this.instructorName = instructorName;
            this.instructorPhone = instructorPhone;
            this.instructorEmail = instructorEmail;
        }
        [PrimaryKey, AutoIncrement]
        public int instructorID { get; set; }
        public string instructorName { get; set; }
        public string instructorPhone { get; set; }
        public string instructorEmail { get; set; }
    }

    [SQLite.Table("Notes")]
    public class Note
    {
        public Note() { }
        public Note(int courseID, string content)
        {
            this.courseID = courseID;
            this.content = content;
        }
        [PrimaryKey, AutoIncrement]
        public int noteID { get; set; }
        public int courseID { get; set; }
        public string content { get; set; }
    }

    [SQLite.Table("Exams")]
    public class Exam
    {
        public Exam() { }
        public Exam(int type, string examName, DateTime start, DateTime end, string examDetails, int courseID)
        {
            this.type = type;
            this.examName = examName;
            this.start = start;
            this.end = end;
            this.examDetails = examDetails;
            this.courseID = courseID;
            dueDate = end;
        }
        [PrimaryKey, AutoIncrement]
        public int examID { get; set; }
        public int type { get; set; }
        public string examName { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string examDetails { get; set; }
        public int startNotif { get; set; }
        public int endNotif { get; set; }
        public int courseID { get; set; }
        public DateTime dueDate { get; set; }
    }
}
