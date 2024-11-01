using System.ComponentModel.DataAnnotations;

namespace Rise.Domain.DomainClasses
{
    public class UserTransaction : Entity
    {
        private int userId;
        private string type = null!;

        private static readonly HashSet<string> ValidTransactionTypes = new()
        {
            "ScanIn",
            "ScanOut",
            "AddStock",
        };

        public UserTransaction(int userId, string type)
        {
            SetUserId(userId);
            SetType(type);
        }

        private void SetUserId(int userId)
        {
            this.userId = Guard.Against.NegativeOrZero(userId, nameof(userId));
        }

        private void SetType(string type)
        {
            var validatedType = Guard.Against.NullOrWhiteSpace(type, nameof(type));
            if (!ValidTransactionTypes.Contains(validatedType))
            {
                throw new ArgumentException(
                    $"Invalid transaction type. Valid types are: {string.Join(", ", ValidTransactionTypes)}");
            }
            this.type = validatedType;
        }

        public int UserId => userId;
        public string Type => type;
    }
}
