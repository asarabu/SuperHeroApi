using System.Text.Json.Serialization;

namespace SuperHeroApi.ViewModels
{
    public class SuperHeroDto
    {
        public int SuperHeroId { get; set; }

        public string? HeroName { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? SuperPowers { get; set; }

        public string? City { get; set; }
    }
}
