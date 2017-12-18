using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;
using TilesOfMonkeyIsland.TileWorld;
using TilesOfMonkeyIsland.Algorithm;

namespace TilesOfMonkeyIsland.Searcher
{
    class Searcher
    {
        public static void start(int howMany) {
        String filename;

        eTileType[,] map;
        // search all files
        for (int fileNr = 1; fileNr <= howMany; fileNr++) {
            // determine file to search
            filename = "i" + fileNr;
            Image rawmap;
            try
            {
                rawmap = Image.FromFile( @"..\..\" + filename + ".png");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine("FILE NOT FOUND!");
                Console.WriteLine(ex);
                System.Threading.Thread.Sleep(5000);
                return;
            }
            
            Bitmap bm = (Bitmap)rawmap;
            map = new eTileType[rawmap.Width,rawmap.Height];
            for (int i = 0; i < rawmap.Width; i++)
            {
                for (int i2 = 0; i2 < rawmap.Height; i2++)
                {
                    Color pixel = bm.GetPixel(i,i2);
                    eTileType pixelType = eTileType.UNKNOWN;
                    switch (pixel.Name)
                    {
                        case "ffffff00"://yellow
                            pixelType = eTileType.SAND;
                            break;
                        case "ff000000"://black
                            pixelType = eTileType.NONWALKABLE;
                            break;
                        case "ffffffff"://white
                            pixelType = eTileType.ROAD;
                            break;
                        case "ffff0000"://red
                            pixelType = eTileType.START;
                            break;
                        case "ff00ff00"://green
                            pixelType = eTileType.END;
                            break;
                        case "ff0000ff"://blue
                            pixelType = eTileType.WATER;
                            break;
                        case "ff808080"://gray
                            pixelType = eTileType.MOUNTAIN;
                            break;
                        case "ff00ffff"://cyan
                            pixelType = eTileType.PATH;
                            break;
                    }
                    map[i,i2] = pixelType;
                }
            }
            

            // search tile world
            ExperimentResults info = search(map, filename);

            // print the results to System.out
            printAllResults(filename, info);
        }
        Console.WriteLine("Press Enter to close the application.");
        Console.ReadLine();
    }

        /**
         * Searches a file for the best path using three algorithms. The solutions
         * can be shown on screen.
         * @param filename The file (without extension) to be searched.
         * @param showSolutions Indicates if solutions should be shown on screen.
         * @return All relevant statistics for the three experiments.
         */
        public static ExperimentResults search(eTileType[,] map, String filename)
        {
            ExperimentResults info = new ExperimentResults();

            // Dijkstra
            Dijkstra dijkstraAlgorithm = new Algorithm.Dijkstra(new TileWorld.TileWorld(map));
            AlgorithmResults dijkstra = dijkstraAlgorithm.run();

            info.setDijkstra(dijkstra);
            saveImage(filename + "_dijkstra", dijkstraAlgorithm.getMap());

            // A*
            AStar aStarAlgorithm = new AStar(new TileWorld.TileWorld(map));
            AlgorithmResults aStar = aStarAlgorithm.run();

            info.setaStar(aStar);
            saveImage(filename + "_aStar", aStarAlgorithm.getMap());

            // BFS Search
            BFS bfsAlgorithm = new BFS(new TileWorld.TileWorld(map));
            AlgorithmResults bfsSearch = bfsAlgorithm.run();

            info.setBFSSearch(bfsSearch);
            saveImage(filename + "_bfs", bfsAlgorithm.getMap());

            return info;
        }

        /**
         * Prints the results for all three algorithms on System.out.
         *
         * @param filename Filename containing the world that has been searched.
         * @param info Results of the experiments for all three algorithms for that file.
         */
        public static void printAllResults(String filename, ExperimentResults info) {
            Console.WriteLine("#######################");
            Console.WriteLine("Testcase: " + filename);
            Console.WriteLine("#######################");
            printAlgorithmResult("A*", info.getaStar());
            Console.WriteLine("-------------------------------------");
            printAlgorithmResult("Dijkstra", info.getDijkstra());
            Console.WriteLine("-------------------------------------");
            printAlgorithmResult("BFS", info.getBFSSearch());
        }

        public static void saveImage(String originalFileName, eTileType[,] map)
        {
            String newFileName = originalFileName + "_solution";
            String saveLocation = @"..\..\" + newFileName + ".png";
            Bitmap bm = new Bitmap(map.GetUpperBound(0)+1,map.GetUpperBound(1)+1);
            for (int i = 0; i < map.GetUpperBound(0)+1; i++)
            {
                for (int i2 = 0; i2 < map.GetUpperBound(1)+1; i2++)
                {
                    Color color;
                    switch (map[i,i2])
                    {
                        case eTileType.SAND://yellow
                            color = Color.FromArgb(255,255,0);
                            break;
                        case eTileType.NONWALKABLE://black
                            color = Color.FromArgb(0,0,0);
                            break;
                        case eTileType.ROAD://white
                            color = Color.FromArgb(255,255,255);
                            break;
                        case eTileType.START://red
                            color = Color.FromArgb(255,0,0);
                            break;
                        case eTileType.END://green
                            color = Color.FromArgb(0,255,0);
                            break;
                        case eTileType.WATER://blue
                            color = Color.FromArgb(0,0,255);
                            break;
                        case eTileType.MOUNTAIN://gray
                            color = Color.FromArgb(128,128,128);
                            break;
                        case eTileType.PATH://cyan
                            color = Color.FromArgb(0,255,255);
                            break;
                        default://unknown = purple
                            color = Color.FromArgb(255,0,255);
                            break;
                    }
                    bm.SetPixel(i,i2,color);
                }
            }
            bm.Save(saveLocation);
        }

            /**
             * Prints the results of one algorithm on System.out.
             *
             * @param algorithmString Name of the algorithm used (A*, Dijkstra, ...).
             * @param info Results of the algorithm.
             */
            private static void printAlgorithmResult(String algorithmString, AlgorithmResults info) {
            Console.WriteLine(algorithmString);
            if (info == null) {
                Console.WriteLine("No results found.");
                return;
            }
            Console.WriteLine("#nodes: " + info.getNodesExpanded());
            Console.WriteLine("#path cost: " + info.getBestPathCost());
        }
    }
}
