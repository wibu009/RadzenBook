﻿namespace RadzenBook.Infrastructure.Mail;

public class SendGridSettings
{
    public string Key { get; set; } = default!;
    public string From { get; set; } = default!;
    public string FromName { get; set; } = default!;
}