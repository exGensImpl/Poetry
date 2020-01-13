package exgens.poetry

import java.io.*

fun main(args: Array<String>) {
    println("Hello, Kotlin!")
}

data class Word(val text: String, val rhytm: Rhythm) { }

class Rhythm(val length: Int, private val stressIndicies: Sequence<Int>) { 

    val hasStress = stressIndicies.any()

    val isStressed = stressIndicies.contains(0)

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

    fun shifted(syllables : Int = 1)
        = Rhythm(
                if (length >= syllables) length - syllables else 0,
                stressIndicies.map{ it - syllables }.filter{ it >= 0 })
    
    override fun toString()
        = (1..length)
                    .map { if (stressIndicies.contains(it)) '\'' else '-' }
                    .joinToString("")
}