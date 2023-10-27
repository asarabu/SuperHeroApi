using System;
using System.Collections.Generic;

namespace SuperHeroApi.Models;

public partial class SuperVillain
{
    public int SuperVillainId { get; set; }

    public int? SuperHeroId { get; set; }

    public string? VillainName { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? SuperPowers { get; set; }

    public string? City { get; set; }

    public Guid? CharacterStoryId { get; set; }

    public virtual SuperHero? SuperHero { get; set; }
}
