﻿using Domain.Models;

namespace Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailBaseRequest baseRequest);
}