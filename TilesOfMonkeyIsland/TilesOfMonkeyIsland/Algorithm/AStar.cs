using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesOfMonkeyIsland.Algorithm
{
    class AStar : Algorithm
    {
        public AStar(TileWorld.TileWorld world)
            : base(world)
        {}
        
        // TO CHANGE 
        override protected float calculateHeuristic(Node node)
        {
            // Calculate the minimal distance walking horizontally / vertically and diagonally.            
            
            // Get the cost.
            
            // Return the heuristic.
            return 0.0
        }
    }
}
