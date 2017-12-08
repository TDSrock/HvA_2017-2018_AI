using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryQuestions
{
    class Program
    {
        static BTTree tree;
        static void Main(string[] args)
        {
            //There's no need to ask for the initial data when it already exists
            if (File.Exists("serialized.bin"))
                tree = new BTTree();
            else
                startNewGame();

            //ask if the player wants to play or see the traversal orders
            preMenu();

            Console.WriteLine("\nStarting the \"20 Binary Questions\" Game!\nThink of an object, person or animal.");
            tree.query(); //play one game
            while(playAgain())
            {
                Console.WriteLine("\nThink of an object, person or animal.");
                Console.WriteLine();
                tree.query(); //play one game
            }
        }

        static bool playAgain()
        {
            Console.Write("\nPlay Another Game? ");
            char inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
            while (inputCharacter != 'y' && inputCharacter != 'n')
            {
                Console.WriteLine("Incorrect input please enter again: ");
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }
            if (inputCharacter == 'y')
                return true;
            else
                return false;
        }

        static void preMenu()
        {
            Console.WriteLine("Play the game or get the traversal orders?");
            Console.Write("Enter 'p' to play or 't' for the traversal orders: "); //let the player play the game or print the traversal order
            char inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
            while (inputCharacter != 'p' && inputCharacter != 't') //ensuring correct input
            {
                Console.WriteLine("Incorrect input please enter again: ");
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }

            if (inputCharacter == 'p') //if player wants to play the game then return back to the main game
            {
                return;
            }

            traversal(); //If the player didn't pick play, start traversal
        }

        static void traversal()
        {
            char inputCharacter;

            Console.WriteLine("What kind of traversal order do you want??");
            Console.Write("Enter '1' for pre-order, '2' for in-order and '3' for post-order: ");
            inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
            while (inputCharacter != '1' && inputCharacter != '2' && inputCharacter != '3') //ensuring correct input
            {
                Console.WriteLine("Incorrect input please enter again: ");
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }

            switch (inputCharacter)
            {
                case '1': //pre-order traversal selected
                    tree.preOrder();
                    break;
                case '2': //in-order traversal selected
                    tree.inOrder();
                    break;
                case '3': //post-order traversal selected
                    tree.postOrder();
                    break;
                default: //impossible
                    Console.WriteLine("ERROR - TERMINATING"); //Sent Arnold back in time to find John Connor.
                    Environment.Exit(0);
                    break;
            }

            Console.WriteLine("\nPrint another traversal order? Enter 'y' for yes 'n' for no: ");
            inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
            while (inputCharacter != 'y' && inputCharacter != 'n') //ensuring correct input
            {
                Console.WriteLine("Incorrect input please enter again: ");
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }

            if(inputCharacter == 'y') //restarting the traversal 
            {
                traversal(); 
            } 
            //if the code reaches this point it goes back to the main game
        }

        static void startNewGame()
        {
            Console.WriteLine("No previous knowledge found!");
            Console.WriteLine("Initializing a new game.\n");
            Console.WriteLine("Enter a question about an object, person or animal: ");
            string question = Console.ReadLine();
            Console.Write("Enter a possible guess (an object, person or animal) if the response to this question is Yes: ");
            string yesGuess = Console.ReadLine();
            Console.Write("Enter a possible guess (an object, person or animal) if the response to this question is No: ");
            string noGuess = Console.ReadLine();

            tree = new BTTree(question, yesGuess, noGuess);
        }
    }
}