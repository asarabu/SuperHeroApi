using System;
using System.Collections.Generic;

namespace SuperHeroApi.Models;

public partial class SuperHero
{
    public int SuperHeroId { get; set; }

    public string? HeroName { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? SuperPowers { get; set; }

    public string? City { get; set; }

    public string? Image { get; set; }

    public Guid? CharacterStoryId { get; set; }

    public virtual ICollection<SuperVillain> SuperVillains { get; set; } = new List<SuperVillain>();
}
