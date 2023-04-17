using Domain.Entities;
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
            Id = Guid.NewGuid().ToString(),
            Token = Guid.NewGuid().ToString(),
        };

        return account;
    }

    public static AccountShort ToAccountShort(this GoogleJsonWebSignature.Payload payload)
    {
        var account = new AccountShort
        {
            Email = payload.Email,
            FullName = payload.Name,
            ProfileImg = payload.Picture
        };

        return account;
    }

    public static AccountShort ToAccountShort(this Account account)
    {
        var shortAccount = new AccountShort
        {
            Email = account.Email,
            FullName = account.FullName,
            ProfileImg = account.ImageUrl!,
            Token = account.Token
        };

        return shortAccount;
    }
}