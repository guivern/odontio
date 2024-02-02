﻿namespace Odontio.Domain.Entities;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}