using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareApp.Models
{
    public class NutrientsModel
    {
        public class full_nutrient
        {
            public int attr_id { get; set; }
            public double value { get; set; }
        }
        public class metadata
        {
            public bool is_raw_food { get; set; }
        }
        public class tag
        {
            public string item { get; set; }
            public string measure { get; set; }
            public double quantity { get; set; }
            public int food_group { get; set; }
            public string tag_id { get; set; }

        }
        public class alt_measure
        {
            public long serving_weight { get; set; }
            public string measure { get; set; }
            public string seq { get; set; }
            public string qty { get; set; }

        }
        public class photo
        {
            public string thumb { get; set; }
            public string highres { get; set; }
            public bool is_user_uploaded { get; set; }
        }
        public class food
        {
            public string food_name { get; set; }
            public string brand_name { get; set; }
            public int serving_qty { get; set; }
            public string serving_unit { get; set; }
            public int serving_weight_grams { get; set; }
            public double nf_calories { get; set; }
            public double nf_total_fat { get; set; }
            public double nf_saturated_fat { get; set; }
            public double nf_cholesterol { get; set; }
            public double nf_sodium { get; set; }
            public double nf_total_carbohydrate { get; set; }
            public double nf_dietary_fiber { get; set; }
            public double nf_sugars { get; set; }
            public double nf_protein { get; set; }
            public double nf_potassium { get; set; }
            public double nf_p { get; set; }
            public List<full_nutrient> full_nutrients { get; set; }
            public string nix_brand_name { get; set; }
            public string nix_brand_id { get; set; }
            public string nix_item_name { get; set; }
            public string nix_item_id { get; set; }
            public string upc { get; set; }
            public string consumed_at { get; set; }
            public metadata metadata { get; set; }
            public int source { get; set; }
            public int ndb_no { get; set; }
            public tag tags { get; set; }
            public List<alt_measure> alt_measures { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public int meal_type { get; set; }
            public photo photo { get; set; }
            public string sub_recipe { get; set; }
            public string class_code { get; set; }
            public string brick_code { get; set; }
            public string tag_id { get; set; }
        }
        public class root
        {
            public List<food> foods { get; set; }
        }
    }
}
