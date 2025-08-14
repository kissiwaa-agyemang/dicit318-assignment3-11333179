using System;
using System.Collections.Generic;

// Define the Transaction record
public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

// Define the ITransactionProcessor interface
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Implement the BankTransferProcessor
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Bank Transfer: {transaction.Amount:C} for {transaction.Category}");
    }
}

// Implement the MobileMoneyProcessor
public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Mobile Money: {transaction.Amount:C} for {transaction.Category}");
    }
}

// Implement the CryptoWalletProcessor
public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Crypto Wallet: {transaction.Amount:C} for {transaction.Category}");
    }
}

// Define the base Account class
public class Account
{
    public string AccountNumber { get; private set; }
    protected decimal Balance { get; private set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

// Define the sealed SavingsAccount class
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance) { }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            base.ApplyTransaction(transaction);
            Console.WriteLine($"New balance: {Balance:C}");
        }
    }
}

// Define the FinanceApp class
public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        var savingsAccount = new SavingsAccount("123456", 1000m);

        // Sample transactions
        var transaction1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
        var transaction2 = new Transaction(2, DateTime.Now, 200m, "Utilities");
        var transaction3 = new Transaction(3, DateTime.Now, 900m, "Entertainment");

        // Process transactions
        var mobileMoneyProcessor = new MobileMoneyProcessor();
        mobileMoneyProcessor.Process(transaction1);
        savingsAccount.ApplyTransaction(transaction1);
        _transactions.Add(transaction1);

        var bankTransferProcessor = new BankTransferProcessor();
        bankTransferProcessor.Process(transaction2);
        savingsAccount.ApplyTransaction(transaction2);
        _transactions.Add(transaction2);

        var cryptoWalletProcessor = new CryptoWalletProcessor();
        cryptoWalletProcessor.Process(transaction3);
        savingsAccount.ApplyTransaction(transaction3);
        _transactions.Add(transaction3);
    }
}

// Main entry point
public class Program
{
    public static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}