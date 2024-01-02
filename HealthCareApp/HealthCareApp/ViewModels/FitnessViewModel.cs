using HealthCareApp.Models;
using HealthCareApp.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthCareApp.ViewModels
{
    public class FitnessViewModel : BaseViewModel
    {
        private string _viewby;
        public string ViewByVM
        {
            get { return _viewby; }
            set
            {
                if (_viewby != value)
                {
                    _viewby = value;
                    OnPropertyChanged(nameof(ViewByVM));
                }
            }
        }
        private DateTime? _viewbyday;
        public DateTime? ViewByDayVM
        {
            get { return _viewbyday; }
            set
            {
                if (_viewbyday != value)
                {
                    _viewbyday = value;
                    OnPropertyChanged(nameof(ViewByDayVM));
                }
            }
        }
        private string _viewbymonth;
        public string ViewByMonthVM
        {
            get { return _viewbymonth; }
            set
            {
                if (_viewbymonth != value)
                {
                    _viewbymonth = value;
                    OnPropertyChanged(nameof(ViewByMonthVM));
                }
            }
        }
        private string _viewbyyear;
        public string ViewByYearVM
        {
            get { return _viewbyyear; }
            set
            {
                if (_viewbyyear != value)
                {
                    _viewbyyear = value;
                    OnPropertyChanged(nameof(ViewByYearVM));
                }
            }
        }
        private string _sreachmeal;
        public string SreachMealVM
        {
            get { return _sreachmeal; }
            set
            {
                if (_sreachmeal != value)
                {
                    _sreachmeal = value;
                    OnPropertyChanged(nameof(SreachMealVM));
                }
            }
        }
        private ObservableCollection<ExerciseLesson> _exerciseList;
        public ObservableCollection<ExerciseLesson> ExerciseList
        {
            get { return _exerciseList; }
            private set
            {
                _exerciseList = value;
                OnPropertyChanged(nameof(ExerciseList));
            }
        }
        private readonly IMongoCollection<ExerciseLesson> _ExerciseCollection;
        public ICommand CalculateCaloriesCommand { get; set; }
        public ICommand TrainingBtnCommand { get; set; }
        public ICommand AddExerciseCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public ICommand EditLessonCommand { get; set; }
        public ICommand DeleteLessonCommand { get; set; }
        public ICommand SreachExerciseCommand { get; set; }
        public ICommand ViewByChangeCM { get; set; }
        public ICommand ViewByDayChangeCM { get; set; }
        public ICommand ViewByMonthChangeCM { get; set; }
        public ICommand ViewByYearChangeCM { get; set; }
        public string ViewByMode { get; set; }
        public FitnessViewModel()
        {
            _ExerciseCollection = GetMongoCollectionFromExerciseLesson();
            CalculateCaloriesCommand = new RelayCommand<FitnessView>((p) => { return true; }, (p) => CalculateCaloriesCM(p));
            TrainingBtnCommand = new RelayCommand<FitnessView>((p) => { return true; }, (p) => TrainingCM(p));
            AddExerciseCommand = new RelayCommand<FitnessView>((p) => { return true; }, (p) => AddExerciseCM(p));

            ViewByChangeCM = new RelayCommand<FitnessView>((p) => true, (p) => ViewByChange(p));
            ViewByDayChangeCM = new RelayCommand<FitnessView>((p) => true, (p) => ViewByDayChange(p));
            ViewByMonthChangeCM = new RelayCommand<FitnessView>((p) => true, (p) => ViewByMonthChange(p));
            ViewByYearChangeCM = new RelayCommand<FitnessView>((p) => true, (p) => ViewByYearChange(p));

            SreachExerciseCommand = new RelayCommand<FitnessView>((p) => true, (p) => SreachExercise(p));
            LoadWindowCommand = new RelayCommand<FitnessView>((p) => true, (p) => LoadWindowCM());
            EditLessonCommand = new RelayCommand<ExerciseLesson>((meal) => true, (meal) => EditExercise(meal));
            DeleteLessonCommand = new RelayCommand<ExerciseLesson>((meal) => true, (meal) => DeleteExercise(meal));
        }
        public void SreachExercise(FitnessView parameter)
        {
            var filterBuilder = Builders<ExerciseLesson>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);

            FilterDefinition<ExerciseLesson> finalFilter = usernameFilter;

            if (!string.IsNullOrEmpty(parameter.SreachExercise.Text))
            {
                var keyword = parameter.SreachExercise.Text;

                finalFilter &= filterBuilder.Regex(
                    x => x.ExerciseName,
                    new BsonRegularExpression($".*{keyword}.*", "i")
                );

                var ex = _ExerciseCollection.Find(finalFilter).ToList();
                ExerciseList = new ObservableCollection<ExerciseLesson>(ex);
            }
            else
            {
                LoadWindowCM();
            }
        }
        public void CalculateCaloriesCM(FitnessView p)
        {
            ExerciseCaloriesView window = new ExerciseCaloriesView();
            p.Opacity = 0.5;
            window.ShowDialog();
            p.Opacity = 1;
            LoadWindowCM();
        }
        public void AddExerciseCM(FitnessView p)
        {
            AddLessonExerciseView window = new AddLessonExerciseView();
            p.Opacity = 0.5;
            window.ShowDialog();
            p.Opacity = 1;
            LoadWindowCM();
        }
        public void TrainingCM(FitnessView p)
        {
            AddExerciseView window = new AddExerciseView();
            p.Opacity = 0.5;
            window.ShowDialog();
            p.Opacity = 1;
            LoadWindowCM();
        }
        public void ViewByChange(FitnessView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByVM))
            {
                if (ViewByVM == "Day")
                {
                    parameter.ChooseDay.Visibility = Visibility.Visible;
                    parameter.ChooseMonth.Visibility = Visibility.Hidden;
                    parameter.ChooseYear.Visibility = Visibility.Hidden;
                    ViewByMonthVM = null;
                    ViewByYearVM = null;
                    ViewByMode = null;
                }
                if (ViewByVM == "Month")
                {
                    parameter.ChooseDay.Visibility = Visibility.Hidden;
                    parameter.ChooseMonth.Visibility = Visibility.Visible;
                    parameter.ChooseYear.Visibility = Visibility.Hidden;
                    ViewByDayVM = null;
                    ViewByYearVM = null;
                    ViewByMode = null;
                }
                if (ViewByVM == "Year")
                {
                    parameter.ChooseDay.Visibility = Visibility.Hidden;
                    parameter.ChooseMonth.Visibility = Visibility.Hidden;
                    parameter.ChooseYear.Visibility = Visibility.Visible;
                    ViewByDayVM = null;
                    ViewByMonthVM = null;
                    ViewByMode = null;
                }
            }
            else
            {
                parameter.ChooseDay.Visibility = Visibility.Hidden;
                parameter.ChooseMonth.Visibility = Visibility.Hidden;
                parameter.ChooseYear.Visibility = Visibility.Hidden;
                ViewByDayVM = null;
                ViewByMonthVM = null;
                ViewByYearVM = null;
                ViewByMode = null;
                LoadWindowCM();
            }
        }
        public void ViewByDayChange(FitnessView parameter)
        {
            string day = ViewByDayVM.HasValue ? ViewByDayVM.Value.ToString("dd/MM/yyyy") : null;
            if (!string.IsNullOrEmpty(day))
            {
                ViewByMode = "Day";
            }
            else { ViewByMode = null; }
            LoadWindowCM();
        }
        public void ViewByMonthChange(FitnessView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByMonthVM))
            {
                ViewByMode = "Month";
            }
            else { ViewByMode = null; }
            LoadWindowCM();
        }
        public void ViewByYearChange(FitnessView parameter)
        {
            if (!string.IsNullOrEmpty(ViewByYearVM))
            {
                ViewByMode = "Year";
            }
            else { ViewByMode = null; }
            LoadWindowCM();
        }
        public void LoadWindowCM()
        {
            var filterBuilder = Builders<ExerciseLesson>.Filter;
            var usernameFilter = filterBuilder.Eq(x => x.Username, Const.Instance.Username);

            FilterDefinition<ExerciseLesson> finalFilter = usernameFilter;

            if (!string.IsNullOrEmpty(ViewByMode))
            {
                if (ViewByMode == "Day")
                {
                    string searchDay = ViewByDayVM.HasValue ? ViewByDayVM.Value.ToString("dd/MM/yyyy") : null;
                    finalFilter &= filterBuilder.Eq(x => x.ExerciseDay, searchDay);
                }
                else if (ViewByMode == "Month")
                {
                    int targetMonth = int.Parse(ViewByMonthVM);
                    string monthString = targetMonth.ToString("00");

                    finalFilter &= filterBuilder.Regex(
                        x => x.ExerciseDay,
                        new BsonRegularExpression($@"^.*\/{monthString}\/[0-9]{{4}}$")
                    );
                }
                else if (ViewByMode == "Year")
                {
                    int targetYear = int.Parse(ViewByYearVM);
                    finalFilter &= filterBuilder.Regex(
                        x => x.ExerciseDay,
                        new BsonRegularExpression($@"^.*\/[0-9]{{2}}\/{targetYear}$")
                    );
                }
            }

            var exercise = _ExerciseCollection.Find(finalFilter).ToList();

            ExerciseList = new ObservableCollection<ExerciseLesson>(exercise);
        }
        public void EditExercise(ExerciseLesson lesson)
        {
            if (lesson != null)
            {
                EditExerciseListViewModel editExViewModel = new EditExerciseListViewModel(lesson);
                EditExerciseListView editView = new EditExerciseListView();
                editView.DataContext = editExViewModel;
                var window = new Window
                {
                    Content = editView,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                window.Closed += (sender, args) => LoadWindowCM();
                window.ShowDialog();
            }
        }
        public void DeleteExercise(ExerciseLesson lesson)
        {
            if (lesson != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {lesson.ExerciseName}?",
                                                          "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var objectId = ObjectId.Parse(lesson.Id);
                    _ExerciseCollection.DeleteOne(Builders<ExerciseLesson>.Filter.Eq("_id", objectId));
                    LoadWindowCM();
                }
            }
        }
        private IMongoCollection<ExerciseLesson> GetMongoCollectionFromExerciseLesson()
        {
            // Set your MongoDB connection string and database name
            string connectionString = "mongodb+srv://HGB3009:HGB30092004@bao-database.xwrghva.mongodb.net/"; // Update with your MongoDB server details
            string databaseName = "HealthcareManagementDatabase"; // Update with your database name

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<ExerciseLesson>("ExerciseLesson");
        }
    }
}
