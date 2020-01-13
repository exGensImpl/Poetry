package exgens.poetry

import java.io.*

/**
* Reads a @see RhythmicVocabulary from Zaliznyak's accentuated paradigm.
* The paradigm can be loaded by the following link: http://www.speakrus.ru/dict/all_forms.rar
*/
final class ZaliznyaksAccentuatedParadigm {
    companion object{
        private val vowels = listOf('а','е','о','у','ы','э','я','и','ю','ё');

        private val stress = '\''
        private val stressString = "\'"

        /** 
        * Reads a vocabulary from Zaliznyak's accentuated paradigm written in the file specified.
        */
        fun read(filepath: String)
            = File(filepath)
                   .readLines()
                   .flatMap{ it.split('#')[1].split(',') }
                   .filterNot{ it.isNullOrBlank() }
                   .filter{ it.any(Char::isLetter) }
                   .map{ parseWord(it) }
                   .toList();

        fun parseWord(wordform: String) : Word {
            val vowels = wordform.filter{ it in vowels || it == stress }.toList()
            val rhythm = Rhythm(vowels.count{ it != stress }, 
                                vowels.indexOf(stress) - 1)

            return Word(wordform.replace(stressString, ""), rhythm)
        }
    }
}