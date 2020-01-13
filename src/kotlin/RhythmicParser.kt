package exgens.poetry

/**
* Splits sentences to words and finds its syllabic rhythm
*/
final class RhythmicParser(val words: Sequence<Word>)
{
    val phraseRegex: Regex

    /**
    * Initializes a new instanc of rhythmic parser by specified list of allowed words 
    */
    init {
        val allowedNonLetters = words.flatMap{ it.text.asSequence() }
                                     .distinct()
                                     .filterNot(Char::isLetter)
                                     .joinToString("")

        phraseRegex = Regex("[^\\w ${Regex.escape(allowedNonLetters)}]")
    }

    /**
    * Returns words and rhythmic information of the specified phrase
    *
    * @param name The phrase that should be parsed
    * @throws IllegalArgumentException phrase contains a word that is not found in the vocabulary
    */
    fun parse(phrase: String) : Phrase {
        var words = phraseRegex.replace(phrase, "")
                                .split(" ")
                                .map{ Word(it, rhythmOf(it)) }

        return Phrase( words )
    }

    private fun rhythmOf(word: String) : Rhythm {
        val rhythm = words.firstOrNull{ it.text.equals(word) }?.rhythm

        if(rhythm == null) {
            throw IllegalArgumentException("Vocabulary does not contain word $word")
        }

        return rhythm
    }
}