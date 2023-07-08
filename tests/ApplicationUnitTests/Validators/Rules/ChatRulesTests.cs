using ApplicationUnitTests.Helpers;
using Xunit;

namespace ApplicationUnitTests.Validators.Rules;

public class ChatRulesTests
{

    [Fact]
    public async Task GetIsUserMemberOfChat_ShouldBe_MemberOfChat()
    {
        // Arrange 
        var dbContext = ApplicationDbContextFactory.Create();
        
        
    }
    
}