using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static HealthCareApp.Models.ExerciseModel;

namespace HealthCareApp.ViewModels
{
    internal class AddExerciseViewModel : BaseViewModel
    {
        public ICommand ExitBtnCommand { get; set; }
        public ICommand SuggestBtnCommand { get; set; }
        public ICommand MusleSelectedItemChangedCommand { get; set; }
        public ICommand TypeSelectedItemChangedCommand { get; set; }
        public ICommand DifficultySelectedItemChangedCommand { get; set; }
        public string nameMerge;
        public string NameMerge { get { return nameMerge; } set { nameMerge = value; OnPropertyChanged(nameof(NameMerge)); } }
        public string typeBox;
        public string TypeBox { get { return typeBox; } set { typeBox = value; OnPropertyChanged(nameof(TypeBox)); } }

        public string muscleBox;
        public string MuscleBox { get { return muscleBox; } set { muscleBox = value; OnPropertyChanged(nameof(MuscleBox)); } }

        public string difficultyBox;
        public string DifficultyBox { get { return difficultyBox; } set { difficultyBox = value; OnPropertyChanged(nameof(DifficultyBox)); } }

        public List<string> tempMuscleList = new List<string>();
        public List<string> TempMuscleList { get { return tempMuscleList; } set { tempMuscleList = value; OnPropertyChanged(nameof(TempMuscleList)); } }
        public List<string> tempTypeList = new List<string>();
        public List<string> TempTypeList { get { return tempTypeList; } set { tempTypeList = value; OnPropertyChanged(nameof(TempTypeList)); } }
        public List<string> tempDifficultyList = new List<string>();
        public List<string> TempDifficultyList { get { return tempDifficultyList; } set { tempDifficultyList = value; OnPropertyChanged(nameof(TempDifficultyList)); } }

        public AddExerciseViewModel()
        {
            AddMuscleList();
            AddTypeList();
            AddDifficultyList();
            ExitBtnCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            SuggestBtnCommand = new RelayCommand<TextBox>((p) =>
            {
                if ((MuscleBox == null || MuscleBox == string.Empty) & (TypeBox == null || TypeBox == string.Empty) & (DifficultyBox == null || DifficultyBox == string.Empty))
                    return false;
                return true;
            }, (p) => {

            });
            MusleSelectedItemChangedCommand = new RelayCommand<ComboBox>((p) => { return (p == null) ? false : true; }, (p) =>
            {
                MuscleBox = p.Text;
            });
            TypeSelectedItemChangedCommand = new RelayCommand<ComboBox>((p) => { return (p == null) ? false : true; }, (p) =>
            {
                TypeBox = p.Text;
            });
            DifficultySelectedItemChangedCommand = new RelayCommand<ComboBox>((p) => { return (p == null) ? false : true; }, (p) =>
            {
                DifficultyBox = p.Text;
            });
        }
        string api_key = "2qhorCkNJtS7gPC5veMb0w==vuH8jN3gfZBHCzzh";
        string url = "https://api.api-ninjas.com/v1/exercises?";
        public rootExercises ListExercise;
        private async void getExercise(string name, string type, string muscle, string difficulty)
        {
            string result = await requestExercise(name, type, muscle, difficulty);
        }
        private async Task<string> requestExercise(string name, string type, string muscle, string difficulty)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", api_key);

                var uri = new Uri($"{url}muscle={muscle}&type={type}&difficulty={difficulty}");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error:{response.StatusCode},{await response.Content.ReadAsStringAsync()}";
                }
            }
        }
        private rootExercises ConvertData(string data)
        {
            rootExercises result = new rootExercises();
            int i = 2;
            int pair = 0;
            string element = "";
            while (data[i] != ']')
            {
                if (data[i] == '}')
                    if (pair < 2) { element += data[i]; ++i; }
                    else
                    {

                    }
            }
            return result;
        }
        private void AddMuscleList()
        {
            List<string> list = new List<string>();
            list.Add("abdominals");
            list.Add("abductors");
            list.Add("adductors");
            list.Add("biceps");
            list.Add("calves");
            list.Add("chest");
            list.Add("forearms");
            list.Add("glutes");
            list.Add("hamstrings");
            list.Add("lats");
            list.Add("lower_back");
            list.Add("middle_back");
            list.Add("neck");
            list.Add("quadriceps");
            list.Add("traps");
            list.Add("triceps");
            TempMuscleList = list;
        }
        private void AddTypeList()
        {
            List<string> list = new List<string>();
            list.Add("cardio");
            list.Add("cardioolympic_weightlifting");
            list.Add("plyometrics");
            list.Add("powerlifting");
            list.Add("strength");
            list.Add("stretching");
            list.Add("strongman");
            TempTypeList = list;
        }
        private void AddDifficultyList()
        {
            List<string> list = new List<string>();
            list.Add("beginner");
            list.Add("intermediate");
            list.Add("expert");
            TempDifficultyList = list;
        }
    }
}
