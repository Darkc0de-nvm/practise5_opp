using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;

public class ExchangeRates
{
    private Dictionary<string, decimal> rates = new();

    private ExchangeRates() { }

    private async Task InitAsync()
    {
        await Task.Delay(500); // імітація запиту

        rates = new Dictionary<string, decimal>
        {
            ["USD"] = 40m,
            ["EUR"] = 45m
        };
    }

    // фабричний метод
    public static async Task<ExchangeRates> CreateAsync()
    {
        var exchangeRates = new ExchangeRates();
        await exchangeRates.InitAsync();

        return exchangeRates;
    }

    public decimal Convert(decimal uah, string currency)
    {
        if (!rates.TryGetValue(currency, out var rate))
            throw new ArgumentException($"Unknown currency: {currency}", nameof(currency));

        return uah / rate;
    }
}

class Program
{
    static async Task Main()
    {
        var rates = await ExchangeRates.CreateAsync();

        WriteLine(rates.Convert(1000, "USD")); // 25
        WriteLine(rates.Convert(900, "EUR"));  // 20
    }
}