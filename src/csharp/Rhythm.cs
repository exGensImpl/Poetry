using System;
using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
    ///<summary>
    /// Represents the syllabic rhythm of a phrase
    ///</summary>
    public struct Rhythm
    {
        /// <summary>
        /// Indicates that this syllabic rhythm has at least one stress
        /// </summary>
        public bool HasStress => m_stressIndices.Any();

        /// <summary>
        /// Indicates that this syllabic rhythm has stress on the first syllable
        /// </summary>
        public bool IsStressed => m_stressIndices.Contains(0);

        /// <summary>
        /// Indicates that this syllabic rhythm has no syllables
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Returns syllable count in the rhythm
        /// </summary>
        public int Length { get; }

        private readonly List<int> m_stressIndices;

        /// <summary>
        /// Returns a syllabic rhythm consisting of sequentially joined specified rhythms
        /// </summary>
        /// <param name="rhytms">The rhytms to concatenate</param>
        public static Rhythm Concat(IEnumerable<Rhythm> rhythms)
        {
            var stresses = new List<int>();
            var length = 0;

            foreach (var rhythm in rhythms)
            {
                foreach (var stress in rhythm.m_stressIndices)
                {
                    stresses.Add(stress + length);
                }

                length += rhythm.Length;
            }

            return new Rhythm(length, stresses);
        }

        public Rhythm(int length, int stressIndex)
        : this(length, stressIndex < -1 ? Array.Empty<int>() : new[] { stressIndex })
        {
        }

        public Rhythm(int length, IEnumerable<int> stressIndices)
        {
            Length = length;
            m_stressIndices = stressIndices.ToList();
        }

        /// <summary>
        /// Returns a syllabic rhythm similar to this but shifted by the specified number of syllables
        /// </summary>
        public Rhythm GetShifted(int syllables = 1)
            => new Rhythm(
                Length >= syllables ? Length - syllables : 0,
                m_stressIndices.Select(_ => _ - syllables).Where(_ => _ >= 0));

        public override string ToString()
        {
            var stresses = m_stressIndices;
            return new string(
                Enumerable.Range(0, Length)
                          .Select(i => stresses.Contains(i) ? '\'' : '-')
                          .ToArray());
        }
    }
}
