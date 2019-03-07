using System;
using System.IO;

namespace Hangman
{
    public class Hangman
    {
        const int BadGuessLimit = 8;
        const int Rows = 5;
        const int Cols = 3;
        static void Main(string[] args)
        {
            string word;
            string answer = "yes";
            ShowHangmanOnStart();
            Console.WriteLine("Welcome to Hangman");

            StreamReader wordFile = new StreamReader(@"C:\Projects\Hangman\Counties.txt");
            while((word = wordFile.ReadLine()) != null && answer == "yes")
            {
                char[,] hangman = CreateHangmanTemplate();
                bool playing = true;
                int badGuesses = 0;
                int wordLength = word.Length;
                var lettersArray = new char[wordLength];
                for (int i = 0; i < lettersArray.Length; i++)
                {
                    lettersArray[i] = '_';
                }
                while (playing == true)
                {
                    DisplayWordTemplate(lettersArray);
                    try
                    {
                        Console.WriteLine("Please enter your guess: ");
                        char letter = char.Parse(Console.ReadLine().ToLower());
                        int position = CorrectGuessPosition(word, letter);
                        if(position > -1)
                        {
                            Console.WriteLine("{0} is a correct guess.", letter);
                            lettersArray = PlaceLetterIntoArray(word, letter, lettersArray, position);
                            if(IsWin(lettersArray))
                            {
                                DisplayWordTemplate(lettersArray);
                                Console.WriteLine("Congratulations.");
                                playing = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} is an incorrect guess.", letter);
                            badGuesses++;
                            hangman = DrawHangman(hangman, badGuesses);
                            if(badGuesses == BadGuessLimit-1)
                            {
                                Console.WriteLine("Be very cafeful you only have one last chance.");
                            }
                            if (badGuesses == BadGuessLimit)
                            {
                                Console.WriteLine("You are dead!");
                                playing = false;
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Your guess must be a single letter.");
                        continue;
                    }
                }

                Console.WriteLine("Would you like to guess another word? (yes/no)");
                answer = Console.ReadLine().ToLower();

            }
            if(answer != "yes")
                Console.WriteLine("Thanks for playing.");
            else
                Console.WriteLine("Sorry the game is over there are no more words to guess.");

            Console.ReadLine();
        }

        public static void ShowHangmanOnStart()
        {
            char[,] template = new char[Rows, Cols];

            template[0, 1] = ' '; template[1, 1] = 'O'; template[1, 2] = ' ';
            template[2, 0] = '-'; template[2, 1] = '|'; template[2, 2] = '-';
            template[3, 0] = ' '; template[3, 1] = '|'; template[3, 2] = ' ';
            template[4, 0] = '/'; template[4, 1] = ' '; template[4, 2] = '\\';

            for (int i = 0; i < Rows; i++)
            {
                for (int z = 0; z < Cols; z++)
                {
                    Console.Write(template[i, z]);
                }
                Console.Write("\n");
            }
        }

        public static char[,] CreateHangmanTemplate()
        {
            char[,] hangman = new char[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int z = 0; z < Cols; z++)
                {
                    hangman[i, z] = ' ';
                }
            }
            return hangman;
        }

        public static char[,] DrawHangman(char[,] hangman, int badGuesses)
        {
            switch (badGuesses)
            {
                case 1:
                    hangman[1, 1] = 'O';
                    break;
                case 2:
                    hangman[2, 0] = '-';
                    break;
                case 3:
                    hangman[2, 1] = '|';
                    break;
                case 4:
                    hangman[2, 2] = '-';
                    break;
                case 5:
                    hangman[3, 1] = '|';
                    break;
                case 6:
                    hangman[4, 0] = '/';
                    break;
                case 7:
                    hangman[4, 2] = '\\';
                    break;
                case 8:
                    hangman[0, 1] = '|';
                    break;
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int z = 0; z < Cols; z++)
                {
                    Console.Write(hangman[i, z]);
                }
                Console.Write("\n");
            }
            return hangman;
        }

        public static void DisplayWordTemplate(char[] array)
        {
            Console.WriteLine("The word you have to guess is: ");
            for(int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine(Environment.NewLine);
        }
        public static int CorrectGuessPosition(string word, char letter)
        {
            int position = -1; 
            foreach (char character in word)
            {
                position++;
                if (character == letter)
                {
                    return position;
                }
            }
            return -1;
        }
        public static char[] PlaceLetterIntoArray(string word, char letter, char[] array, int position)
        {
            array[position] = letter;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter && array[i] != letter)
                {
                    array[i] = letter;
                }
            }
            return array;
        }

        public static bool IsWin(char[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] == '_')
                    return false;
            }
            return true;
        }

    }
}
