using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic;
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

        public string firstExerciseName;
        public string FirstExerciseName { get { return firstExerciseName; } set { firstExerciseName = value; OnPropertyChanged(nameof(FirstExerciseName)); } }

        public string firstEquipmentBox;
        public string FirstEquipmentBox { get { return firstEquipmentBox; } set { firstEquipmentBox = value; OnPropertyChanged(nameof(FirstEquipmentBox)); } }

        public string firstInstructionsBox;
        public string FirstInstructionsBox { get { return firstInstructionsBox; } set { firstInstructionsBox = value; OnPropertyChanged(nameof(FirstInstructionsBox)); } }
        public string secondExerciseName;
        public string SecondExerciseName { get { return secondExerciseName; } set { secondExerciseName = value; OnPropertyChanged(nameof(SecondExerciseName)); } }

        public string secondEquipmentBox;
        public string SecondEquipmentBox { get { return secondEquipmentBox; } set { secondEquipmentBox = value; OnPropertyChanged(nameof(SecondEquipmentBox)); } }

        public string secondInstructionsBox;
        public string SecondInstructionsBox { get { return secondInstructionsBox; } set { secondInstructionsBox = value; OnPropertyChanged(nameof(SecondInstructionsBox)); } }
        public string thirdExerciseName;
        public string ThirdExerciseName { get { return thirdExerciseName; } set { thirdExerciseName = value; OnPropertyChanged(nameof(ThirdExerciseName)); } }

        public string thirdEquipmentBox;
        public string ThirdEquipmentBox { get { return thirdEquipmentBox; } set { thirdEquipmentBox = value; OnPropertyChanged(nameof(ThirdEquipmentBox)); } }

        public string thirdInstructionsBox;
        public string ThirdInstructionsBox { get { return thirdInstructionsBox; } set { thirdInstructionsBox = value; OnPropertyChanged(nameof(ThirdInstructionsBox)); } }
        public string fouthExerciseName;
        public string FouthExerciseName { get { return fouthExerciseName; } set { fouthExerciseName = value; OnPropertyChanged(nameof(FouthExerciseName)); } }

        public string fouthEquipmentBox;
        public string FouthEquipmentBox { get { return fouthEquipmentBox; } set { fouthEquipmentBox = value; OnPropertyChanged(nameof(FouthEquipmentBox)); } }

        public string fouthInstructionsBox;
        public string FouthInstructionsBox { get { return fouthInstructionsBox; } set { fouthInstructionsBox = value; OnPropertyChanged(nameof(FouthInstructionsBox)); } }
        public string firthExerciseName;
        public string FirthExerciseName { get { return firthExerciseName; } set { firthExerciseName = value; OnPropertyChanged(nameof(FirthExerciseName)); } }

        public string firthEquipmentBox;
        public string FirthEquipmentBox { get { return firthEquipmentBox; } set { firthEquipmentBox = value; OnPropertyChanged(nameof(FirthEquipmentBox)); } }

        public string firthInstructionsBox;
        public string FirthInstructionsBox { get { return firthInstructionsBox; } set { firthInstructionsBox = value; OnPropertyChanged(nameof(FirthInstructionsBox)); } }


        public AddExerciseViewModel()
        {
            AddMuscleList();
            AddTypeList();
            AddDifficultyList();
            ExitBtnCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            SuggestBtnCommand = new RelayCommand<Card>((p) =>
            {
                return (MuscleBox == null || MuscleBox == string.Empty ||
                        TypeBox == null || TypeBox == string.Empty ||
                        DifficultyBox == null || DifficultyBox == string.Empty) ? false : true;
            }, (p) => {
                if (NameMerge == null || NameMerge == string.Empty) NameMerge = string.Empty;
                getExercise(NameMerge, TypeBox, MuscleBox, DifficultyBox,p);
            });
            MusleSelectedItemChangedCommand = new RelayCommand<ComboBox>((p) => { return (p == null) ? false : true; }, (p) =>
            {
                if (p.SelectedValue != null)
                    MuscleBox = p.SelectedValue.ToString();
            });
            TypeSelectedItemChangedCommand = new RelayCommand<ComboBox>((p) => { return (p == null) ? false : true; }, (p) =>
            {
                if (p.SelectedValue != null)
                    TypeBox = p.SelectedValue.ToString();
            });
            DifficultySelectedItemChangedCommand = new RelayCommand<ComboBox>((p) => { return (p == null) ? false : true; }, (p) =>
            {
                if (p.SelectedValue != null)
                    DifficultyBox = p.SelectedValue.ToString();
            });
        }
        string api_key = "2qhorCkNJtS7gPC5veMb0w==vuH8jN3gfZBHCzzh";
        string url = "https://api.api-ninjas.com/v1/exercises?";
        public List<exercise> ListExercise;
        bool SuccessSuggest;
        private async void getExercise(string name, string type, string muscle, string difficulty,Card p)
        {
            string result = await requestExercise(name, type, muscle, difficulty);
            if (result.Length < 3)
            {
                p.Visibility = Visibility.Hidden;
                MessageBox.Show("Can't find any exercises fit your options. Please try again!");
                return;
            }
            p.Visibility = Visibility.Visible;
            ListExercise = ConvertData(result);
            ShowResult(ListExercise);
            return;
            // ListExercise = JsonConvert.DeserializeObject<rootExerciseInfo>(result);
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
        private List<exercise> ConvertData(string data)
        {
            List<exercise> result = new List<exercise>();
            int i = 2;
            int pair = 0;
            string element = "";
            while (data[i] != ']')
            {
                if (data[i] == '}' || data[i] == '{')
                    pair++;
                if (pair < 2) { element += data[i]; }
                else
                {
                    string namePath = convertPath(element, 1);
                    string equipmetnPath = convertPath(element, 4);
                    string instructionsPath = convertPath(element, 6);
                    exercise newExercise = new exercise();
                    newExercise.name = namePath;
                    newExercise.type = typeBox;
                    newExercise.muscle = MuscleBox;
                    newExercise.equipment = equipmetnPath;
                    newExercise.difficulty = DifficultyBox;
                    newExercise.instructions = instructionsPath;

                    result.Add(newExercise);

                    element = "";
                    pair = 0;
                }
                ++i;
            }
            return result;
        }
        private string convertPath(string element, int k)
        {
            string temp = "";
            int count = 0, i = 0;
            while (count < k)
            {
                if (element[i] == ':') count++;
                ++i;
            }
            while (element[i] != '"')
                ++i;
            i = i + 1;
            while (element[i] != '"')
            {
                temp += element[i];
                ++i;
            }
            return temp;
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
        private void ShowResult(List<exercise> list)
        {
            FirstExerciseName = list[0].name;
            FirstEquipmentBox = "Equipment: " + list[0].equipment;
            FirstInstructionsBox = list[0].instructions;

            SecondExerciseName = list[1].name;
            SecondEquipmentBox = "Equipment: " + list[1].equipment;
            SecondInstructionsBox = list[1].instructions;

            ThirdExerciseName = list[2].name;
            ThirdEquipmentBox = "Equipment: " + list[2].equipment;
            ThirdInstructionsBox = list[2].instructions;

            FouthExerciseName = list[3].name;
            FouthEquipmentBox = "Equipment: " + list[3].equipment;
            FouthInstructionsBox = list[3].instructions;

            FirthExerciseName = list[4].name;
            FirthEquipmentBox = "Equipment: " + list[4].equipment;
            FirthInstructionsBox = list[4].instructions;
        }
    }
}
