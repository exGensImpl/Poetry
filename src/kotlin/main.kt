fun main(args: Array<String>) {
    println("Hello, Kotlin!")
}

data class Word(val text: String, val rhytm: Rhytm) { }

class Rhytm(private val _length: Int, private val _stress_indicies: Sequence<Int>) { 
}