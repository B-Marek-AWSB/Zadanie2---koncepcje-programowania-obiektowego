import random
import time


class Card:
    def __init__(self, wartosc):
        self.wartosc = wartosc
        self.odkryta = False
        self.dopasowana = False


class Deck:
    def __init__(self, liczba_par):
        self.karty = []
        for i in range(liczba_par):
            litera = chr(65 + i)
            self.karty.append(Card(litera))
            self.karty.append(Card(litera))
        random.shuffle(self.karty)


class Game:
    def __init__(self, liczba_par):
        self.talia = Deck(liczba_par)
        self.ruchy = 0

    def wyswietl_plansze(self):
        print("\nPlansza:")
        for i in range(len(self.talia.karty)):
            karta = self.talia.karty[i]
            if karta.odkryta:
                tekst = "\033[1m" + karta.wartosc + "\033[0m"
                print("[" + tekst.rjust(2 + 8) + "]", end=" ")
            elif karta.dopasowana:
                tekst = karta.wartosc
                print("[" + tekst.rjust(2) + "]", end=" ")
            else:
                tekst = str(i + 1)
                print("[" + tekst.rjust(2) + "]", end=" ")
            if (i + 1) % 4 == 0:
                print()

    def graj(self):
        czas_start = time.time()
        liczba_par = len(self.talia.karty) // 2
        dopasowane_pary = 0

        while dopasowane_pary < liczba_par:
            self.wyswietl_plansze()

            nr1 = int(input("Wybierz pierwszą kartę (numer): ")) - 1
            if nr1 < 0 or nr1 >= len(self.talia.karty) or self.talia.karty[nr1].dopasowana:
                print("Nieprawidłowy wybór! Spróbuj ponownie.")
                continue
            self.talia.karty[nr1].odkryta = True

            nr2 = int(input("Wybierz drugą kartę (numer): ")) - 1
            if nr2 < 0 or nr2 >= len(self.talia.karty) or self.talia.karty[nr2].dopasowana or nr1 == nr2:
                print("Nieprawidłowy wybór! Spróbuj ponownie.")
                self.talia.karty[nr1].odkryta = False
                continue
            self.talia.karty[nr2].odkryta = True

            self.wyswietl_plansze()
            self.ruchy += 1

            if self.talia.karty[nr1].wartosc == self.talia.karty[nr2].wartosc:
                print("Para znaleziona!")
                self.talia.karty[nr1].dopasowana = True
                self.talia.karty[nr2].dopasowana = True
                self.talia.karty[nr1].odkryta = False
                self.talia.karty[nr2].odkryta = False
                dopasowane_pary += 1
            else:
                print("To nie para. Karty zostaną zakryte.")
                input("Naciśnij Enter aby kontynuować...")
                self.talia.karty[nr1].odkryta = False
                self.talia.karty[nr2].odkryta = False

        czas_calkowity = time.time() - czas_start
        self.wyswietl_plansze()
        print("\nGratulacje! Znalazłeś wszystkie pary!")
        print("Liczba ruchów:", self.ruchy)
        print("Czas gry:", round(czas_calkowity, 2), "sekund")

print("=== GRA MEMORY ===")
print("Plansza 4x4, 8 par do znalezienia.")
gra = Game(8)
gra.graj()
