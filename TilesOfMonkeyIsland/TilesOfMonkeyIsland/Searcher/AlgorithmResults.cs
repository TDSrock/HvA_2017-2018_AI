using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesOfMonkeyIsland.Searcher
{
    class AlgorithmResults
    {
        private int bestPathCost;
        private int nodesExpanded;
        /**
         * Integer list containing the indices of the tiles in the best path.
         */
        private ArrayList solutionPath = new ArrayList();

        public AlgorithmResults()
        {
            this.bestPathCost = 0;
            this.nodesExpanded = 0;
        }

        /**
         * This constructor immediately initializes the class attributes.
         * 
         * @param bestPathCost The cost of the best path
         * @param nodesExpanded Number of expanded nodes
         */
        public AlgorithmResults(int bestPathCost, int nodesExpanded)
        {
            this.bestPathCost = bestPathCost;
            this.nodesExpanded = nodesExpanded;
        }

        public int getBestPathCost()
        {
            return bestPathCost;
        }

        public void setBestPathCost(int cost)
        {
            this.bestPathCost = cost;
        }

        public int getNodesExpanded()
        {
            return nodesExpanded;
        }

        public void setNodesExpanded(int nodesExpanded)
        {
            this.nodesExpanded = nodesExpanded;
        }

        public ArrayList getSolutionPath()
        {
            return solutionPath;
        }

        public void setSolutionPath(ArrayList solutionPath)
        {
            this.solutionPath = solutionPath;
        }
    }
}
