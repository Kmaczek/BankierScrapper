using Binance.Api.BinanceDto;
using Newtonsoft.Json;
using System;

namespace Binance.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new BinanceClient();
            var response = client.Ping();
            //var timeResponse = client.ServerTime();
            //var exchange = client.ExchangeInfo();
            var accountInfo = client.AccountInfo();

            Console.ReadLine();
        }
    }
}
