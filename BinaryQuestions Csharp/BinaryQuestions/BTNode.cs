﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryQuestions
{
    [Serializable] class BTNode
    {
        string message;
        BTNode noNode;
        BTNode yesNode;

        /**
         * Constructor for the nodes: This class holds an String representing 
         * an object if the noNode and yesNode are null and a question if the
         * yesNode and noNode point to a BTNode.
         */
        public BTNode(string nodeMessage)
        {
            message = nodeMessage;
            noNode = null;
            yesNode = null;
        }

        public void query(int q)
        {
            if (q > 20)
            {
                Console.WriteLine("That was the last question. You win!");
            }
            else if (this.isQuestion())
            {
                Console.WriteLine(q + ") " + this.message);
                Console.Write("Enter 'y' for yes and 'n' for no: ");
                char input = getYesOrNo(); //y or n
                if (input == 'y')
                    yesNode.query(q+1);
                else
                    noNode.query(q+1);
            }
            else
                this.onQueryObject(q);
        }

        public void onQueryObject(int q)
        {
            Console.WriteLine(q + ") Are you thinking of a(n) " + this.message + "? ");
            Console.Write("Enter 'y' for yes and 'n' for no: ");
            char input = getYesOrNo(); //y or n
            if (input == 'y')
                Console.Write("I Win!\n");
            else
                updateTree();
        }

        private void updateTree()
        {
            Console.Write("You win! What were you thinking of? ");
            string userObject = Console.ReadLine();
            Console.Write("Please enter a question to distinguish a(n) "
                + this.message + " from " + userObject + ": ");
            string userQuestion = Console.ReadLine();
            Console.Write("If you were thinking of a(n) " + userObject
                + ", what would the answer to that question be (\'yes\' or \'no\')? ");
            char input = getYesOrNo(); //y or n
            if (input == 'y')
            {
                this.noNode = new BTNode(this.message);
                this.yesNode = new BTNode(userObject);
            }
            else
            {
                this.yesNode = new BTNode(this.message);
                this.noNode = new BTNode(userObject);
            }
            Console.Write("Thank you! My knowledge has been increased");
            this.setMessage(userQuestion);
        }

        public bool isQuestion()
        {
            if (noNode == null && yesNode == null)
                return false;
            else
                return true;
        }

        /**
         * Asks a user for yes or no and keeps prompting them until the key
         * Y,y,N,or n is entered
         */
        private char getYesOrNo()
        {
            char inputCharacter = ' ';
            while (inputCharacter != 'y' && inputCharacter != 'n')
            {
                inputCharacter = Console.ReadLine().ElementAt(0);
                inputCharacter = Char.ToLower(inputCharacter);
            }
            return inputCharacter;

        }

        // tree traversal methods
        public void preOrder(BTNode node) //Write root, left(yes), right(no)
        {
            Console.Write(node.getMessage() + " ");

            if (node.isQuestion())
            {
                preOrder(node.yesNode);
                preOrder(node.noNode);
            }
        }

        public void inOrder(BTNode node) //Write left(yes), root, right(no)
        {
            if (node.isQuestion())
            {
                inOrder(node.yesNode);
            }
            Console.Write(node.getMessage() + " ");
            if (node.isQuestion())
            {
                inOrder(node.noNode);
            }
        }

        public void postOrder(BTNode node) //Write left(yes), right(no), root
        {
            if (node.isQuestion())
            {
                preOrder(node.yesNode);
                preOrder(node.noNode);
            }
            Console.Write(node.getMessage() + " ");
        }

        //Calculate the value of a given node
        public int calculateEndNodeValue() //returns 0 for question nodes
        {
            int value = 0;
            if (!isQuestion()) // check if the node is a leafnode
            {
                for (int i = 0; i < message.Length; i++)
                {
                    if (char.IsLetter(message[i]))
                    {
                        value++;
                    }
                }
            }
            return value;
        }
        
        public int minMax(bool isMaxCycle)
        {
            int value = 0;
            int yesValue;
            int noValue;


            if (yesNode.isQuestion())
            {
                yesValue = yesNode.minMax(!isMaxCycle); //if the node has children run another minmax cycle
            }
            else
            {
                yesValue = yesNode.calculateEndNodeValue();
            }

            if (noNode.isQuestion())
            {
                noValue = noNode.minMax(!isMaxCycle); //if the node has children run another minmax cycle
            }
            else
            {
                noValue = noNode.calculateEndNodeValue();
            }

            if (isMaxCycle) // if the player tries to max
            {
                if(yesValue > noValue) //set the value to the highest value
                {
                    value = yesValue; 
                }
                else
                {
                    value = noValue;
                }
            }
            else // if the player tries to min 
            {
                if (yesValue < noValue) // set the value to the lowest value    
                {
                    value = yesValue;
                }
                else
                {
                    value = noValue;
                }
            }
            
            return value;
        }

        public void evaluate(bool alphaBetaPruning, bool print, int depth = -1)
        {
            if (!alphaBetaPruning)
            {
                if (print)
                {
                    Console.WriteLine("the optimal value is: " + minMax(true));
                }
                else
                {
                    minMax(true);
                }
            }
            else
            {
                if (print)
                {
                    Console.WriteLine("the optimal value is: " + Algorithms.MiniMaxAlphaBetaPruning(this, depth));
                }
                else
                {
                    Algorithms.MiniMaxAlphaBetaPruning(this, depth);
                }
                
            }
        }

        //Mutator Methods
        public void setMessage(string nodeMessage)
        {
            message = nodeMessage;
        }

        public string getMessage()
        {
            return message;
        }

        public void setNoNode(BTNode node)
        {

            noNode = node;
        }

        public BTNode getNoNode()
        {
            return noNode;
        }

        public void setYesNode(BTNode node)
        {
            yesNode = node;
        }

        public BTNode getYesNode()
        {
            return yesNode;
        }

        public List<BTNode> GetChildren()
        {
            List<BTNode> children = new List<BTNode>();
            if (yesNode != null)
                children.Add(yesNode);
            if (noNode != null)
                children.Add(noNode);
            return children;
        }
    }
}
