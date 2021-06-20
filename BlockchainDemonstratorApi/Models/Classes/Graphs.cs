using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    /// <summary>
    /// The Graphs class is used to make creating graphs a lot easier and efficient.
    /// </summary>
    /// <remarks>
    /// With the end of the development cycle approaching, reconsiderations of this class were thought of. 
    /// Because the data can be send through as JSON objects between the C# code and JS. 
    /// This would deprecate most of these methods in this class.
    /// If need be to change the use of the graphs, considering JSON as a form of transportation can be a more suffecient option.
    /// </remarks>
    public static class Graphs
    {
        /// <summary>
        /// This method creates the labels of the graph in rounds.
        /// </summary>
        /// <typeparam name="T">The type of the object list</typeparam>
        /// <param name="list">List of objects used in the graph</param>
        /// <returns>Returns a Javascript array in the form of a string.</returns>
        public static string CreateLabels<T>(List<T> list)
        {
            string labels = "[";
            for (int i = 0; i < list.Count; i++)
            {
                labels += "'Round " + (i + 1) + "'";
                if (list.Count - 1 > i) labels += ", ";
            }
            labels += "]";
            return labels;
        }

        /// <summary>
        /// This function creates the data array for the graph.
        /// </summary>
        /// <typeparam name="T">The type of the object list.</typeparam>
        /// <param name="list">List of objects used in the graph.</param>
        /// <returns>Returns a Javascript array in the form of a string.</returns>
        public static string CreateData<T>(List<T> list)
        {
            string data = "[";
            for (int i = 0; i < list.Count; i++)
            {
                data += "'" + list[i].ToString().Replace(',', '.') + "'";
                if (list.Count - 1 > i) data += ", ";
            }
            data += "]";
            return data;
        }

        /// <summary>
        /// This method creates a data set for the graphs. Each dataset is represented as one line in the graph.
        /// </summary>
        /// <param name="data">The string of data created with the CreateData function.</param>
        /// <param name="name">The name that should represent the line.</param>
        /// <param name="lineColour">The colour of the line in the graph.</param>
        /// <returns>Returns a dataset object literal in as a string.</returns>
        public static string CreateDataSet(string data, string name, string lineColour)
        {
            return @"{
                    label: '" + name + @"',
                    data: " + data + @",
                    fill: true,
                    backgroundColor: 'rgba(0, 0, 0, 0)',
                    borderColor: '" + lineColour + @"',
                    borderWidth: 1
                }";
        }

        public static string CreateInventoryDataSets(List<Player> players)
        {
            string dataSets = "[";
            for (int i = 0; i < players.Count; i++)
            {
                if(i == 0) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].InventoryHistory), players[i].Name, "rgba(255, 0, 0, 1)") + ",";
                if(i == 1) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].InventoryHistory), players[i].Name, "rgba(0, 255, 0, 1)") + ",";
                if(i == 2) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].InventoryHistory), players[i].Name, "rgba(0, 0, 255, 1)") + ",";
                if(i == 3) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].InventoryHistory), players[i].Name, "rgba(255, 255, 0, 1)") + "]";
            }

            return dataSets;
        }

        public static string CreateOrderWorthDataSets(List<Player> players)
        {
            string dataSets = "[";
            for (int i = 0; i < players.Count; i++)
            {
                if (i == 0) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OrderWorthHistory), players[i].Name, "rgba(255, 0, 0, 1)") + ",";
                if (i == 1) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OrderWorthHistory), players[i].Name, "rgba(0, 255, 0, 1)") + ",";
                if (i == 2) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OrderWorthHistory), players[i].Name, "rgba(0, 0, 255, 1)") + ",";
                if (i == 3) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OrderWorthHistory), players[i].Name, "rgba(255, 255, 0, 1)") + "]";
            }

            return dataSets;
        }

        public static string CreateOverallProfitDataSets(List<Player> players)
        {
            string dataSets = "[";
            for (int i = 0; i < players.Count; i++)
            {
                if (i == 0) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OverallProfitHistory), players[i].Name, "rgba(255, 0, 0, 1)") + ",";
                if (i == 1) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OverallProfitHistory), players[i].Name, "rgba(0, 255, 0, 1)") + ",";
                if (i == 2) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OverallProfitHistory), players[i].Name, "rgba(0, 0, 255, 1)") + ",";
                if (i == 3) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].OverallProfitHistory), players[i].Name, "rgba(255, 255, 0, 1)") + "]";
            }

            return dataSets;
        }

        public static string CreateGrossProfitDataSets(List<Player> players)
        {
            string dataSets = "[";
            for (int i = 0; i < players.Count; i++)
            {
                if (i == 0) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].GrossProfitHistory), players[i].Name, "rgba(255, 0, 0, 1)") + ",";
                if (i == 1) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].GrossProfitHistory), players[i].Name, "rgba(0, 255, 0, 1)") + ",";
                if (i == 2) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].GrossProfitHistory), players[i].Name, "rgba(0, 0, 255, 1)") + ",";
                if (i == 3) dataSets += Graphs.CreateDataSet(Graphs.CreateData(players[i].GrossProfitHistory), players[i].Name, "rgba(255, 255, 0, 1)") + "]";
            }

            return dataSets;
        }
    }
}
