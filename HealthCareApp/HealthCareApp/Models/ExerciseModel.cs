using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    public class ExerciseModel
    {
        public class exercise
        {
            public string name { get; set; }
            public string type { get; set; }
            public string muscle { get; set; }
            public string equipment { get; set; }
            public string difficulty { get; set; }
            public string instructions { get; set; }

        }
        public class rootExercises
        {
            public List<exercise> exercises { get; set; }
        }
        public class photo
        {
            public string highres { get; set; }
            public string thumb { get; set; }
            public bool is_user_uploaded { get; set; }

        }
        public class exerciseInfo
        {
            public int tag_id { get; set; }
            public string user_input { get; set; }
            public double duration_min { get; set; }
            public double met { get; set; }
            public double nf_calories { get; set; }
            public photo photo { get; set; }
            public string compendium_code { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string benefits { get; set; }

        }
        public class rootExerciseInfo
        {
            public List<exerciseInfo> exercises { get; set; }
        }
    }
}
