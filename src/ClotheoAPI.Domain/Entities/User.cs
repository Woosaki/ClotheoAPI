﻿namespace ClotheoAPI.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastOnlineDate { get; set; }
}
