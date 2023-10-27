namespace SuperHeroApi.ViewModels
{
    public class SuperVillainDto
    {
        public int SuperVillainId { get; set; }

        public int? SuperHeroId { get; set; }

        public string? VillainName { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? SuperPowers { get; set; }

        public string? City { get; set; }

        public Guid? CharacterStoryId { get; set; }
    }
}
