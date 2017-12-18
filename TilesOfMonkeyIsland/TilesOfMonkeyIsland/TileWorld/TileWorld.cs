using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesOfMonkeyIsland.TileWorld
{
    class TileWorld
    {
        /**
         * TileWorld attributes
         */
        private eTileType[,] world = null;
        private int width;
        private int height;
        private int blockSize;
        private int DEFAULT_BLOCK_SIZE = 20;

        public TileWorld()
        {
            width = 0;
            height = 0;
            blockSize = DEFAULT_BLOCK_SIZE;
        }

        public TileWorld(eTileType[,] pWorld)
        {
            blockSize = DEFAULT_BLOCK_SIZE;
            width = pWorld.GetLength(0);
            height = pWorld.GetLength(1);

            //we need to copy values, not references. 
            world = new eTileType[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int i2 = 0; i2 < height; i2++)
                {
                    eTileType TypeClone = eTileType.UNKNOWN;
                    switch (pWorld[i, i2])
                    {
                        case eTileType.SAND:
                            TypeClone = eTileType.SAND;
                            break;
                        case eTileType.NONWALKABLE:
                            TypeClone = eTileType.NONWALKABLE;
                            break;
                        case eTileType.ROAD:
                            TypeClone = eTileType.ROAD;
                            break;
                        case eTileType.START:
                            TypeClone = eTileType.START;
                            break;
                        case eTileType.END:
                            TypeClone = eTileType.END;
                            break;
                        case eTileType.WATER:
                            TypeClone = eTileType.WATER;
                            break;
                        case eTileType.MOUNTAIN:
                            TypeClone = eTileType.MOUNTAIN;
                            break;
                        case eTileType.PATH:
                            TypeClone = eTileType.PATH;
                            break;
                    }
                    world[i, i2] = TypeClone;
                }
            }
            //clear();
        }

        public eTileType[,] getWorld()
        {
            return world;
        }

        private void clear() {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    world[x, y] = eTileType.ROAD;
                }
            }
        }

        public int getWidth() {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public eTileType getTileType(int x, int y)
        {
            if (world == null) {
                return eTileType.UNKNOWN;
            } else {
                return world[x,y];
            }
        }

        public void setTileType(int x, int y, eTileType type)
        {
            if (world != null) {
                world[x, y] = type;
            }
        }

        public void setBlockSize(int blockSize)
        {
            this.blockSize = blockSize;
        }

        public int twoDimIndexToOneDimIndex(int x, int y) {
            return y * width + x;
        }

        public int oneDimToTwoDimXCoordinate(int index)
        {
            return index % width;
        }

        public int oneDimToTwoDimYCoordinate(int index)
        {
            return index / width;
        }

        public int findStartIndex()
        {
            return findIndexContainingType(eTileType.START);
        }

        public int findEndIndex()
        {
            return findIndexContainingType(eTileType.END);
        }
        
        /**
         * Searches the tile world for a tile with a given type.
         * 
         * @return The one-dimensional index of the first tile with the given type. If it has not been found, NO_INDEX is returned.
         */
        private int findIndexContainingType(eTileType type) {
            for (int x = 0; x < getWidth(); x++) {
                for (int y = 0; y < getHeight(); y++) {
                    if (getTileType(x, y) == type) {
                        return twoDimIndexToOneDimIndex(x, y);
                    }
                }
            }
            return -1;
        }
    }
}
