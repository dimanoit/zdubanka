using Domain.Entities;
using Domain.Requests;

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
}