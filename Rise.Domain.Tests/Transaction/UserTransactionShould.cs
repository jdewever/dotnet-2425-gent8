using Rise.Domain.DomainClasses;
using Shouldly;

namespace Rise.Domain.Tests.Transaction;

public class UserTransactionShould
{
    [Theory]
    [InlineData("ScanIn")]
    [InlineData("ScanOut")]
    [InlineData("AddStock")]
    public void BeCreatedWithValidData(string type)
    {
        var transaction = new UserTransaction(
            userId: "1",
            type: type
        );

        transaction.UserId.ShouldBe("1");
        transaction.Type.ShouldBe(type);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NotBeCreatedWithInvalidUserId(string userId)
    {
        Action act = () => new UserTransaction(
            userId: userId,
            type: "ScanIn"
        );

        act.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("InvalidType")]
    public void NotBeCreatedWithInvalidType(string? type)
    {
        Action act = () => new UserTransaction(
            userId: "1",
            type: type!
        );

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void NotBeCreatedWithInvalidTransactionType()
    {
        Action act = () => new UserTransaction(
            userId: "1",
            type: "InvalidType"
        );

        act.ShouldThrow<ArgumentException>()
            .Message.ShouldContain("Invalid transaction type. Valid types are: ScanIn, ScanOut, AddStock");
    }
}