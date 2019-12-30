using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExGens.Poetry
{
    ///<summary>
    /// Reads a <see cref="RhythmicVocabulary"> from Zaliznyak's accentuated paradigm.
    /// The paradigm can be loaded by the following link: http://www.speakrus.ru/dict/all_forms.rar
    ///</summary>
    public static class ZaliznyaksAccentuatedParadigm
    {
        private static char[] m_vowels = new[]{'а','е','о','у','ы','э','я','и','ю','ё'};

        private const char m_stress = '\'';
        private const string m_stressString = "\'";

        ///<summary>
        /// Reads a <see cref="RhythmicVocabulary"> from Zaliznyak's accentuated paradigm written in the file specified.
        ///</summary>
        public static IReadOnlyCollection<Word> Read(string filepath)
            => File.ReadLines(filepath)
                   .SelectMany(_ => _.Split('#')[1].Split(','))
                   .Where(_ => string.IsNullOrWhiteSpace(_) == false)
                   .Where(_ => _.Any(char.IsLetter))
                   .Select(ParseWord)
                   .ToArray();

        private static Word ParseWord(string wordform) {
            var vowels = wordform.Where(_ => m_vowels.Contains(_) || _ == m_stress).ToList();
            var Rhythm = new Rhythm(vowels.Count(_ => _ != m_stress), 
                                    vowels.IndexOf(m_stress) - 1);

            return new Word(wordform.Replace(m_stressString, ""), Rhythm);
        }
    }
}
