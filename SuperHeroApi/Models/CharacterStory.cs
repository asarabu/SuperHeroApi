using System;
using System.Collections.Generic;

namespace SuperHeroApi.Models;

public partial class CharacterStory
{
    public Guid CharacterStoryId { get; set; }

    public string? Title { get; set; }

    public string? ShortDescription { get; set; }

    public string? Story { get; set; }

    public string? UrlHandle { get; set; }

    public string? ImageUrl { get; set; }

    public string? Author { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? LastUpdatedBy { get; set; }

    public bool? IsVisible { get; set; }

    public int? SuperHeroId { get; set; }

    public int? SuperVillainId { get; set; }

    public virtual SuperHero? SuperHero { get; set; }

    public virtual SuperVillain? SuperVillain { get; set; }
}
