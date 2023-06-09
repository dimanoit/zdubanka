﻿using Domain.Entities;
using Domain.Response;
using Google.Apis.Auth;

namespace Api.Mappers;

public static class AccountMapper
{
    public static Account ToAccount(this GoogleJsonWebSignature.Payload payload)
    {
        var account = new Account
        {
            Email = payload.Email,
            FullName = payload.Name,
            ImageUrl = payload.Picture,
            Id = Guid.NewGuid().ToString()
        };

        return account;
    }

    public static AccountShort ToAccountShort(this Account account)
    {
        var shortAccount = new AccountShort
        {
            Email = account.Email!,
            FullName = account.FullName,
            ProfileImg = account.ImageUrl!
        };

        return shortAccount;
    }
}