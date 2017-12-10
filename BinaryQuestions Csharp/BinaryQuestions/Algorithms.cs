using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryQuestions
{
    static class Algorithms
    {
        
        static public int MiniMaxAlphaBetaPruning(BTNode node, int depth = -1, int alpha = int.MinValue, int beta = int.MaxValue, bool Max = true)
        {
            if(depth < 0)//if our depth is left unchanged we want to run through the whole tree
            {
                depth = int.MaxValue;
            }
            if (depth == 0 || !node.isQuestion())
            {
                return node.calculateEndNodeValue();
            }
            var children = node.GetChildren();
            if (Max == true)
            {
                foreach (BTNode child in children)
                {
                    alpha = Math.Max(alpha, MiniMaxAlphaBetaPruning(child, depth - 1, alpha, beta, !Max));
                    if (beta < alpha)
                    {
                        break;//prune
                    }

                }

                return alpha;
            }
            else
            {
                foreach (BTNode child in children)
                {
                    beta = Math.Min(beta, MiniMaxAlphaBetaPruning(child, depth - 1, alpha, beta, !Max));

                    if (beta < alpha)
                    {
                        break;//prune
                    }
                }

                return beta;
            }
        }
    }
}
