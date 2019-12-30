using System.Collections.Generic;

namespace ExGens.Poetry
{
    /// <summary>
    /// Represents a word sequence
    /// </summary>
    public interface IPhrase
    {
        /// <summary>
        /// Returns ordered list of the words of the phrase
        /// </summary>
        IReadOnlyList<Word> Words { get; }

        /// <summary>
        /// Returns the text representation of the phrase
        /// </summary>
        string Text { get; }
    }
}
