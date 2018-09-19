using System;
using System.Linq;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace XTB.App
{
    class Program
    {
        private static Server serverData = Servers.REAL;
        private static string userId = "10332876";
        private static string password = "Policja8";

        static void Main(string[] args)
        {
            //serverData.
            var ser = new Server("xapia.x-station.eu", 5124, 5125, true, Environment.MachineName);
            SyncAPIConnector connector = new SyncAPIConnector(ser);
            Console.WriteLine("Connected to the server");

            // Login to server
            Credentials credentials = new Credentials(userId, password, "", "YOUR APP NAME");

            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);
            Console.WriteLine("Logged in as: " + userId);

            // Execute GetServerTime command
            ServerTimeResponse serverTimeResponse = APICommandFactory.ExecuteServerTimeCommand(connector, true);
            Console.WriteLine("Server time: " + serverTimeResponse.TimeString);

            // Execute GetAllSymbols command
            AllSymbolsResponse allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(connector, true);
            Console.WriteLine("All symbols count: " + allSymbolsResponse.SymbolRecords.Count);

            // Print first 5 symbols
            Console.WriteLine("First five symbols:");
            foreach (SymbolRecord symbolRecord in allSymbolsResponse.SymbolRecords.Take(5))
            {
                Console.WriteLine(" > " + symbolRecord.Symbol + " ask: " + symbolRecord.Ask + " bid: " + symbolRecord.Bid);
            }

            Console.Read();
        }
    }
}
