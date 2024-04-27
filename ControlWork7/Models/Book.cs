﻿using System.ComponentModel.DataAnnotations;

namespace ControlWork7.Models;

public class Book
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public string PhotoUrl { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string Status { get; set; }
}
