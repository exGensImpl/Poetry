using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
    /// <summary>
    /// Represents a word sequence
    /// </summary>
    public class Phrase
    {
        /// <summary>
        /// Returns ordered list of the words of the phrase
        /// </summary>
        public IReadOnlyList<Word> Words { get; }

        /// <summary>
        /// Returns the text representation of the phrase
        /// </summary>
        public string Text => Words.Select(_ => _.Text).Print(" ");

        /// <summary>
        /// Returns the syllabic rhythm of the phrase
        /// </summary>
        public Rhythm Rhythm => Rhythm.Concat(Words.Select(_ => _.Rhythm));

        public Phrase(IEnumerable<Word> words) => Words = words.ToArray();

        /// <inheritdoc/>
        public  override string ToString() => Text;
    }
}
