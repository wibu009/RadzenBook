﻿namespace RadzenBook.Contract.DTO.Auth;

public class RegisterRequestDto
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
}