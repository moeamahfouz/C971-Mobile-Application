using Plugin.LocalNotification;
using SQLite;
using System.Collections;
using System.Windows.Input;

namespace WCU_App
{
    public partial class MainPage : ContentPage
    {
        public static List<Term> terms = new List<Term>();
        public static Dictionary<Term, List<Course>> courses = new Dictionary<Term, List<Course>>();
        public static Dictionary<int, Course> courseList = new Dictionary<int, Course>();
        public static Dictionary<int, Exam> exams = new Dictionary<int, Exam>();
        public static Dictionary<int, Instructor> instructors = new Dictionary<int, Instructor>();
        public static Dictionary<int, Note> notes = new Dictionary<int, Note>();
        public static Term termSelected;
        public static List<String> statusValues = new List<String>();
        public static List<int> notificationValues = new List<int>();
        public static IList<NotificationRequest> notificationRequestsStatic = new List<NotificationRequest>();
        public static string appDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AppData.db");
        public MainPage()
        {
            InitializeComponent();
            loadSavedData();
            loadDefaultData();  //Move this function above the loadSavedData function to reset app to default values
            loadUI(1);
            statusValues.Add("Scheduled");
            statusValues.Add("In Progress");
            statusValues.Add("Passed");
            statusValues.Add("Dropped");
            notificationValues.Add(0);
            notificationValues.Add(1);
            notificationValues.Add(2);
            notificationValues.Add(3);
            notificationValues.Add(4);
            notificationValues.Add(5);
            notificationValues.Add(6);
            notificationValues.Add(7);
            notificationValues.Add(14);
        }

        public async void notificationHandler()
        {

            notificationRequestsStatic = await LocalNotificationCenter.Current.GetPendingNotificationList();
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }


            List<NotificationRequest> requests = new List<NotificationRequest>();
            List<int> cancelledRequests = new List<int>();
            DateTime tmpdt = DateTime.Now;
            foreach (List<Course> listCourse in courses.Values)
            {
                foreach (Course course in listCourse)
                {
                    if (course.startNotification == 0)
                    {
                        cancelledRequests.Add(course.courseID + 1000);
                    }
                    else
                    {
                        NotificationRequest request = new NotificationRequest()
                        {
                            NotificationId = course.courseID + 1000,
                            Title = course.courseName + " Reminder",
                            Description = course.courseName + " is starting soon",
                            Schedule = new NotificationRequestSchedule()
                            {
                                NotifyTime = course.start.AddDays(-course.startNotification).AddHours(tmpdt.Hour).AddMinutes(tmpdt.Minute + 1),
                                RepeatType = NotificationRepeat.Daily
                            }
                        };
                        requests.Add(request);
                    }
                    if (course.endNotification == 0)
                    {
                        cancelledRequests.Add(course.courseID + 2000);
                    }
                    else
                    {
                        NotificationRequest request2 = new NotificationRequest()
                        {
                            NotificationId = course.courseID + 2000,
                            Title = course.courseName + " Reminder",
                            Description = course.courseName + " will be ending soon",
                            Schedule = new NotificationRequestSchedule()
                            {
                                NotifyTime = course.end.AddDays(-course.endNotification).AddHours(tmpdt.Hour).AddMinutes(tmpdt.Minute + 1),
                                RepeatType = NotificationRepeat.Daily
                            }
                        };
                        requests.Add(request2);
                    }

                }
            }

            foreach (Exam exam in exams.Values)
            {
                if (exam.startNotif == 0)
                {
                    cancelledRequests.Add(exam.examID + 3000);
                }
                else
                {
                    NotificationRequest request = new NotificationRequest()
                    {
                        NotificationId = exam.examID + 3000,
                        Title = exam.examName + " Alert",
                        Description = exam.examName + " will be opening soon",
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = exam.start.AddDays(-exam.startNotif).AddHours(tmpdt.Hour).AddMinutes(tmpdt.Minute + 1),
                            RepeatType = NotificationRepeat.Daily
                        }
                    };
                    requests.Add(request);
                }

                if (exam.startNotif == 0)
                {
                    cancelledRequests.Add(exam.examID + 4000);
                }
                else
                {
                    NotificationRequest request2 = new NotificationRequest()
                    {
                        NotificationId = exam.examID + 4000,
                        Title = exam.examName + " Alert",
                        Description = exam.examName + " will be closing soon",
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = exam.end.AddDays(-exam.endNotif).AddHours(tmpdt.Hour).AddMinutes(tmpdt.Minute + 1),
                            RepeatType = NotificationRepeat.Daily
                        }
                    };
                    requests.Add(request2);
                }
            }

            foreach (int i in cancelledRequests)
            {
                foreach (NotificationRequest notfi in notificationRequestsStatic)
                {
                    if (notfi.NotificationId == i)
                    {
                        notfi.Cancel();
                    }
                }
            }
            foreach (NotificationRequest request in requests)
            {
                await LocalNotificationCenter.Current.Show(request);

            }
        }

        protected override void OnAppearing()
        {
            loadUI(termSelected.termID);
            notificationHandler();
        }

        public void loadDefaultData()
        {

            File.Delete(appDatabase);
            AppFunctions.createTable();
            var db = new SQLiteConnection(appDatabase);
            Term term1 = new Term("Fall Term", DateTime.Now, DateTime.Now.AddMonths(6));
            Term term2 = new Term("Winter Term", DateTime.Now.AddMonths(6), DateTime.Now.AddMonths(12));
            AppFunctions.addTerm(db, term1);
            AppFunctions.addTerm(db, term2);
            Course course1 = new Course(1, 1, "Geology", DateTime.Now, DateTime.Now.AddMonths(1), "In Progress", "An introduction to rocks", 1, 2);
            Course course2 = new Course(1, 1, "Advanced Geology", DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(2), "Scheduled", "A deeper dive into rocks", 1, 1);
            Course course3 = new Course(1, 1, "Applied Geology", DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(3), "Scheduled", "Hands on with rocks", 1, 1);
            Course course4 = new Course(1, 1, "History of Geology", DateTime.Now.AddMonths(3), DateTime.Now.AddMonths(4), "Scheduled", "Rocks in the past", 1, 1);
            Course course5 = new Course(1, 1, "Statistical Geology", DateTime.Now.AddMonths(4), DateTime.Now.AddMonths(5), "Scheduled", "The chances of rocks", 1, 1);
            Course course6 = new Course(1, 1, "Geology 2", DateTime.Now.AddMonths(5), DateTime.Now.AddMonths(6), "Scheduled", "Return of the rocks", 1, 1);
            AppFunctions.addCourse(db, course1);
            AppFunctions.addCourse(db, course2);
            AppFunctions.addCourse(db, course3);
            AppFunctions.addCourse(db, course4);
            AppFunctions.addCourse(db, course5);
            AppFunctions.addCourse(db, course6);
            course1 = new Course(2, 1, "Physics in Geology", DateTime.Now.AddMonths(6), DateTime.Now.AddMonths(7), "Scheduled", "What happens if you throw a rock really hard?", 1, 2);
            course2 = new Course(2, 1, "Criminal Geology", DateTime.Now.AddMonths(7), DateTime.Now.AddMonths(8), "Scheduled", "When rocks fall into the wrong hands", 1, 2);
            course3 = new Course(2, 1, "Ethical Geology", DateTime.Now.AddMonths(8), DateTime.Now.AddMonths(9), "Scheduled", "Discussing right from rock", 1, 2);
            course4 = new Course(2, 1, "Social Geology", DateTime.Now.AddMonths(9), DateTime.Now.AddMonths(10), "Scheduled", "All rocks are created equal", 1, 2);
            course5 = new Course(2, 1, "Theoretical Geology", DateTime.Now.AddMonths(10), DateTime.Now.AddMonths(11), "Scheduled", "Rocks?", 1, 2);
            course6 = new Course(2, 1, "English", DateTime.Now.AddMonths(11), DateTime.Now.AddMonths(12), "Scheduled", "You'll learn how to spell", 1, 2);
            AppFunctions.addCourse(db, course1);
            AppFunctions.addCourse(db, course2);
            AppFunctions.addCourse(db, course3);
            AppFunctions.addCourse(db, course4);
            AppFunctions.addCourse(db, course5);
            AppFunctions.addCourse(db, course6);
            Exam PA = new Exam(1, "Working with Rocks", DateTime.Now, DateTime.Now.AddMonths(3), "It's time to handle some rocks", 1);
            Exam OA = new Exam(0, "A Rock Exam", DateTime.Now, DateTime.Now.AddMonths(3), "Do you truly know about rocks?", 1);
            AppFunctions.addExam(db, PA);
            AppFunctions.addExam(db, OA);
            Instructor instructor = new Instructor("Anika Patel", "555-123-4567", "anika.patel@strimeuniversity.edu");
            AppFunctions.addInstructor(db, instructor);
            Note note1 = new Note(1, "Pebbles are small");
            AppFunctions.addNote(db, note1);
            note1 = new Note(1, "Boulders are big");
            AppFunctions.addNote(db, note1);
        }

        private void loadSavedData()
        {
            var db = new SQLiteConnection(appDatabase);

            var tmpterm = db.Query<Term>("SELECT * FROM Terms");
            foreach (Term term in tmpterm)
            {
                terms.Add(term);
            }

            foreach (Term term in terms)
            {
                var tmpcourses = db.Query<Course>($"SELECT * FROM Courses WHERE termID={term.termID}");
                List<Course> CourseList = new List<Course>();
                foreach (Course course in tmpcourses)
                {
                    CourseList.Add(course);
                    courseList.Add(course.courseID, course);
                }
                courses.Add(term, CourseList);
            }
            var tmpExam = db.Query<Exam>("SELECT * FROM Exams");
            foreach (Exam exam in tmpExam)
            {
                exams.Add(exam.examID, exam);
            }
            var tmpInstructor = db.Query<Instructor>("SELECT * FROM Instructors");
            foreach (Instructor instructor in tmpInstructor)
            {
                instructors.Add(instructor.instructorID, instructor);
            }
            var tmpNote = db.Query<Note>("SELECT * FROM Notes");
            foreach (Note note in tmpNote)
            {
                notes.Add(note.noteID, note);
            }
        }

        public static void sync_db()
        {
            terms = new List<Term>();
            courses = new Dictionary<Term, List<Course>>();
            courseList = new Dictionary<int, Course>();
            exams = new Dictionary<int, Exam>();
            instructors = new Dictionary<int, Instructor>();
            notes = new Dictionary<int, Note>();
            var db = new SQLiteConnection(appDatabase);
            var tmpterm = db.Query<Term>("SELECT * FROM Terms");
            foreach (Term term in tmpterm)
            {
                terms.Add(term);
            }

            foreach (Term term in terms)
            {
                var tmpcourses = db.Query<Course>($"SELECT * FROM Courses WHERE termID={term.termID}");
                List<Course> CourseList = new List<Course>();
                foreach (Course course in tmpcourses)
                {
                    CourseList.Add(course);
                    courseList.Add(course.courseID, course);
                }
                courses.Add(term, CourseList);
            }
            var tmpExam = db.Query<Exam>("SELECT * FROM Exams");
            foreach (Exam exam in tmpExam)
            {
                exams.Add(exam.examID, exam);
            }
            var tmpInstructor = db.Query<Instructor>("SELECT * FROM Instructors");
            foreach (Instructor instructor in tmpInstructor)
            {
                instructors.Add(instructor.instructorID, instructor);
            }
            var tmpNote = db.Query<Note>("SELECT * FROM Notes");
            foreach (Note note in tmpNote)
            {
                notes.Add(note.noteID, note);
            }

        }

        public void loadUI(int term)
        {

            termStack.Children.Clear();
            courseStack.Children.Clear();
            termSelected = terms[term - 1];


            foreach (Term tmpterm in terms)
            {
                Button button = new Button
                {
                    Text = tmpterm.termName,
                    Padding = 8,
                    BackgroundColor = Microsoft.Maui.Graphics.Colors.MidnightBlue,
                    TextColor = Microsoft.Maui.Graphics.Colors.White,
                    CornerRadius = 3,
                };
                button.Clicked += void (sender, args) => loadUI(tmpterm.termID);
                termStack.Children.Add(button);
            }

            Button buttonTermAdd = new Button()
            {
                Text = "Add Term",
                Padding = 8,
                BackgroundColor = Microsoft.Maui.Graphics.Colors.LightSkyBlue,
                TextColor = Microsoft.Maui.Graphics.Colors.Black,
                CornerRadius = 3,
            };
            buttonTermAdd.Clicked += void (sender, args) => onNewTerm();
            termStack.Children.Add(buttonTermAdd);

            foreach (Course course in courses[termSelected])
            {
                Grid grid = new Grid
                {
                    BackgroundColor = Colors.White
                };
                Button button = new Button
                {
                    Text = course.courseName
                };
                grid.Add(button);
                SwipeItem deleteItem = new SwipeItem
                {
                    Text = "Delete",
                    BindingContext = course,
                    BackgroundColor = Colors.OrangeRed

                };
                deleteItem.Invoked += onDeleteInvoked;
                List<SwipeItem> items = new List<SwipeItem>() { deleteItem };
                SwipeView swp = new SwipeView
                {
                    RightItems = new SwipeItems(items),
                    Content = grid
                };
                button.Clicked += async (sender, args) => await Navigation.PushAsync(new CoursePage(course.courseID));

                courseStack.Children.Add(swp);
            }
            if (courses[termSelected].Count < 6)
            {
                Button buttonCourseAdd = new Button()
                {
                    Text = "Add Course",
                };
                buttonCourseAdd.Clicked += void (sender, args) => onNewCourse();
                courseStack.Children.Add(buttonCourseAdd);
            }
            if (courses[termSelected].Count == 0)
            {
                Button buttonTermRemove = new Button()
                {
                    Text = "Delete Term",
                    BackgroundColor = Colors.Red
                };
                buttonTermRemove.Clicked += void (sender, args) => onTermDelete();
                courseStack.Children.Add(buttonTermRemove);
            }



            termStart.Date = termSelected.start;
            termEnd.Date = termSelected.end;
            termTitle.Text = termSelected.termName;

        }

        public void onNewCourse()
        {
            AppFunctions.addNewCourse(termSelected.termID);
            loadUI(termSelected.termID);
        }
        public void onNewTerm()
        {
            AppFunctions.addNewTerm();
            loadUI(termSelected.termID);
        }
        public void onTermDelete()
        {
            var db = new SQLiteConnection(appDatabase);
            db.Delete(termSelected);
            sync_db();
            loadUI(1);
        }
        private void onDeleteInvoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var course = item.BindingContext as Course;
            AppFunctions.deleteCourse(course);
            MainPage.sync_db();
            loadUI(termSelected.termID);
        }
        public static bool checkDates(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void termTitleChange(object sender, TextChangedEventArgs e)
        {
            var db = new SQLiteConnection(appDatabase);

            if (e.NewTextValue != null)
            {
                termSelected.termName = e.NewTextValue;
                AppFunctions.updateTerm(db, termSelected);
                loadUI(termSelected.termID);
            }
        }

        private void termEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            var db = new SQLiteConnection(appDatabase);
            bool valid = checkDates(termStart.Date, termEnd.Date);
            if (valid)
            {
                termEnd.Date = e.NewDate;
                termSelected.end = e.NewDate;
                AppFunctions.updateTerm(db, termSelected);
            }
        }
        private void termStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            var db = new SQLiteConnection(appDatabase);
            bool valid = checkDates(termStart.Date, termEnd.Date);
            if (valid)
            {
                termStart.Date = e.NewDate;
                termSelected.start = e.NewDate;
                AppFunctions.updateTerm(db, termSelected);
            }

        }
    }

}
