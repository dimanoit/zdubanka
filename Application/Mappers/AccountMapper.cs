using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Application.Mappers;

public static class AccountMapper
{
    public static void UpdateAccount(this UpdateAccountRequest accountRequest, Account account)
    {
        account.FullName = accountRequest.FullName;
        account.Bio = accountRequest.Bio;
        account.ImageUrl = accountRequest.ImageUrl;
        account.RelationshipStatus = accountRequest.RelationshipStatus;
        account.UserLanguages = accountRequest.UserLanguages;
    }
    
    public static AccountShort ToAccountShort(this Account account)
    {
        var accountShort = new AccountShort()
        {
            Email = account.Email ?? string.Empty,
            FullName = account.FullName,
            ProfileImg = account.ImageUrl ?? string.Empty,
            Description = account.Bio ?? string.Empty,
        };

        return accountShort;
    }

}