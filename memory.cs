using System;
using System.Collections.Generic;
using System.Diagnostics;

class Card
{
    public string Wartosc;
    public bool Odkryta;
    public bool Dopasowana;

    public Card(string wartosc)
    {
        Wartosc = wartosc;
        Odkryta = false;
        Dopasowana = false;
    }
}

class Deck
{
    public List<Card> Karty;

    public Deck(int liczbaPar, int seed)
    {
        Karty = new List<Card>();
        for (int i = 0; i < liczbaPar; i++)
        {
            string litera = ((char)(65 + i)).ToString();
            Karty.Add(new Card(litera));
            Karty.Add(new Card(litera));
        }
        Random random = new Random(seed);
        for (int i = Karty.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Card temp = Karty[i];
            Karty[i] = Karty[j];
            Karty[j] = temp;
        }
    }
}

class Game
{
    public Deck Talia;
    public int Ruchy;

    public Game(int liczbaPar, int seed)
    {
        Talia = new Deck(liczbaPar, seed);
        Ruchy = 0;
    }

    public void WyswietlPlansze()
    {
        Console.WriteLine("\nPlansza:");
        for (int i = 0; i < Talia.Karty.Count; i++)
        {
            Card karta = Talia.Karty[i];
            if (karta.Odkryta)
            {
                string tekst = "*" + karta.Wartosc + "*";
                Console.Write("[" + tekst.PadLeft(3) + "] ");
            }
            else if (karta.Dopasowana)
            {
                string tekst = " " + karta.Wartosc + " ";
                Console.Write("[" + tekst + "] ");
            }
            else
            {
                string tekst = (i + 1).ToString();
                Console.Write("[" + tekst.PadLeft(3) + "] ");
            }
            if ((i + 1) % 4 == 0)
            {
                Console.WriteLine();
            }
        }
    }

    public void Graj()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int liczbaPar = Talia.Karty.Count / 2;
        int dopasowanePary = 0;

        while (dopasowanePary < liczbaPar)
        {
            WyswietlPlansze();

            Console.Write("Wybierz pierwszą kartę (numer): ");
            int nr1 = int.Parse(Console.ReadLine()) - 1;
            if (nr1 < 0 || nr1 >= Talia.Karty.Count || Talia.Karty[nr1].Dopasowana)
            {
                Console.WriteLine("Nieprawidłowy wybór! Spróbuj ponownie.");
                continue;
            }
            Talia.Karty[nr1].Odkryta = true;

            Console.Write("Wybierz drugą kartę (numer): ");
            int nr2 = int.Parse(Console.ReadLine()) - 1;
            if (nr2 < 0 || nr2 >= Talia.Karty.Count || Talia.Karty[nr2].Dopasowana || nr1 == nr2)
            {
                Console.WriteLine("Nieprawidłowy wybór! Spróbuj ponownie.");
                Talia.Karty[nr1].Odkryta = false;
                continue;
            }
            Talia.Karty[nr2].Odkryta = true;

            WyswietlPlansze();
            Ruchy++;

            if (Talia.Karty[nr1].Wartosc == Talia.Karty[nr2].Wartosc)
            {
                Console.WriteLine("Para znaleziona!");
                Talia.Karty[nr1].Dopasowana = true;
                Talia.Karty[nr2].Dopasowana = true;
                Talia.Karty[nr1].Odkryta = false;
                Talia.Karty[nr2].Odkryta = false;
                dopasowanePary++;
            }
            else
            {
                Console.WriteLine("To nie para. Karty zostaną zakryte.");
                Console.Write("Naciśnij Enter aby kontynuować...");
                Console.ReadLine();
                Talia.Karty[nr1].Odkryta = false;
                Talia.Karty[nr2].Odkryta = false;
            }
        }

        stopwatch.Stop();
        double czasCalkowity = stopwatch.Elapsed.TotalSeconds;
        WyswietlPlansze();
        Console.WriteLine("\nGratulacje! Znalazłeś wszystkie pary!");
        Console.WriteLine("Liczba ruchów: " + Ruchy);
        Console.WriteLine("Czas gry: " + Math.Round(czasCalkowity, 2) + " sekund");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== GRA MEMORY ===");
        Console.WriteLine("Plansza 4x4, 8 par do znalezienia.");
        Console.Write("Podaj dowolną liczbę (seed gry): ");
        int seed = int.Parse(Console.ReadLine());
        Game gra = new Game(8, seed);
        gra.Graj();
    }
}
