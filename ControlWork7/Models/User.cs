﻿using System.ComponentModel.DataAnnotations;

namespace ControlWork7.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    [Required]
    public string Password { get; set; }

    public List<UserAndBook>? UsersAndBooks { get; set; } = new();
}
