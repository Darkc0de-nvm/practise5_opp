using System;
using System.Globalization;
using System.Linq;
using System.Text;
using static System.Console;


// Варіант А: окрема фабрика
namespace FactoryTasks.Task2A
{
    public class Color
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        internal Color(byte r, byte g, byte b) => (R, G, B) = (r, g, b);

        public override string ToString() => $"#{R:X2}{G:X2}{B:X2}";
    }

    public static class ColorFactory
    {
        public static Color FromRgb(byte r, byte g, byte b) => new Color(r, g, b);

        public static Color FromHsv(double h, double s, double v)
        {
            h = ((h % 360) + 360) % 360;
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
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
            if (hex is null) throw new ArgumentException("Hex is null", nameof(hex));

            string s = hex.StartsWith('#') ? hex[1..] : hex;
            if (s.Length != 6 ||
                !int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int value))
                throw new ArgumentException($"Invalid hex color: {hex}", nameof(hex));

            return new Color((byte)(value >> 16), (byte)(value >> 8), (byte)value);
        }

        private static byte ToByte(double x) => (byte)Math.Round(x * 255);
    }
}

// ПОРІВНЯННЯ ВАРІАНТІВ:
// Я б обрав варіант Б:
// private конструктор гарантує, що колір створюється лише через перевірені
// фабричні методи, навіть для коду всередині самої бібліотеки, а Color.Factory
// легко знайти, бо IDE підказує його одразу після "Color.".
// Варіант А кращий, коли фабрику треба підміняти, розширювати чи передавати
// як залежність, але ціною internal-конструктора, який захищає слабше за private.

// Варіант Б: внутрішня фабрика
namespace FactoryTasks.Task2B
{
    public class Color
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        private Color(byte r, byte g, byte b) => (R, G, B) = (r, g, b);

        public override string ToString() => $"#{R:X2}{G:X2}{B:X2}";

        public static class Factory
        {
            public static Color FromRgb(byte r, byte g, byte b) => new Color(r, g, b);

            public static Color FromHsv(double h, double s, double v)
            {
                h = ((h % 360) + 360) % 360;
                double c = v * s;
                double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
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
                if (hex is null) throw new ArgumentException("Hex is null", nameof(hex));

                string s = hex.StartsWith('#') ? hex[1..] : hex;
                if (s.Length != 6 ||
                    !int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int value))
                    throw new ArgumentException($"Invalid hex color: {hex}", nameof(hex));

                return new Color((byte)(value >> 16), (byte)(value >> 8), (byte)value);
            }

            private static byte ToByte(double x) => (byte)Math.Round(x * 255);
        }
    }
}

namespace FactoryTasks
{
    class Program
    {
        static void Main()
        {

            Console.OutputEncoding = Encoding.UTF8;

            WriteLine("2A: окрема фабрика:");
            WriteLine(Task2A.ColorFactory.FromHex("#00FF00")); // #00FF00
            WriteLine(Task2A.ColorFactory.FromHsv(32, 1, 1));  // #FF8800

            WriteLine("\n2Б: внутрішня фабрика:");
            WriteLine(Task2B.Color.Factory.FromHex("#00FF00")); // #00FF00
            WriteLine(Task2B.Color.Factory.FromHsv(32, 1, 1));  // #FF8800

            // Перевірка, що конструктор недоступний (розкоментувати перед цим):
            // var c = new Task2B.Color(0, 255, 0);
        }
    }
}