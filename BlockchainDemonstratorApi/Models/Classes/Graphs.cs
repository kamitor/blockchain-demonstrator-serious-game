using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockchainDemonstratorApi.Models.Classes
{
    public static class Graphs
    {
        public static string CreateLabels<T>(List<T> list)
        {
            string labels = "[";
            for (int i = 0; i < list.Count; i++)
            {
                labels += "'Round " + (i + 1) + "'";
                if (list.Count - 1 > i) labels += ", ";
                else labels += "]";
            }
            return labels;
        }

        public static string CreateData<T>(List<T> list)
        {
            string data = "[";
            for (int i = 0; i < list.Count; i++)
            {
                data += "'" + list[i].ToString().Replace(',', '.') + "'";
                if (list.Count - 1 > i) data += ", ";
                else data += "]";
            }
            return data;
        }

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
