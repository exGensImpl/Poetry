package exgens.poetry

fun main(args: Array<String>) {
    val words = ZaliznyaksAccentuatedParadigm.read("All_Forms.txt")
    println(words)
}

data class Word(val text: String, val rhythm: Rhythm) {
    init {
        if (text.isNullOrBlank()) {
            throw IllegalArgumentException("A word should not be empty")
        }
        if (text.any(Char::isLetter) == false) {
            throw IllegalArgumentException("A word should contain at least one letter. Given: $text")
        }
    }
}

final class Rhythm(val length: Int, private val stressIndicies: Sequence<Int>) { 

    val hasStress = stressIndicies.any()
    val isStressed = 0 in stressIndicies
    val isEmpty = length == 0

    companion object{
        fun concat(rhythms: Sequence<Rhythm>)
        {
            val stresses =  mutableListOf<Int>()
            var length = 0

            for (rhythm in rhythms) {
                for (stress in rhythm.stressIndicies) {
                    stresses.add(stress + length);
                }
                length += rhythm.length;
            }

            Rhythm(length, stresses.asSequence())
        }
    }

    constructor(length: Int, stressIndex: Int) : this(length, listOf(stressIndex).asSequence()) {  }

    fun shifted(syllables : Int = 1)
        = Rhythm(
                if (length >= syllables) length - syllables else 0,
                stressIndicies.map{ it - syllables }.filter{ it >= 0 })
    
    override fun toString()
        = (1..length)
                    .map { if (stressIndicies.contains(it)) '\'' else '-' }
                    .joinToString("")
}

final class Phrase(val words: List<Word>)
{
    val text = words.map{ it.text }.joinToString(" ")
    val rhythm = Rhythm.concat(words.map{ it.rhythm }.asSequence())

    override fun toString() = text
}
    

    