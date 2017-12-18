using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilesOfMonkeyIsland.TileWorld;
using TilesOfMonkeyIsland.Searcher;

namespace TilesOfMonkeyIsland.Algorithm
{
    abstract class Algorithm
    {
        protected TileWorld.TileWorld world;
        protected ArrayList open = new ArrayList();
        protected ArrayList closed = new ArrayList();
        protected ArrayList notDiscoveredYet = new ArrayList();
        protected ArrayList solutionPath = new ArrayList();

        protected Node startNode, goalNode, currentNode;

        public Algorithm(TileWorld.TileWorld world)
        {
            this.world = world;

            // Find the start and goalNode.
            int startX = 0;
            int startY = 0;
            int goalX = 0;
            int goalY = 0;

            for (int x = 0; x < world.getWidth(); x++)
            {
                for (int y = 0; y < world.getHeight(); y++)
                {
                    if (world.getTileType(x, y) == eTileType.START)
                    {
                        startX = x;
                        startY = y;
                    }

                    if (world.getTileType(x, y) == eTileType.END)
                    {
                        goalX = x;
                        goalY = y;
                    }
                }
            }
            startNode = getNode(startX, startY, world.getTileType(startX, startY));
            goalNode = getNode(goalX, goalY, world.getTileType(goalX, goalY));

            startNode.cost = 0;
            startNode.heuristic = calculateHeuristic(startNode);

            open.Add(startNode);
            notDiscoveredYet.Add(goalNode);
        }

        /**
         * Run the algorithm.
         */
        public AlgorithmResults run() {
		while (this.open.Count > 0) {
			this.currentNode = pickLowestHeuristicValue();
			if (notDiscoveredYet.Contains(currentNode)) {
				notDiscoveredYet.Remove(currentNode);
			}
			
			// run the algorithm
			if (done()) {
				// The algorithm is done.
				tracePath(goalNode);
				
				AlgorithmResults info = new AlgorithmResults();
				info.setBestPathCost((int)goalNode.cost);
				info.setNodesExpanded(closed.Count);
				info.setSolutionPath(solutionPath);
				return info;
			} else {
				closed.Add(currentNode);
				open.Remove(currentNode);
				ArrayList neighbors = getNeighbourNodes(currentNode);
				for (int i = 0; i < neighbors.Count; i++) {
                    Node neighbourNode = (Node)neighbors[i];
					if (closed.Contains(neighbourNode)) {
						continue;
					}
					
					neighbourNode.updateNodeIfNeeded(currentNode, currentNode.cost + calculateNodeCost(currentNode, neighbourNode));
					neighbourNode.heuristic = calculateHeuristic(neighbourNode);
					if (!open.Contains(neighbourNode)) {
						open.Add(neighbourNode);
					}
				}
			}
		}

		AlgorithmResults nfo = new AlgorithmResults();
		nfo.setBestPathCost(-1);
		nfo.setNodesExpanded(closed.Count);
		nfo.setSolutionPath(solutionPath);
		return nfo;
	}

        public eTileType[,] getMap()
        {
            return world.getWorld();
        }

        private float calculateNodeCost(Node from, Node to)
        {
            int xDiff = from.x - to.x;
            int yDiff = from.y - to.y;

            if ((xDiff == 1 && (yDiff == 1 || yDiff == -1)) || (xDiff == -1 && (yDiff == 1 || yDiff == -1)))
            {
                return I_Cost.getDiagonalCost(to.type);
            }
            else
            {
                return I_Cost.getCost(to.type);
            }
        }

        /**
         * Returns a list with all the neighbours of the given node.
         * 
         * @param node
         * @return
         */
        protected ArrayList getNeighbourNodes(Node node)
        {
            ArrayList neighbours = new ArrayList();

            /* Straight neightbours */

            // If the node has a left neighbour.
            if (node.x > 0)
            {
                int neighbourX = node.x - 1;
                int neighbourY = node.y;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            // If the node has a right neighbour.
            if (node.x < world.getWidth() - 1)
            {
                int neighbourX = node.x + 1;
                int neighbourY = node.y;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            // If the node has a top neighbour.
            if (node.y > 0)
            {
                int neighbourX = node.x;
                int neighbourY = node.y - 1;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            // If the node has a bottom neighbour.
            if (node.y < world.getHeight() - 1)
            {
                int neighbourX = node.x;
                int neighbourY = node.y + 1;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            /* Diagonal neighbours */

            // If the node has an upper left neighbour.
            if (node.x > 0 && node.y > 0)
            {
                int neighbourX = node.x - 1;
                int neighbourY = node.y - 1;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            // If the node has an upper right neighbour.
            if (node.x < world.getWidth() - 1 && node.y > 0)
            {
                int neighbourX = node.x + 1;
                int neighbourY = node.y - 1;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            // If the node has a bottom left neighbour.
            if (node.x > 0 && node.y < world.getHeight() - 1)
            {
                int neighbourX = node.x - 1;
                int neighbourY = node.y + 1;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            // If the node has a bottom right neighbour.
            if (node.x < world.getWidth() - 1 && node.y < world.getHeight() - 1)
            {
                int neighbourX = node.x + 1;
                int neighbourY = node.y + 1;
                if (world.getTileType(neighbourX, neighbourY) != eTileType.NONWALKABLE)
                    neighbours.Add(getNode(neighbourX, neighbourY, world.getTileType(neighbourX, neighbourY)));
            }

            return neighbours;
        }

        /**
         * Returns the node with the lowest heuristic value from the open list.
         * 
         * @return
         */
        protected Node pickLowestHeuristicValue() {
		Node best = null;
        for (int i = 0; i < open.Count; i++)
        {
            Node node = (Node)open[i];
            //this.printNodes("open", open);
            //this.printNodes("closed", closed);
            if (best == null || node.heuristic < best.heuristic)
            {
                best = node;
            }
        }
		return best;
	}

        /**
         * This method is meant for avoiding duplicate nodes.
         * It checks if a node with the given x, y and type is already been created.
         * If it is the node will be returned, otherwise the node will be created.
         * 
         * @param nodeX
         * @param nodeY
         * @param type
         * @return The existing or new node with the given x, y and type.
         */
        protected Node getNode(int nodeX, int nodeY, eTileType type) {
		// Return the node from the open array if it's in there.
		for (int i = 0; i < open.Count; i++) {
            Node node = (Node)open[i];
			if (node.x == nodeX && node.y == nodeY) {
				return node;
			}
		}
		
		// Return the node from the closed array if it's in there.
		for (int i = 0; i < closed.Count; i++) {
            Node node = (Node)closed[i];
			if (node.x == nodeX && node.y == nodeY) {
				return node;
			}
		}
		
		// Return the node from the notDiscoveredYet array if it's in there.
		for (int i = 0; i < notDiscoveredYet.Count; i++) {
            Node node = (Node)notDiscoveredYet[i];
			if (node.x == nodeX && node.y == nodeY) {
				return node;
			}
		}
		
		// At this point it's clear the node hasn't been encounteren before so create and return it.
		return new Node(nodeX - 1, nodeY - 1, type);
	}

        protected void printNodes(String title, ArrayList nodes) {
		Console.WriteLine("---------NODES: " + title + "-------------\r\n");
		Console.WriteLine("SIZE: " + nodes.Count + "\r\n");
        for (int i = 0; i < nodes.Count; i++ )
        {
            Node node = (Node)nodes[i];
            Console.WriteLine("NODE x: " + node.x + " y: " + node.y + " heuristic: " + node.heuristic + " type: " + node.type.ToString() + "\r\n");
        }
		Console.WriteLine("---------------------\r\n");
	}

        /**
         * Calculates the heuristic value of a node.
         * This method allows for different algorithms to be derived from the Algorithm class.
         * 
         * @param node
         * @return
         */
        protected abstract float calculateHeuristic(Node node);

        /**
         * Checks if the solution-found condition is met.
         * @return
         */
        protected virtual bool done()
        {
            return currentNode == goalNode;
        }

        /**
         * Prints out the currently known path to the given node.
         * If the path is the optimal path depends on the calculateHeuristic value.
         * 
         * @param node
         */
        protected void tracePath(Node node)
        {
            this.world.setTileType(node.x, node.y, eTileType.PATH);
            if (node.parent == null || node.type == eTileType.START)
            {
                this.solutionPath.Add(world.twoDimIndexToOneDimIndex(node.x, node.y));
                return;
            }
            else
            {
                tracePath(node.parent);
                this.solutionPath.Add(world.twoDimIndexToOneDimIndex(node.x, node.y));
            }
        }
    }
}
