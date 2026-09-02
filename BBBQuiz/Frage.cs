using System.Collections.Generic;

namespace BBBQuiz
{
    /// <summary>
    /// Eine einzelne Quizfrage.
    /// Die Namen der Eigenschaften muessen genau mit den Namen
    /// in der Datei fragen.json uebereinstimmen.
    /// </summary>
    public class Frage
    {
        public string Text { get; set; } = "";

        public List<string> Antworten { get; set; } = new List<string>();

        /// <summary>
        /// Nummer der richtigen Antwort. Die Zaehlung beginnt bei 0.
        /// 0 = erste Antwort, 1 = zweite, 2 = dritte, 3 = vierte.
        /// </summary>
        public int RichtigeAntwort { get; set; }

        /// <summary>
        /// Optionale Erklaerung, die nach der Antwort erscheint.
        /// Kann leer bleiben.
        /// </summary>
        public string Erklaerung { get; set; } = "";
    }
}
