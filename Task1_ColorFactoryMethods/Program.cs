using System;
using static System.Console;

public class Color
{
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    private Color(byte r, byte g, byte b)
    {
        R = r; G = g; B = b;
    }

    public static Color FromRgb(byte r, byte g, byte b) => new Color(r, g, b);

    public static Color FromHsv(double h, double s, double v)
    {
        if (s < 0 || s > 1) throw new ArgumentOutOfRangeException(nameof(s), "Saturation must be in [0, 1].");
        if (v < 0 || v > 1) throw new ArgumentOutOfRangeException(nameof(v), "Value must be in [0, 1].");
        if (h < 0 || h > 360) throw new ArgumentOutOfRangeException(nameof(h), "Hue must be in [0, 360].");

        h %= 360;

        double c = v * s;
        double x = c * (1 - Math.Abs(h / 60 % 2 - 1));
        double m = v - c;

        (double r, double g, double b) = (int)(h / 60) switch
        {
            0 => (c, x, 0.0),
            1 => (x, c, 0.0),
            2 => (0.0, c, x),
            3 => (0.0, x, c),
            4 => (x, 0.0, c),
            _ => (c, 0.0, x)
        };

        return new Color(ToByte(r + m), ToByte(g + m), ToByte(b + m));
    }

    public static Color FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            throw new ArgumentException("Hex string is empty.", nameof(hex));

        string s = hex.Trim();
        if (s.StartsWith('#')) s = s[1..];

        if (s.Length != 6 || !s.All(Uri.IsHexDigit))
            throw new ArgumentException($"Invalid hex color: \"{hex}\".", nameof(hex));

        return new Color(
            Convert.ToByte(s[0..2], 16),
            Convert.ToByte(s[2..4], 16),
            Convert.ToByte(s[4..6], 16));
    }

    private static byte ToByte(double unit) => (byte)Math.Round(unit * 255);

    public override string ToString() => $"#{R:X2}{G:X2}{B:X2}";
}

class Program
{
    static void Main()
    {
        WriteLine(Color.FromRgb(255, 136, 0)); // #FF8800
        WriteLine(Color.FromHsv(32, 1, 1));    // #FF8800
        WriteLine(Color.FromHex("ff8800"));    // #FF8800
        WriteLine(Color.FromHex("#Ff8800"));   // #FF8800

        try { Color.FromHex("#GG0000"); }
        catch (ArgumentException e) { WriteLine(e.Message); }
    }
}