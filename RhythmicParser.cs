using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExGens.Poetry
{
    /// <summary>
    /// Splits sentences to words and finds its syllabic rhythm
    /// </summary>
    public sealed class RhythmicParser
    {
        private readonly IReadOnlyCollection<Word> m_words;

        private readonly Regex m_phraseRegex;

        /// <summary>
        /// Initializes a new instanc of rhythmic parser by specified list of allowed words 
        /// </summary>
        public RhythmicParser(IReadOnlyCollection<Word> words)
        {
            m_words = words;

            var allowedNonLetters = words.SelectMany(_ => _.Text)
                                         .Distinct()
                                         .Where(_ => char.IsLetter(_) == false)
                                         .Select(_ => _.ToString())
                                         .Print("")
                                         .To(Regex.Escape);

            m_phraseRegex = new Regex($"[^\\w {allowedNonLetters}]");
        }

        /// <summary>
        /// Returns words and rhythmic information of the specified sentence
        /// </summary>
        public IPhrase Parse(string phrase)
        {
            var words = m_phraseRegex.Replace(phrase, "")
                                     .Split(" ")
                                     .Select(word => new Word(word, GetRhythm(word)));

            return new PhraseImplementation( words );
        }

        private Rhythm GetRhythm(string word)
            => m_words.FirstOrDefault(_ => _.Text.Equals(word, StringComparison.InvariantCultureIgnoreCase))?.Rhythm
            ?? throw new ArgumentException($"Vocabulary does not contain word {word}");

        private struct PhraseImplementation : IPhrase
        {
            public IReadOnlyList<Word> Words { get; }

            public string Text => Words.Select(_ => _.Text).Print(" ");

            public Rhythm Rhythm => Rhythm.Concat(Words.Select(_ => _.Rhythm));

            public PhraseImplementation(IEnumerable<Word> words) => Words = words.ToArray();
        }
    }
}
