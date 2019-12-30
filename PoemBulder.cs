using System;
using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
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

        public IEnumerable<IPhrase> BuildSimilarStrings(string phrase)
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
            public Rhythm RemainingSyllables { get; }

            public IReadOnlyList<Word> Words { get; }

            public string Text => string.Join(" ", Words.Select(_ => _.Text));

            public bool IsCompleted => RemainingSyllables.IsEmpty;

            public static BuildStep Initial(Rhythm rhythm) => new BuildStep(rhythm, Array.Empty<Word>());

            public BuildStep(Rhythm remaining, IReadOnlyList<Word> words)
            {
                RemainingSyllables = remaining;
                Words = words;
            }

            public BuildStep Add(Word word)
            {
                return new BuildStep(
                    RemainingSyllables.GetShifted(word.Rhythm.Length),
                    Words.Concat(new[] { word }).ToArray()
                );
            }

            public override string ToString() => Text;
        }

    }
}
