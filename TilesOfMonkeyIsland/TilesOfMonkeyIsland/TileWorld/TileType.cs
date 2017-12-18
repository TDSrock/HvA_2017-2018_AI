using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesOfMonkeyIsland.TileWorld
{
    enum eTileType
    {
        ROAD,
        SAND,
        WATER,
        MOUNTAIN,
        NONWALKABLE,
        START,
        END,
        PATH,
        UNKNOWN
        /**
         * UNKNOWN is used for colors that were not recognized
         */
    }

    class TileType
    {
        eTileType Type;
        
        private readonly int cost;
        private readonly int diagonalCost;

        TileType(eTileType type) {
            Type = type;
            cost = I_Cost.getCost(type);
            diagonalCost = I_Cost.getDiagonalCost(type);
        }
        
        public int getCost() {
            return cost;
        }
        
        public int getDiagonalCost() {
            return diagonalCost;
        }

        public eTileType getType() {
            return Type;
        }
    }
}
