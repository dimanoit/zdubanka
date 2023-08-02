using Application.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Services;

public class AccountServiceTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAccountService _sut;

    public AccountServiceTests()
    {
        _dbContext = ApplicationDbContextFactory.Create();
        _sut = new AccountService(_dbContext);
    }

    [Fact]
    public async Task AddAsync_Should_CreateAccount()
    {
        // Arrange 
        var expected = AccountFaker.Create();

        // Act 
        await _sut.AddAsync(expected);

        // Assert
        var actual = await _dbContext.Accounts.FindAsync(expected.Id);
        expected.Id.Should().Be(actual!.Id);
    }

    [Fact]
    public async Task UpdateAccountAsync_Should_UpdateAccount()
    {
        // Arrange 
        var expected = await CreateAccount();
        var newFullName = new Faker().Person.FirstName;
        expected.FullName = newFullName;

        // Act
        await _sut.UpdateAccountAsync(expected);

        // Assert
        var actual = await _dbContext.Accounts.FindAsync(expected.Id);
        actual.FullName.Should().Be(newFullName);
    }

    [Fact]
    public async Task SaveRefreshTokenAsync_Should_SaveRefreshToken()
    {
        // Arrange 
        var expected = await CreateAccount();
        var newRefreshToken = Guid.NewGuid().ToString();

        // Act
        await _sut.SaveRefreshTokenAsync(expected, newRefreshToken, DateTime.Today);

        // Assert
        var actual = await _dbContext.Accounts.FindAsync(expected.Id);
        actual.RefreshToken.Should().Be(newRefreshToken);
        actual.RefreshTokenExpiryTime.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task DeleteAccountAsync_Should_DeleteAccount()
    {
        // Arrange 
        var expected = await CreateAccount();

        // Act
        await _sut.DeleteAccountAsync(expected);

        // Assert
        var actual = await _dbContext.Accounts.FindAsync(expected.Id);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetAccountByIdAsync_Should_ReturnAccount() =>
        await GetAccountAsync_Should_ReturnAccount(expected => _sut.GetAccountByIdAsync(expected.Id, default));

    [Fact]
    public async Task GetAccountByEmailAsync_Should_ReturnAccount() =>
        await GetAccountAsync_Should_ReturnAccount(expected => _sut.GetAccountByEmailAsync(expected.Email, default));

    private async Task GetAccountAsync_Should_ReturnAccount(
        Func<Account, Task<Account?>> getAccountFunc)
    {
        // Arrange
        var expected = await CreateAccount();

        // Act
        var actual = await getAccountFunc.Invoke(expected);

        // Assert
        actual.Id.Should().Be(expected.Id);
    }

    private async Task<Account> CreateAccount()
    {
        var expected = AccountFaker.Create();
        _dbContext.Accounts.Add(expected);
        await _dbContext.SaveChangesAsync();
        return expected;
    }
}