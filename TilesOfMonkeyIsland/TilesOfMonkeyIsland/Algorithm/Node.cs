using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilesOfMonkeyIsland.TileWorld;

namespace TilesOfMonkeyIsland.Algorithm
{
    class Node
    {
        public int x;
        public int y;
        public eTileType type;
        public float heuristic;
        public float manhattanDistance = 0;
        public float cost = 0;
        public Node parent = null;

        public Node(int x, int y, eTileType type)
        {
            this.x = x + 1;
            this.y = y + 1;
            this.type = type;
        }

        public void updateNodeIfNeeded(Node potentialParentNode, float costFromPotentialParentNode)
        {
            if (parent == null || costFromPotentialParentNode < this.cost)
            {
                this.parent = potentialParentNode;
                this.cost = costFromPotentialParentNode;
            }
        }
    }
}
