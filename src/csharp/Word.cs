using System;
using System.Linq;

namespace ExGens.Poetry
{
    /// <summary>
    /// Represents a word with its syllabic rhythm
    /// </summary>
    public sealed class Word
    {
        /// <summary>
        /// Text representation of the word
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Syllabic rhythm of the word
        /// </summary>
        public Rhythm Rhythm { get; }

        public Word(string text, Rhythm rhythm)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("A word should not be empty");
            }
            if (text.Any(char.IsLetter) == false)
            {
                throw new ArgumentException($"A word should contain at least one letter. Given: {text}");
            }

            Text = text;
            Rhythm = rhythm;
        }

        public override string ToString() => $"{Text} ({Rhythm})";
    }
}
