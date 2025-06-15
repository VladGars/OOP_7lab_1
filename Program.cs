using System;
using System.Globalization;

CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

Console.WriteLine("## Тест 1: Спроба створення вкладу з негативною сумою ##");
try
{
    Deposit invalidDeposit = new OnDemandDeposit("Петренко П. П.", -500);
}
catch (VkladException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Помилка конструктора: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("Виняток VkladException було успішно перехоплено.\n");
}

Console.WriteLine(new string('-', 50));

Console.WriteLine("## Тест 2: Розрахунок суми вкладу ##");

LongTermDeposit validDeposit = new LongTermDeposit("Іваненко І. І.", 20000);

Console.WriteLine("Спроба розрахунку для коректної кількості місяців (12):");
validDeposit.CalculateDepositAmount(12);

Console.WriteLine("\nСпроба розрахунку для некоректної кількості місяців (-5):");
validDeposit.CalculateDepositAmount(-5);


public class VkladException : Exception
{
    public VkladException(string message) : base(message) { }
}

public class KilkistException : Exception
{
    public KilkistException(string message) : base(message) { }
}

public class Bank
{
    public string Name { get; set; }
}

public class Branch
{
    public string Name { get; set; }
    public decimal TotalDeposits { get; set; }
}

public abstract class Deposit
{
    public string DepositorFullName { get; set; }
    public decimal Amount { get; set; }

    public Deposit(string fullName, decimal amount)
    {
        if (amount < 0)
        {
            string errorMessage = $"Неможливо створити внесок – вказана негативна сума вкладу: {amount:C}";
            throw new VkladException(errorMessage);
        }

        DepositorFullName = fullName;
        Amount = amount;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Створено вклад для '{DepositorFullName}' на суму {Amount:C}.");
        Console.ResetColor();
    }

    public abstract void CalculateDepositAmount(int months);
}

public class LongTermDeposit : Deposit
{
    public LongTermDeposit(string fullName, decimal amount) : base(fullName, amount) { }

    public override void CalculateDepositAmount(int months)
    {
        try
        {
            if (months < 0)
            {
                throw new KilkistException($"Помилка: кількість місяців не може бути негативною (вказано: {months}).");
            }

            decimal interestRate = 0.12m;
            decimal finalAmount = Amount * (1 + interestRate / 12 * months);

            Console.WriteLine($"Розрахунок для довгострокового вкладу: через {months} міс. сума складе {finalAmount:C}");
        }
        catch (KilkistException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Перехоплено виняток KilkistException: {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Сталася неочікувана помилка: {ex.Message}");
            Console.ResetColor();
        }
    }
}

public class OnDemandDeposit : Deposit
{
    public OnDemandDeposit(string fullName, decimal amount) : base(fullName, amount) { }

    public override void CalculateDepositAmount(int months)
    {
        try
        {
            if (months < 0)
            {
                throw new KilkistException($"Помилка: кількість місяців не може бути негативною (вказано: {months}).");
            }

            decimal interestRate = 0.015m;
            decimal finalAmount = Amount * (1 + interestRate / 12 * months);

            Console.WriteLine($"Розрахунок для вкладу на вимогу: через {months} міс. сума складе {finalAmount:C}");
        }
        catch (KilkistException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Перехоплено виняток KilkistException: {ex.Message}");
            Console.ResetColor();
        }
    }
}