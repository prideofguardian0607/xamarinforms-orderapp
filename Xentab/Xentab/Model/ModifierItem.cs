using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Xentab.Model
{
    public class ModifierItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("isPizzaCrust")]
        public bool IsPizzaCrust { get; set; }

        [JsonPropertyName("isPizzaTopping")]
        public bool IsPizzaTopping { get; set; }

        [JsonPropertyName("isBarMixer")]
        public bool IsBarMixer { get; set; }

        [JsonPropertyName("isBarDrink")]
        public bool IsBarDrink { get; set; }
    }

    public class ModifierGroup
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
