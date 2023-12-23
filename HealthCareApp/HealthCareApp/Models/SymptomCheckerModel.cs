using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    internal class SymptomCheckerModel
    {
        public class symptom
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }
        public class Autherity
        {
            public string Token { get; set; }
            public int ValidThrough { get; set; }
            public class rootSymptoms
            {
                public List<symptom> symptoms = new List<symptom>();
            }
            public class issueInfo
            {
                public string Description { get; set; }
                public string DescriptionShort { get; set; }
                public string MedicalCondition { get; set; }
                public string Name { get; set; }
                public string PossibleSymptoms { get; set; }
                public string ProfName { get; set; }
                public string Synonyms { get; set; }
                public string TreatmentDescription { get; set; }
            }
            public class Issue
            {
                public int ID { get; set; }
                public string Name { get; set; }
                public double Accuracy { get; set; }
                public string Icd { get; set; }
                public string IcdName { get; set; }
                public string ProfName { get; set; }
                public int Ranking { get; set; }
            }
            public class Specialisation
            {
                public int ID { get; set; }
                public string Name { get; set; }
                public int SpecialistID { get; set; }
            }
            public class IssueComponent
            {
                public Issue Issue { get; set; }
                public Specialisation Specialisation { get; set; }
            }
        }
    }
}
