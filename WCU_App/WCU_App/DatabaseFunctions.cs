using Plugin.LocalNotification;
using SQLite;
using System;
using System.Collections.Generic;

namespace WCU_App
{

    public static class AppFunctions
    {

        public static void createTable()
        {
            var appDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AppData.db");
            var db = new SQLiteConnection(appDatabase);
            db.CreateTable<Term>();
            db.CreateTable<Course>();
            db.CreateTable<Exam>();
            db.CreateTable<Instructor>();
            db.CreateTable<Note>();
        }
        public static void addNewTerm()
        {
            var db = new SQLiteConnection(MainPage.appDatabase);
            var resp = db.Query<Term>($"SELECT * FROM Terms ORDER BY termID DESC LIMIT 1");
            Term nt = resp.First();
            string termName = "Term " + (nt.termID + 1).ToString();
            Term rt = new Term(termName, DateTime.Now, DateTime.Now.AddDays(60));
            db.Insert(rt);
            MainPage.sync_db();
        }

        public static void addNewCourse(int termID)
        {
            var db = new SQLiteConnection(MainPage.appDatabase);
            Exam PA = new Exam(1, "PerformanceAssessment23", DateTime.Now, DateTime.Now.AddMonths(3), "Enter details about exam here:", 1);
            Exam OA = new Exam(0, "ObjectiveAssessment23", DateTime.Now, DateTime.Now.AddMonths(3), "Enter details about exam here:", 1);
            Course course1 = new Course(termID, 1, "NewCourse23", DateTime.Now, DateTime.Now.AddMonths(4), "Plan to Take", "Enter Course Details Here:", 1, 2);
            addCourse(db, course1);
            List<Course> resp = db.Query<Course>($"SELECT courseID FROM Courses WHERE courseName='NewCourse23'");
            PA.courseID = resp[0].courseID;
            OA.courseID = resp[0].courseID;
            addExam(db, PA);
            addExam(db, OA);
            List<Exam> resp2 = db.Query<Exam>($"SELECT examID FROM Exams WHERE courseID='{resp[0].courseID.ToString()}'");
            foreach (Exam a in resp2)
            {
                if (a.type == 1)
                {
                    course1.pa = a.examID;
                }
                else
                {
                    course1.oa = a.examID;
                }
            }
            db.Update(course1);
            MainPage.sync_db();


        }

        public static void addTerm(SQLiteConnection db, Term term)
        {
            db.Insert(term);
        }
        public static void updateTerm(SQLiteConnection db, Term term)
        {
            db.Update(term);
        }
        public static void addCourse(SQLiteConnection db, Course course)
        {
            db.Insert(course);
        }
        public static void updateCourse(SQLiteConnection db, Course course)
        {
            db.Update(course);
        }

        public static void deleteCourse(Course course)
        {
            var db = new SQLiteConnection(MainPage.appDatabase);
            db.Delete(course);
        }
        public static void addExam(SQLiteConnection db, Exam exam)
        {
            db.Insert(exam);
        }
        public static void updateExam(SQLiteConnection db, Exam exam)
        {
            db.Update(exam);
        }
        public static void addInstructor(SQLiteConnection db, Instructor instructor)
        {
            db.Insert(instructor);
        }
        public static void updateInstructor(SQLiteConnection db, Instructor instructor)
        {
            db.Update(instructor);
        }
        public static void addNote(SQLiteConnection db, Note note)
        {
            db.Insert(note);
        }
        public static void updateNote(SQLiteConnection db, Note note)
        {
            db.Update(note);
        }
        public static void deleteNote(Note note)
        {
            var db = new SQLiteConnection(MainPage.appDatabase);
            db.Delete(note);
        }
        public static List<Course> getCourses(SQLiteConnection db, Term term)
        {
            var courses = db.Query<Course>($"SELECT * FROM Courses WHERE termID={term.termID}");
            List<Course> result = new List<Course>();
            foreach (Course course in courses)
            {
                result.Add(course);
            }
            return result;
        }
    }
}
