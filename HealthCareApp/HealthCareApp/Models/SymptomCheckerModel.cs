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
        }
        public class rootSymptoms
        {
            public List<symptom> symptoms = new List<symptom>();
        }
    }
}
