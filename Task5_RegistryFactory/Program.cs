using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

public interface INotifier
{
    void Send(string to, string message);
}

public class EmailNotifier : INotifier
{
    public void Send(string to, string message) =>
        WriteLine($"Email to {to}: {message}");
}

public class SmsNotifier : INotifier
{
    public void Send(string to, string message) =>
        WriteLine($"SMS to {to}: {message}");
}

public class TelegramNotifier : INotifier
{
    public void Send(string to, string message) =>
        WriteLine($"Telegram to {to}: {message}");
}

public class NotifierFactory
{
    private readonly Dictionary<string, Func<INotifier>> creators =
        new(StringComparer.OrdinalIgnoreCase);

    public void Register(string channel, Func<INotifier> creator)
    {
        if (string.IsNullOrWhiteSpace(channel))
            throw new ArgumentException("Channel name must not be empty.", nameof(channel));
        if (creator is null)
            throw new ArgumentNullException(nameof(creator));

        if (!creators.TryAdd(channel, creator))
            throw new ArgumentException($"Channel already registered: {channel}", nameof(channel));
    }

    public INotifier Create(string channel)
    {
        if (!creators.TryGetValue(channel, out var creator))
            throw new ArgumentException($"Unknown channel: {channel}", nameof(channel));

        return creator();
    }
}

class Program
{
    static void Main()
    {

        Console.OutputEncoding = Encoding.UTF8;

        var factory = new NotifierFactory();
        factory.Register("email", () => new EmailNotifier());
        factory.Register("sms", () => new SmsNotifier());
        factory.Register("telegram", () => new TelegramNotifier());

        factory.Create("Telegram").Send("@denys", "Лекція о 10:00");
        // Telegram to @denys: Лекція о 10:00

        factory.Create("EMAIL").Send("denys@example.com", "Привіт");
        // Email to denys@example.com: Привіт

        try { factory.Create("viber"); }
        catch (ArgumentException ex) { WriteLine(ex.Message); }
        // Unknown channel: viber (Parameter 'channel')

        try { factory.Register("SMS", () => new SmsNotifier()); }
        catch (ArgumentException ex) { WriteLine(ex.Message); }
        // Channel already registered: SMS (Parameter 'channel')
    }
}