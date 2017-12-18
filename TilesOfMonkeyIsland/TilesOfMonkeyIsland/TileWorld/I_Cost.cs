using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesOfMonkeyIsland.TileWorld
{
    class I_Cost
    {
        /**
         * Infinity cost. No cost is higher than this.
         */
        public static int INFINITY = 999999999;
        /**
         * The normal and diagonal costs for all walkable types.
         */
        public static int ROAD_COST = 10;
        public static int ROAD_DIAGONAL_COST = 14;
        public static int SAND_COST = 14;
        public static int SAND_DIAGONAL_COST = 20;
        public static int WATER_COST = 20;
        public static int WATER_DIAGONAL_COST = 28;
        public static int MOUNTAIN_COST = 24;
        public static int MOUNTAIN_DIAGONAL_COST = 34;

        public static int getCost(eTileType type)
        {
            switch (type)
            {
            case eTileType.ROAD:
                return ROAD_COST;
            case eTileType.SAND:
                return SAND_COST;
            case eTileType.WATER:
                return WATER_COST;
            case eTileType.MOUNTAIN:
                return MOUNTAIN_COST;
            case eTileType.NONWALKABLE:
                return INFINITY;
            case eTileType.START:
                return ROAD_COST;
            case eTileType.END:
                return ROAD_COST;
            case eTileType.PATH:
                return ROAD_COST;
            case eTileType.UNKNOWN:
            default:
                return INFINITY;
            }
        }

        public static int getDiagonalCost(eTileType type)
        {
            switch (type)
            {
            case eTileType.ROAD:
                return ROAD_DIAGONAL_COST;
            case eTileType.SAND:
                return SAND_DIAGONAL_COST;
            case eTileType.WATER:
                return WATER_DIAGONAL_COST;
            case eTileType.MOUNTAIN:
                return MOUNTAIN_DIAGONAL_COST;
            case eTileType.NONWALKABLE:
                return INFINITY;
            case eTileType.START:
                return ROAD_DIAGONAL_COST;
            case eTileType.END:
                return ROAD_DIAGONAL_COST;
            case eTileType.PATH:
                return ROAD_DIAGONAL_COST;
            case eTileType.UNKNOWN:
            default:
                return INFINITY;
            }
        }
    }
}
