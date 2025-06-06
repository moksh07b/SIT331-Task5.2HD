﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Gallery.Models;

public partial class Artifact
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public int? ArtistId { get; set; }

    public int? ExhibitionId { get; set; }

    public virtual Artist Artist { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Exhibition Exhibition { get; set; }
}