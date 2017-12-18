using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilesOfMonkeyIsland.TileWorld;

namespace TilesOfMonkeyIsland.Algorithm
{
    class Dijkstra : Algorithm
    {
        public Dijkstra(TileWorld.TileWorld world) 
            : base(world)
        {}

        override protected float calculateHeuristic(Node node)
        {
            // Get the cost.
            float cost = node.cost;

            // Return the heuristic.
            return cost;
        }

        /**
         * Dijkstra is finished when there is no other path open with a lower cost than the solution path.
         */
        override protected bool done()
        {
            return this.pickLowestHeuristicValue() == goalNode;
        }
    }
}
