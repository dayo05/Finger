// Implemente package via gradle to compile.
import org.openkoreantext.processor.OpenKoreanTextProcessorJava.*

fun main(args: Array<String>) = println(extractPhrases(tokenize(normalize(args.joinToString(" "))), true, false).joinToString("\n") { it.text() })
