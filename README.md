# BBB-Quiz

Windows-App in C# mit eigenem Design. Ein Quiz über die Berufsfachschule BBB.

Projektarbeit von vier Lernenden über drei Tage: zwei Plattformentwickler, zwei Mediamatiker.

---

## Was man braucht

| Was | Version |
|---|---|
| Visual Studio 2022 | mit Arbeitslast **.NET-Desktopentwicklung** |
| .NET SDK | 8.0 |
| Betriebssystem | Windows |

Läuft nicht auf macOS oder Linux. WPF gibt es nur für Windows.

---

## Starten

```
git clone https://github.com/BENUTZERNAME/bbb-quiz.git
cd bbb-quiz
```

Dann `BBBQuiz/BBBQuiz.csproj` in Visual Studio öffnen und F5 drücken.

Oder ohne Visual Studio:

```
cd BBBQuiz
dotnet run
```

---

## Ordner

```
bbb-quiz/
├── README.md
├── .gitignore
└── BBBQuiz/
    ├── BBBQuiz.csproj        Projektdatei
    ├── App.xaml              Meldet Design.xaml an
    ├── App.xaml.cs
    ├── MainWindow.xaml       Die Oberfläche
    ├── MainWindow.xaml.cs    Die Logik
    ├── Design.xaml           Alle Farben, Schriften, Grössen
    ├── Frage.cs              Datenmodell einer Frage
    ├── fragen.json           Die Fragen
    ├── Fonts/                Schriftdateien (.ttf)
    └── Bilder/               Logo und Icons (.png)
```

---

## Für die Mediamatiker

Ihr müsst kein Visual Studio öffnen. Ihr arbeitet an drei Stellen.

### 1. Farben ändern

Datei `BBBQuiz/Design.xaml` mit einem Texteditor öffnen. Oben stehen die Farben:

```xml
<SolidColorBrush x:Key="Hintergrund"  Color="#F4F1DE"/>
<SolidColorBrush x:Key="Primaer"      Color="#2D6A4F"/>
```

Nur den Hex-Code hinter `Color=` ändern. Den Namen bei `x:Key` **nicht** ändern, sonst findet das Programm die Farbe nicht mehr.

| Name | Wofür |
|---|---|
| `Hintergrund` | Hintergrund des Fensters |
| `Primaer` | Antwort-Knöpfe im Normalzustand |
| `Richtig` | Knopf bei richtiger Antwort |
| `Falsch` | Knopf bei falscher Antwort |
| `Textfarbe` | Fragetext und Punktestand |
| `TextAufKnopf` | Text auf den Knöpfen |

### 2. Schrift einbauen

1. Die `.ttf`-Datei in den Ordner `BBBQuiz/Fonts/` legen.
2. Doppelklick auf die Datei. Oben steht der **interne Schriftname**. Das ist nicht der Dateiname.
3. In `Design.xaml` die Zeile ändern:

```xml
<FontFamily x:Key="Hausschrift">/Fonts/#Montserrat</FontFamily>
```

Aufbau: `/Ordner/#InternerName`

Nur Schriften verwenden, die frei nutzbar sind. Google Fonts ist auf der sicheren Seite.

### 3. Fragen schreiben

Datei `BBBQuiz/fragen.json` mit einem Texteditor öffnen.

```json
{
  "Text": "Hier steht die Frage?",
  "Antworten": [
    "Erste Antwort",
    "Zweite Antwort",
    "Dritte Antwort",
    "Vierte Antwort"
  ],
  "RichtigeAntwort": 1,
  "Erklaerung": "Erscheint nach der Antwort. Kann leer bleiben."
}
```

**Wichtig: `RichtigeAntwort` zählt ab 0.**

| Richtige Antwort | Wert |
|---|---|
| erste | `0` |
| zweite | `1` |
| dritte | `2` |
| vierte | `3` |

Regeln:

- Genau vier Antworten pro Frage. Nicht drei, nicht fünf.
- Zwischen den Fragen steht ein Komma, nach der letzten keins.
- Anführungszeichen im Text mit `\"` schreiben.

Wenn die Datei einen Fehler hat, zeigt die App beim Start eine Meldung mit dem Grund.

### 4. Logo einbauen

1. Logo als `.png` mit transparentem Hintergrund in `BBBQuiz/Bilder/` legen, Name `logo.png`.
2. In `MainWindow.xaml` den Platzhalter-`TextBlock` löschen und die kommentierte `Image`-Zeile freischalten. Dafür braucht es einen Informatiker.

---

## Design-Vorgaben

Diese Werte legen die Mediamatiker fest und die Informatiker bauen sie ein.

| Was | Wert | Format |
|---|---|---|
| Hintergrund | offen | Hex-Code |
| Hauptfarbe | offen | Hex-Code |
| Farbe richtig | offen | Hex-Code |
| Farbe falsch | offen | Hex-Code |
| Schrift | offen | `.ttf` oder `.otf` |
| Schriftgrösse Frage | 28 px | Zahl |
| Schriftgrösse Antwort | 18 px | Zahl |
| Ecken abgerundet | 8 px | Zahl |
| Rand aussen | 48 px | Zahl |
| Logo | offen | `.png`, ca. 500 px breit, transparent |
| Icons | offen | `.png`, 128×128 px, transparent |

---

## Was nicht dazugehört

Diese Punkte bauen wir ausdrücklich **nicht**:

- Login oder Benutzerverwaltung
- Datenbank oder Server
- Rangliste über mehrere Geräte
- Handy- oder Web-Version
- Editor zum Bearbeiten der Fragen im Programm
- Übersetzung in andere Sprachen
- Installationsprogramm

Für drei Tage zu umfangreich.

---

## Häufige Fehler

| Fehler | Ursache | Lösung |
|---|---|---|
| «fragen.json wurde nicht gefunden» | Datei wird nicht kopiert | Eigenschaften: «In Ausgabeverzeichnis kopieren» = «Kopieren, wenn neuer» |
| «Formatfehler» beim Start | Komma oder Klammer fehlt | Datei auf jsonlint.com prüfen |
| Schrift wird nicht angezeigt | Interner Name falsch | Doppelklick auf `.ttf`, Namen oben ablesen |
| Falsche Antwort gilt als richtig | Ab 1 gezählt | `RichtigeAntwort` zählt ab 0 |
| Projekt lässt sich nicht öffnen | Arbeitslast fehlt | Visual Studio Installer: «.NET-Desktopentwicklung» nachinstallieren |

---

## Team

| Rolle | Aufgabe |
|---|---|
| Plattformentwickler ×2 | Programmierung, Einbau des Designs |
| Mediamatiker ×2 | Design, Logo, Fragen, Präsentation |
