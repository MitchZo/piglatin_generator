using System;
using System.Text.RegularExpressions;

namespace PigLatin_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool again = true;
            while (again)
            {
                //ask the user for a word or words
                string userInput = GetUserInput("Please give me a word or sentence to be translated into Pig Latin.");


                //if the input contains spaces, break the sentence into words
                string[] englishWords = ParseSentence(userInput);

                //take the user input and translate it to PigLatin
                string pigLatinWords = ConvertToPigLatin(englishWords);

                //display the word(s) in Pig Latin
                Console.WriteLine(pigLatinWords);

                //ask the user if they'd like to run again
                again = RunAgain();
            }
        }

        public static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            string response = Console.ReadLine();

            //verify the input isn't blank
            if (response.Length == 0)
            {
                return GetUserInput(message);
            }
            else
                return response;
        }
        public static string[] ParseSentence(string userSentence)
        {
            string[] wordArray = userSentence.Split(' ');
            return wordArray;
        }
        public static string ConvertToPigLatin(string[] words)
        {
            //set variables for use later in the method
            string translatedWords = "";
            string translatedSentence = translatedWords;
            int i = 0;

            //loop through each word in the sentence array, gather information
            //about each and translate it
            foreach (string word in words)
            {
                //determine if the word is upper case, lower case, or title case
                //and save it to a string
                string capitalization = DetermineCase(word);

                //Determine if the word ends in punctuation
                string punctuation = DeterminePunctuation(word);

                //Determine if the word starts with a vowel
                bool startsWithVowel = DetermineVowel(word);

                //Determine if the word contains special characters or numbers
                bool containsSpecialOrNum = DetermineSpecialCase(word);

                //send the word out to be built in Pig Latin
                translatedWords = BuildTranslatedWord(word, capitalization, punctuation, startsWithVowel, containsSpecialOrNum);

                //If a sentence was sent, rebuild it and convert it back to a string.
                if (words.Length > 1)
                {
                    words[i] = translatedWords + punctuation + " ";
                    translatedSentence += words[i];
                    i++;
                }
                else
                    translatedSentence = translatedWords + punctuation;
            }
                return translatedSentence;
        }
        public static string DetermineCase(string word)
        {
            string capitalization = "";

            //compare word to it's all uppercase version
            if (word == word.ToUpper())
                capitalization = "UpperCase";

            //compare word to its all lowercase version
            else if (word == word.ToLower())
            {
                capitalization = "LowerCase";
            }

            //if it's neither all uppercase or all lowercase, assume it's title case
            else
                capitalization = "TitleCase";
            return capitalization;
        }
        public static string DeterminePunctuation(string word)
        {
            //determine if there's punctuation at the end of the word
            if ((word.EndsWith(".") == true) || (word.EndsWith(",") == true) || (word.EndsWith("'") == true)
             || (word.EndsWith("?") == true) || (word.EndsWith("!") == true))
            {
                //find the index of the final letter
                int lastPosition = word.Length-1;

                //convert the word into its respective characters
                char[] letters = word.ToCharArray();

                //take the final character (which should be punctuation) and return it
                return letters[lastPosition].ToString();
            }
            else
                //if there is no punctuation mark at the end of the word, return
                //an empty string
                return string.Empty;
        }
        public static bool DetermineVowel(string word)
        {
            //convert the word into letters
            char[] letters = word.ToCharArray();

            //isolate the first character
            char letter = letters[0];

            //determine if the first letter is a vowel
            if ((letter == 'a') ||
                (letter == 'e') ||
                (letter == 'i') ||
                (letter == 'o') ||
                (letter == 'u') ||
                (letter == 'A') ||
                (letter == 'E') ||
                (letter == 'I') ||
                (letter == 'O') ||
                (letter == 'U'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool DetermineSpecialCase(string word)
        {
            if (Regex.IsMatch(word, @"[0-9]"))
                return true;
            else
                return false;
        }
        public static string TranslateVowel(string word)
        {
            //remove any punctuation so we are just working on the word. Could be moved to the determine punctuation method
            if ((word.EndsWith(".") == true) || (word.EndsWith(",") == true) || (word.EndsWith("'") == true)
             || (word.EndsWith("?") == true) || (word.EndsWith("!") == true))
                word = word.Remove(word.Length - 1);

            return word + "way";
        }
        public static string TranslateConsonant(string word)
        {
            //determine where the first vowel is and return the index
            int vowelPosition = DetermineVowelPosition(word);

            //using the fist vowel position, store the first part of the word up to that point
            string prefix = StorePrefix(vowelPosition, word);

            //using the first vowel position, store the second part of the word past that point
            string suffix = StoreSuffix(vowelPosition, word);

            //append the prefix to the suffix and add "ay" to the end
            return suffix.ToLower() + prefix.ToLower() + "ay";

        }
        public static int DetermineVowelPosition(string word)
        {
            //set the first vowel position to be the max
            int firstVowelPosition = word.Length;

            //set to lowercase for easier comparison. We reset the case at the end so this is safe.
            word = word.ToLower();

            //Compare the current vowel position to each of the vowels. If one has an earlier position, use that instead.
            if (firstVowelPosition > word.IndexOf('a') && word.Contains('a'))
            {
                firstVowelPosition = word.IndexOf('a');
            }
            if (firstVowelPosition > word.IndexOf('e') && word.Contains('e'))
            {
                firstVowelPosition = word.IndexOf('e');
            }
            if (firstVowelPosition > word.IndexOf('i') && word.Contains('i'))
            {
                firstVowelPosition = word.IndexOf('i');
            }
            if (firstVowelPosition > word.IndexOf('o') && word.Contains('o'))
            {
                firstVowelPosition = word.IndexOf('o');
            }
            if (firstVowelPosition > word.IndexOf('u') && word.Contains('u'))
            {
                firstVowelPosition = word.IndexOf('u');
            }
            return firstVowelPosition;
        }
        public static string StorePrefix(int trimLocation, string word)
        {
            //try to trim the word at the first vowel
            try
            { return word.Remove(trimLocation); }
            
            //if there are no vowels, just return the word
            catch
            { return word; }

        }
        public static string StoreSuffix(int trimLocation, string word)
        {
            //set the word equal to the last part after the vowel
            word = word.Substring(trimLocation);
            
            //remove any punctuation. This might be able to be moved to the determine punctuation method
            if ((word.EndsWith(".") == true) || (word.EndsWith(",") == true) || (word.EndsWith("'") == true)
             || (word.EndsWith("?") == true) || (word.EndsWith("!") == true))
                return word.Remove(word.Length-1);
            else
                return word;
        }
        public static string ModifyToTitleCase(string word)
        {
            //set the word to all lowercase
            word.ToLower();

            //separate the word into an array of letters
            char[] letters = word.ToCharArray();
            char firstLetter = letters[0];

            //set the first letter to uppercase.
            letters[0] = char.ToUpper(firstLetter);

            //empty the word so we can set to to be equal to the new letter array
            word = "";
            
            //rebuild the word out of the array values
            int i = 0;
            foreach (char letter in letters)
            {
                word += letters[i].ToString();
                i++;
            }

            return word;
        }
        public static string BuildTranslatedWord(string word, string capitalization, string punctuation, bool startsWithVowel, bool containsSpecialOrNum)
        {
            string translatedWord = "";
            //if the word starts with a vowel, add "way" to the end and
            //save it
            if (startsWithVowel == true && containsSpecialOrNum == false)
                translatedWord = TranslateVowel(word);

            //if the word starts with a consonant, take the first sound up to the 
            //first vowel, add it to the end, append "ay" and the punctuation after
            //the first sound and set the Case.
            else if (startsWithVowel == false && containsSpecialOrNum == false)
                translatedWord = TranslateConsonant(word);
            
            //if neither of the above is correct, the word is a special case and should not
            //be changed
            else
                translatedWord = word;

            //set the word to be uppercase, lowercase, or titlecase
            if (capitalization == "UpperCase")
            {
                translatedWord = translatedWord.ToUpper();
            }
            else if (capitalization == "LowerCase")
            {
                translatedWord = translatedWord.ToLower();
            }
            else
            {
                translatedWord = ModifyToTitleCase(translatedWord);
            }
            return translatedWord;
        }
        public static bool RunAgain()
        {
            Console.WriteLine("would you like to run again?");
            string userResponse = Console.ReadLine();
            return ValidateYesNo(userResponse);
        }
        public static bool ValidateYesNo(string response)
        {
            if ((response.ToLower() == "yes") || (response.ToLower() == "y"))
            {
                return true;
            }
            else if ((response.ToLower() == "no") || (response.ToLower() == "n"))
            {
                return false;
            }
            else
                return RunAgain();
        }
    }
}