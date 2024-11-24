using Rise.Domain.DomainClasses;
using Shouldly;

namespace Rise.Domain.Tests.Transaction;

public class TransactionItemShould
{
    private readonly UserTransaction _userTransaction;
    private readonly Product _product;

    public TransactionItemShould()
    {
        _userTransaction = new UserTransaction();
        _product = new Product();
    }
    [Fact]
    public void BeCreatedWithValidData()
    {
        var item = new TransactionItem(
            product: _product,
            transaction: _userTransaction,
            quantity: 3
        );

        item.Transaction.ShouldBe(_userTransaction);
        item.Product.ShouldBe(_product);
        item.Quantity.ShouldBe(3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NotBeCreatedWithInvalidQuantity(int quantity)
    {
        Action act = () => new TransactionItem(
            product: _product,
            transaction: _userTransaction,
            quantity: quantity
        );

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void NotBeCreatedWithInvalidTransactionId()
    {
        Action act = () => new TransactionItem(
            product: _product,
            transaction: null!,
            quantity: 1
        );

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void NotBeCreatedWithInvalidProductId()
    {
        Action act = () => new TransactionItem(
            transaction: _userTransaction,
            product: null!,
            quantity: 1
        );

        act.ShouldThrow<ArgumentException>();
    }
}