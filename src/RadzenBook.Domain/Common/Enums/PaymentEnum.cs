namespace RadzenBook.Domain.Common.Enums;

public enum PaymentMethod
{
    Cash,
    CreditCard,
    BankTransfer
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Cancelled
}