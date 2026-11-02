using System;
using static System.Console;

public interface IButton
{
    string Render(string caption);
}

public interface ICheckbox
{
    string Render(string label, bool isChecked);
}

public interface IThemeFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}

// Світла тема
public class LightButton : IButton
{
    public string Render(string caption) => $"[light] [ {caption} ]";
}

public class LightCheckbox : ICheckbox
{
    public string Render(string label, bool isChecked) =>
        $"[light] [{(isChecked ? "x" : " ")}] {label}";
}

public class LightThemeFactory : IThemeFactory
{
    public IButton CreateButton() => new LightButton();
    public ICheckbox CreateCheckbox() => new LightCheckbox();
}

// Темна тема
public class DarkButton : IButton
{
    public string Render(string caption) => $"[dark] [ {caption} ]";
}

public class DarkCheckbox : ICheckbox
{
    public string Render(string label, bool isChecked) =>
        $"[dark] [{(isChecked ? "x" : " ")}] {label}";
}

public class DarkThemeFactory : IThemeFactory
{
    public IButton CreateButton() => new DarkButton();
    public ICheckbox CreateCheckbox() => new DarkCheckbox();
}

// Висококонтрастна тема
public class HighContrastButton : IButton
{
    public string Render(string caption) => $"[contrast] [[ {caption.ToUpper()} ]]";
}

public class HighContrastCheckbox : ICheckbox
{
    public string Render(string label, bool isChecked) =>
        $"[contrast] [{(isChecked ? "X" : "_")}] {label.ToUpper()}";
}

public class HighContrastThemeFactory : IThemeFactory
{
    public IButton CreateButton() => new HighContrastButton();
    public ICheckbox CreateCheckbox() => new HighContrastCheckbox();
}

public class SettingsDialog
{
    private readonly IThemeFactory factory;

    public SettingsDialog(IThemeFactory factory) => this.factory = factory;

    public void Show()
    {
        var button = factory.CreateButton();
        var checkbox = factory.CreateCheckbox();

        WriteLine(button.Render("Зберегти"));
        WriteLine(checkbox.Render("Сповіщення", true));
    }
}

class Program
{
    static void Main()
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        new SettingsDialog(new LightThemeFactory()).Show();
        new SettingsDialog(new DarkThemeFactory()).Show();
        new SettingsDialog(new HighContrastThemeFactory()).Show();
    }
}

// Який принцип SOLID це демонструє?
//
// Принцип відкритості/закритості (OCP): нову тему додаємо новими класами,
// а SettingsDialog не змінюється.
// Також принцип інверсії залежностей (DIP): діалог залежить від абстракцій.