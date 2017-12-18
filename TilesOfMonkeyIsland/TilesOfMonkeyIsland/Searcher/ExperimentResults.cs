using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilesOfMonkeyIsland.TileWorld;

namespace TilesOfMonkeyIsland.Searcher
{
    class ExperimentResults
    {
        /**
     * The following three variables contain information on results of the 
     * searches by the three different algorithms
     */
        private AlgorithmResults aStar;
        private AlgorithmResults dijkstra;
        private AlgorithmResults bfsSearch;

        /**
         * Initializes the three algorithm results.
         */
        public ExperimentResults()
        {
            aStar = new AlgorithmResults();
            dijkstra = new AlgorithmResults();
            bfsSearch = new AlgorithmResults();
        }

        public AlgorithmResults getaStar()
        {
            return aStar;
        }

        public void setaStar(AlgorithmResults aStar)
        {
            this.aStar = aStar;
        }

        public AlgorithmResults getDijkstra()
        {
            return dijkstra;
        }

        public void setDijkstra(AlgorithmResults dijkstra)
        {
            this.dijkstra = dijkstra;
        }

        public AlgorithmResults getBFSSearch()
        {
            return bfsSearch;
        }

        public void setBFSSearch(AlgorithmResults bfsSearch)
        {
            this.bfsSearch = bfsSearch;
        }
    }
}
