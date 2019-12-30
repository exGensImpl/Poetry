using System;
using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
    /// <summary>
    /// Finds syllabic rhythm of sentences
    /// </summary>
    public sealed class RhythmFinder
    {
        private readonly IReadOnlyCollection<Word> m_words;

        public RhythmFinder(IReadOnlyCollection<Word> words)
        {
            m_words = words;
        }

        /// <summary>
        /// Returns syllabic rhythm of the specified sentence
        /// </summary>
        public Rhythm GetRhythm(string phrase)
        {
            var words = phrase.Split(" ");
            var rhythms = words.Select(GetWordRhythm).ToArray();
            return Rhythm.Concat(rhythms);
        }

        private Rhythm GetWordRhythm(string word)
        => m_words.FirstOrDefault(_ => _.Text.Equals(word, StringComparison.InvariantCultureIgnoreCase))?.Rhythm
        ?? throw new ArgumentException($"Vocabulary does not contain word {word}");
    }
}
