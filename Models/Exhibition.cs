﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Gallery.Models;

public partial class Exhibition
{
    public int Id { get; set; }

    public string Title { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Location { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
}