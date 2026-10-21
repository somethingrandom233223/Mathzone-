using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MathZone;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<GameItem> allGames = new();
    private string currentPage = "home";

    public MainWindow()
    {
        InitializeComponent();
        AddGame("Apex Legends", "Heroisk action og lagspill", "GRATIS", "▶", "#5A2531", "BUTIKK");
        AddGame("Rocket League", "Biler møter fotball", "EIER DU?", "⚽", "#173D62", "BIBLIOTEK");
        AddGame("Minecraft", "Bygg, utforsk og skap", "EGEN KILDE", "▧", "#2E6347", "BIBLIOTEK");
        AddGame("Stardew Valley", "Rolig gårdsliv og eventyr", "14,99 €", "✿", "#6C5638", "BUTIKK");
        AddGame("Trackmania", "Racing og banebygging", "GRATIS", "🏁", "#4D4A80", "BUTIKK");
        AddGame("Eget spill", "Legg til en exe-fil fra PC-en", "DIN PC", "+", "#3B4050", "VERKTØY");
        GamesList.ItemsSource = allGames;
    }

    private void AddGame(string name, string description, string price, string symbol, string color, string tag)
    {
        allGames.Add(new GameItem { Name=name, Description=description, Price=price, Symbol=symbol, Color=(Brush)new BrushConverter().ConvertFromString(color)!, Tag=tag, Action=tag=="BIBLIOTEK" ? "Start" : tag=="VERKTØY" ? "Legg til" : "Se spill" });
    }

    private void ShowPage(string page, string title)
    {
        currentPage = page;
        SectionTitle.Text = title;
        var items = page switch {
            "store" => allGames.Where(g => g.Tag == "BUTIKK" || g.Tag == "VERKTØY"),
            "library" => allGames.Where(g => g.Tag == "BIBLIOTEK" || g.Tag == "VERKTØY"),
            "downloads" => Enumerable.Empty<GameItem>(),
            _ => allGames
        };
        GamesList.ItemsSource = items.ToList();
        CountLabel.Text = page == "downloads" ? "Ingen aktive nedlastinger" : $"{items.Count()} spill";
        if (page == "downloads") {
            GamesList.ItemsSource = new List<GameItem> { new GameItem { Name="Ingen nedlastinger", Description="Når du kobler til ekte spillinstallasjoner, vises status her.", Price="—", Symbol="⇩", Color=(Brush)new BrushConverter().ConvertFromString("#303548")!, Tag="STATUS", Action="OK" } };
        }
    }

    private void Home_Click(object sender, RoutedEventArgs e) => ShowPage("home", "Anbefalt for deg");
    private void Store_Click(object sender, RoutedEventArgs e) => ShowPage("store", "Spillbutikk");
    private void Library_Click(object sender, RoutedEventArgs e) => ShowPage("library", "Spillbibliotek");
    private void Downloads_Click(object sender, RoutedEventArgs e) => ShowPage("downloads", "Nedlastinger");

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (GamesList == null || allGames.Count == 0) return;
        var q = SearchBox.Text.Trim();
        var filtered = allGames.Where(g => g.Name.Contains(q, StringComparison.OrdinalIgnoreCase) || g.Description.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        GamesList.ItemsSource = filtered;
        CountLabel.Text = $"{filtered.Count} treff";
    }

    private void GameAction_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not string name) return;
        var game = allGames.FirstOrDefault(g => g.Name == name);
        if (game == null) return;
        if (game.Name == "Eget spill")
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Title="Velg spillets .exe-fil", Filter="Programfiler (*.exe)|*.exe", CheckFileExists=true };
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                try { Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }); }
                catch (Exception ex) { MessageBox.Show($"Kunne ikke starte spillet: {ex.Message}", "MathZone"); }
            }
        }
        else if (game.Tag == "BIBLIOTEK")
        {
            MessageBox.Show($"For å starte {game.Name}, må du koble til den installerte spillfilen (.exe) først.\n\nBruk «Eget spill» for å velge et spill fra PC-en.", "MathZone");
        }
        else
        {
            MessageBox.Show($"{game.Name} er et eksempel i butikkvisningen.\n\nDenne startpakken har ikke en ekte spillbutikk eller nedlastingsserver koblet til ennå.", "MathZone");
        }
    }
}

public class GameItem
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Price { get; set; } = "";
    public string Symbol { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Action { get; set; } = "";
    public Brush Color { get; set; } = Brushes.SlateGray;
}
