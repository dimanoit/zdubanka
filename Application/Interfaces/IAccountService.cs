﻿using Domain.Entities;

namespace Application.Interfaces;

public interface IAccountService
{
    Task AddAsync(Account account);
    Task<Account?> GetAccountByIdAsync(string id, CancellationToken cancellationToken);
    Task<Account?> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Account account);
    Task<string> GetUserIdByTokenAsync(string token);
}