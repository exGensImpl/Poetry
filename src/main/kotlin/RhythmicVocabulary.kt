
package exgens.poetry

/**
* Finds the words which syllabic rhythm mathes to the specified ones
*/
final class RhythmicVocabulary(words: Sequence<Word>) {

    private val root = RhythmicVocabularyNode()

    init {        
        for (word in words) {
            root.add(word, word.rhythm)
        }
    }

    /**
    * Returns all the words which syllabic rhythm is matched by the start of the rhythm specified 
    */
    fun getSatisfied(rhythm: Rhythm) = root.getSatisfied(rhythm)

    final class RhythmicVocabularyNode {

        private val words = mutableListOf<Word>()
        private val stressed by lazy { RhythmicVocabularyNode() }
        private val unstressed by lazy { RhythmicVocabularyNode() }

        fun getSatisfied(rhythm: Rhythm) : Sequence<Word> {
            if (rhythm.isEmpty) {
                return words.asSequence()
            }
            else {
                val nextNode = if (rhythm.isStressed) stressed else unstressed
                return nextNode.getSatisfied(rhythm.shifted()).plus(words)
            }
        }

        fun add(word: Word, rhythm: Rhythm) {
            if (rhythm.isEmpty) {
                words.add(word)
            }
            else {
                val next = if (rhythm.isStressed) stressed else unstressed
                next.add(word, rhythm.shifted())
            }
        }
    }    
}