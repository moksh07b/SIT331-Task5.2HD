﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Gallery.Models;

public partial class Artist
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Bio { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? DeathDate { get; set; }

    public string Nationality { get; set; }

    public virtual ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
}