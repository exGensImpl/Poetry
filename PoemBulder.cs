using System;
using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
    /// <summary>
    /// Creates poetic continuations of the specified phrases
    /// </summary>
    public sealed class PoemBulder
    {
        private readonly RhythmicVocabulary m_vocabulary;

        private readonly RhythmFinder m_rhythmFinder;

        private readonly int m_rhymeLength;

        public PoemBulder(IReadOnlyCollection<Word> words, int rhymeLength)
        {
            m_vocabulary = new RhythmicVocabulary(words);
            m_rhythmFinder = new RhythmFinder(words);
            m_rhymeLength = rhymeLength;
        }

        /// <summary>
        /// Builds a sequence of phrases which matched to the specified phrase by syllabic rhythm and rhyme.
        /// This method is lazy evaluated
        /// </summary>
        public IEnumerable<IPhrase> GetPoeticContinuations(string phrase)
         => m_rhythmFinder.GetRhythm(phrase)
                          .To(BuildStep.Initial)
                          .Unfold(NextStep)
                          .Where(step => step.IsCompleted)
                          .Where(step => HaveRhyme(phrase, step.Text));

        private IEnumerable<BuildStep> NextStep(BuildStep step)
         => m_vocabulary.GetSatisfied(step.RemainingSyllables)
                        .Where(word => CanBeAdded(step, word))
                        //For fun
                        .Shuffle()
                        .Select(step.Add);

        private bool CanBeAdded(BuildStep step, Word word)
         => word.Rhythm.HasStress 
         || step.Words.Count(_ => _.Rhythm.HasStress) < 3;

        private bool HaveRhyme(string first, string second)
         => first.Length >= m_rhymeLength
         && second.Length >= m_rhymeLength
         && string.Equals(
             Last(first, m_rhymeLength),
             Last(second, m_rhymeLength),
             StringComparison.InvariantCultureIgnoreCase);

        private static string Last(string source, int characters)
        => source.Substring(source.Length - characters, characters);

        private sealed class BuildStep : IPhrase
        {
            /// <inheritdoc/>
            public Rhythm RemainingSyllables { get; }

            /// <inheritdoc/>
            public IReadOnlyList<Word> Words { get; }

            public string Text => string.Join(" ", Words.Select(_ => _.Text));

            public bool IsCompleted => RemainingSyllables.IsEmpty;

            public BuildStep(Rhythm remaining, IReadOnlyList<Word> words)
            {
                RemainingSyllables = remaining;
                Words = words;
            }

            /// <summary>
            /// Creates an initial step of building of phrases that matched to the specified syllabic rhythm
            /// </summary>
            public static BuildStep Initial(Rhythm rhythm) => new BuildStep(rhythm, Array.Empty<Word>());

            /// <summary>
            /// Creates a copy of this step with addition of the specified word to the end of the phrase
            /// </summary>
            public BuildStep Add(Word word)
            {
                return new BuildStep(
                    RemainingSyllables.GetShifted(word.Rhythm.Length),
                    Words.Concat(new[] { word }).ToArray()
                );
            }

            /// <inheritdoc/>
            public override string ToString() => Text;
        }

    }
}
