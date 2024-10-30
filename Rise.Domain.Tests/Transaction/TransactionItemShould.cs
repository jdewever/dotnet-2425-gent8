using Rise.Domain.DomainClasses;
using Shouldly;

namespace Rise.Domain.Tests.Transactions;

public class TransactionItemShould
{
    [Fact]
    public void BeCreatedWithValidData()
    {
        var item = new TransactionItem(
            transactionId: 1,
            productId: 2,
            quantity: 3
        );

        item.TransactionID.ShouldBe(1);
        item.ProductID.ShouldBe(2);
        item.Quantity.ShouldBe(3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NotBeCreatedWithInvalidQuantity(int quantity)
    {
        Action act = () => new TransactionItem(
            transactionId: 1,
            productId: 2,
            quantity: quantity
        );

        act.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NotBeCreatedWithInvalidTransactionId(int transactionId)
    {
        Action act = () => new TransactionItem(
            transactionId: transactionId,
            productId: 2,
            quantity: 1
        );

        act.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NotBeCreatedWithInvalidProductId(int productId)
    {
        Action act = () => new TransactionItem(
            transactionId: 1,
            productId: productId,
            quantity: 1
        );

        act.ShouldThrow<ArgumentException>();
    }
}