using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BinaryQuestions
{
    class TwentyQuestionsGameHost : MonoBehaviour
    {
        static BTTree tree;
        public string output;
        void onAwake()
        {
            //There's no need to ask for the initial data when it already exists
            if(File.Exists("serialized.bin"))
            {
                tree = new BTTree();
                tree.parent = this;
            }
            else
                startNewGame();

            Debug.Log("\nStarting the \"20 Binary Questions\" Game!\nThink of an object, person or animal.");
            tree.query(); //play one game
            while(playAgain())
            {
                Debug.Log("\nThink of an object, person or animal.");
                tree.query(); //play one game
            }
        }

        static bool playAgain()
        {
            Debug.Log("\nPlay Another Game? ");
            char inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
            while (inputCharacter != 'y' && inputCharacter != 'n')
            {
                Debug.Log("Incorrect input please enter again: ");
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }
            if (inputCharacter == 'y')
                return true;
            else
                return false;
        }

        static void startNewGame()
        {
            Debug.LogError("No previous knowledge found!\n" + 
                "Initializing a new game.\n");
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
