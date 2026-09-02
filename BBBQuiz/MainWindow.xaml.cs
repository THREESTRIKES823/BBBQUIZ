using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BBBQuiz
{
    public partial class MainWindow : Window
    {
        private List<Frage> fragen = new List<Frage>();
        private int aktuelleFrage = 0;
        private int punkte = 0;
        private Button[] knoepfe = Array.Empty<Button>();

        public MainWindow()
        {
            InitializeComponent();

            // Die vier Knoepfe in ein Array, damit wir sie mit einer
            // Schleife behandeln koennen statt viermal dasselbe zu schreiben.
            knoepfe = new[] { btnA, btnB, btnC, btnD };

            if (FragenLaden())
            {
                FrageAnzeigen();
            }
        }


        /// <summary>
        /// Liest die Datei fragen.json ein.
        /// Gibt false zurueck, wenn etwas schiefgeht.
        /// </summary>
        private bool FragenLaden()
        {
            string pfad = "fragen.json";

            if (!File.Exists(pfad))
            {
                MessageBox.Show(
                    "Die Datei fragen.json wurde nicht gefunden.\n\n" +
                    "Pruefe in Visual Studio: Rechtsklick auf fragen.json, " +
                    "Eigenschaften, 'In Ausgabeverzeichnis kopieren' auf " +
                    "'Kopieren, wenn neuer' stellen.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                string json = File.ReadAllText(pfad);

                var einstellungen = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                fragen = JsonSerializer.Deserialize<List<Frage>>(json, einstellungen)
                         ?? new List<Frage>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show(
                    "Die Datei fragen.json hat einen Formatfehler.\n\n" +
                    "Meist fehlt ein Komma oder eine Klammer.\n\n" +
                    "Details: " + ex.Message,
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (fragen.Count == 0)
            {
                MessageBox.Show("In fragen.json steht keine Frage.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Pruefen, ob jede Frage genau vier Antworten hat
            for (int i = 0; i < fragen.Count; i++)
            {
                if (fragen[i].Antworten.Count != 4)
                {
                    MessageBox.Show(
                        "Frage " + (i + 1) + " hat " + fragen[i].Antworten.Count +
                        " Antworten. Es muessen genau 4 sein.",
                        "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (fragen[i].RichtigeAntwort < 0 || fragen[i].RichtigeAntwort > 3)
                {
                    MessageBox.Show(
                        "Bei Frage " + (i + 1) + " ist RichtigeAntwort = " +
                        fragen[i].RichtigeAntwort + ".\n\n" +
                        "Erlaubt sind nur 0, 1, 2 oder 3. Die Zaehlung beginnt bei 0!",
                        "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Zeigt die aktuelle Frage an und setzt die Knoepfe zurueck.
        /// </summary>
        private void FrageAnzeigen()
        {
            Frage f = fragen[aktuelleFrage];

            txtFrage.Text = f.Text;

            for (int i = 0; i < 4; i++)
            {
                knoepfe[i].Content = f.Antworten[i];
                knoepfe[i].Background = (SolidColorBrush)FindResource("Primaer");
                knoepfe[i].IsEnabled = true;
            }

            txtErklaerung.Visibility = Visibility.Collapsed;
            txtPunkte.Text = punkte + " Punkte";
            txtFortschritt.Text = "Frage " + (aktuelleFrage + 1) + " von " + fragen.Count;
        }


        /// <summary>
        /// Wird aufgerufen, wenn auf einen der vier Knoepfe geklickt wird.
        /// Welcher es war, steht in der Eigenschaft Tag.
        /// </summary>
        private void Antwort_Click(object sender, RoutedEventArgs e)
        {
            Button geklickt = (Button)sender;
            int gewaehlt = int.Parse(geklickt.Tag.ToString()!);
            Frage f = fragen[aktuelleFrage];

            // Alle Knoepfe sperren, damit niemand zweimal klickt
            foreach (Button b in knoepfe)
            {
                b.IsEnabled = false;
            }

            if (gewaehlt == f.RichtigeAntwort)
            {
                geklickt.Background = (SolidColorBrush)FindResource("Richtig");
                punkte++;
            }
            else
            {
                geklickt.Background = (SolidColorBrush)FindResource("Falsch");
                // Die richtige Antwort trotzdem gruen zeigen
                knoepfe[f.RichtigeAntwort].Background = (SolidColorBrush)FindResource("Richtig");
            }

            txtPunkte.Text = punkte + " Punkte";

            if (!string.IsNullOrWhiteSpace(f.Erklaerung))
            {
                txtErklaerung.Text = f.Erklaerung;
                txtErklaerung.Visibility = Visibility.Visible;
            }

            WeiterNach(2.0);
        }


        /// <summary>
        /// Wartet die angegebenen Sekunden und geht dann zur naechsten Frage.
        /// </summary>
        private void WeiterNach(double sekunden)
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(sekunden);
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                aktuelleFrage++;

                if (aktuelleFrage < fragen.Count)
                {
                    FrageAnzeigen();
                }
                else
                {
                    Auswertung();
                }
            };
            timer.Start();
        }


        /// <summary>
        /// Zeigt das Ergebnis und fragt, ob nochmals gespielt wird.
        /// </summary>
        private void Auswertung()
        {
            string text = "Fertig!\n\n" +
                          punkte + " von " + fragen.Count + " richtig.\n\n" +
                          "Nochmals spielen?";

            var antwort = MessageBox.Show(text, "Ergebnis",
                MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (antwort == MessageBoxResult.Yes)
            {
                aktuelleFrage = 0;
                punkte = 0;
                FrageAnzeigen();
            }
            else
            {
                Close();
            }
        }
    }
}
