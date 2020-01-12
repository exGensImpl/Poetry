using System;
using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
    /// <summary>
    /// Finds the words which syllabic rhythm mathes to the specified ones
    /// </summary>
    public sealed class RhythmicVocabulary
    {
        private readonly RhythmicVocabularyNode m_root = new RhythmicVocabularyNode();

        public RhythmicVocabulary(IReadOnlyCollection<Word> words)
        {
            foreach (var word in words)
            {
                m_root.Add(word, word.Rhythm);
            }
        }

        /// <summary>
        /// Returns all the words which syllabic rhythm is matched by the start of the rhythm specified 
        /// </summary>
        public IEnumerable<Word> GetSatisfied(Rhythm rhythm) => m_root.GetSatisfied(rhythm);

        private sealed class RhythmicVocabularyNode
        {
            private readonly List<Word> m_words = new List<Word>();

            private Lazy<RhythmicVocabularyNode> m_stressed { get; } = new Lazy<RhythmicVocabularyNode>();

            private Lazy<RhythmicVocabularyNode> m_unstressed { get; } = new Lazy<RhythmicVocabularyNode>();

            public IEnumerable<Word> GetSatisfied(Rhythm rhythm)
            {
                if (rhythm.IsEmpty)
                {
                    return m_words;
                }
                else
                {
                    var nextNode = rhythm.IsStressed ? m_stressed : m_unstressed;
                    return nextNode.Value.GetSatisfied(rhythm.GetShifted()).Concat(m_words);
                }
            }

            public void Add(Word word, Rhythm rhythm)
            {
                if (rhythm.IsEmpty)
                {
                    m_words.Add(word);
                }
                else
                {
                    var next = rhythm.IsStressed ? m_stressed : m_unstressed;
                    next.Value.Add(word, rhythm.GetShifted());
                }
            }
        }
    }
}
