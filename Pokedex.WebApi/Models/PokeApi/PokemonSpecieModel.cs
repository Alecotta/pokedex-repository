﻿using System.Text.Json.Serialization;

namespace Pokedex.WebApi.Models.PokeApi
{
    public class Area
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class Color
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class EggGroup
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class EvolutionChain
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class FlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; } = string.Empty;
        [JsonPropertyName("language")]
        public Language? Language { get; set; }
        [JsonPropertyName("version")]
        public Version? Version { get; set; }
    }

    public class Genera
    {
        [JsonPropertyName("genus")]
        public string Genus { get; set; } = string.Empty;
        [JsonPropertyName("language")]
        public Language? Language { get; set; }
    }

    public class Generation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class GrowthRate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class Name
    {
        [JsonPropertyName("language")]
        public Language? Language { get; set; }
        public string name { get; set; } = string.Empty;
    }

    public class PalParkEncounter
    {
        [JsonPropertyName("area")]
        public Area? Area { get; set; }
        [JsonPropertyName("base_score")]
        public int BaseScore { get; set; }
        [JsonPropertyName("rate")]
        public int Rate { get; set; }
    }

    public class Pokedex
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class PokedexNumber
    {
        [JsonPropertyName("entry_number")]
        public int EntryNumber { get; set; }
        [JsonPropertyName("pokedex")]
        public Pokedex? Pokedex { get; set; }
    }

    public class Pokemon
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class PokemonSpecieModel
    {
        [JsonPropertyName("base_happiness")]
        public int BaseHappiness { get; set; }
        [JsonPropertyName("capture_rate")]
        public int CaptureRate { get; set; }
        [JsonPropertyName("color")]
        public Color? Color { get; set; }
        [JsonPropertyName("egg_groups")]
        public List<EggGroup>? EggGroups { get; set; }
        [JsonPropertyName("evolution_chain")]
        public EvolutionChain? EvolutionChain { get; set; }
        [JsonPropertyName("evolves_from_species")]
        public object? EvolvesFromSpecies { get; set; }
        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorTextEntry>? FlavorTextEntries { get; set; }
        [JsonPropertyName("form_descriptions")]
        public List<object>? FormDescriptions { get; set; }
        [JsonPropertyName("forms_switchable")]
        public bool FormsSwitchable { get; set; }
        [JsonPropertyName("gender_rate")]
        public int GenderRate { get; set; }
        [JsonPropertyName("genera")]
        public List<Genera>? Genera { get; set; }
        [JsonPropertyName("generation")]
        public Generation? Generation { get; set; }
        [JsonPropertyName("growth_rate")]
        public GrowthRate? GrowthRate { get; set; }
        [JsonPropertyName("habitat")]
        public Habitat? Habitat { get; set; }
        [JsonPropertyName("has_gender_differences")]
        public bool HasGenderDifferences { get; set; }
        [JsonPropertyName("hatch_counter")]
        public int HatchCounter { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("is_baby")]
        public bool IsBaby { get; set; }
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
        [JsonPropertyName("is_mythical")]
        public bool IsMythical { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("names")]
        public List<Name>? Names { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("pal_park_encounters")]
        public List<PalParkEncounter>? PalParkEncounters { get; set; }
        [JsonPropertyName("pokedex_numbers")]
        public List<PokedexNumber>? PokedexNumbers { get; set; }
        [JsonPropertyName("shape")]
        public Shape? Shape { get; set; }
        [JsonPropertyName("varieties")]
        public List<Variety>? Varieties { get; set; }
    }

    public class Shape
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class Variety
    {
        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }
        [JsonPropertyName("pokemon")]
        public Pokemon? Pokemon { get; set; }
    }

    public class Version
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
