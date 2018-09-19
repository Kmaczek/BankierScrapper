using RestSharp;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Binance.Api.BinanceDto;
using Newtonsoft.Json.Serialization;
using System.Resources;

namespace Binance.Api
{
    public class BinanceClient
    {
        private string url = $"https://api.binance.com";
        private readonly string secret;
        private readonly string apiKey;
        private RestClient client;

        public RestClient Client
        {
            get
            {
                if(client == null)
                {
                    client = new RestClient(url);
                }

                return client;
            }
        }

        public JsonSerializerSettings CamelCaseResolver
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            }
        }

        public BinanceClient()
        {
            apiKey = Resource.apiKey;
            secret = Resource.secret;
        }

        public bool Ping()
        {
            var request = new RestRequest("/api/v1/ping", Method.GET);
            IRestResponse response = Client.Execute(request);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public ServerTimeDto ServerTime()
        {
            var request = new RestRequest("/api/v1/time", Method.GET);
            IRestResponse response = Client.Execute(request);

            var serverTime = JsonConvert.DeserializeObject<ServerTimeDto>(response.Content);
            return serverTime;
        }

        public ExchangeInfoDto ExchangeInfo()
        {
            var request = new RestRequest("/api/v1/exchangeInfo", Method.GET);
            IRestResponse response = Client.Execute(request);

            var exchangeInfo = JsonConvert.DeserializeObject<ExchangeInfoDto>(response.Content, CamelCaseResolver);
            return exchangeInfo;
        }

        public ExchangeInfoDto OrderBook()
        {
            var request = new RestRequest("/api/v1/depth", Method.GET);
            IRestResponse response = Client.Execute(request);

            var exchangeInfo = JsonConvert.DeserializeObject<ExchangeInfoDto>(response.Content, CamelCaseResolver);
            return exchangeInfo;
        }

        public TradeDto HistoricalTrades()
        {
            var request = new RestRequest("/api/v1/historicalTrades", Method.GET);
            IRestResponse response = Client.Execute(request);

            var trade = JsonConvert.DeserializeObject<TradeDto>(response.Content, CamelCaseResolver);
            return trade;
        }

        public AccountInformationDto AccountInfo()
        {
            var request = new RestRequest("/api/v3/account", Method.GET);
            request.AddHeader("X-MBX-APIKEY", apiKey);
            //request.AddOrUpdateParameter(new Parameter() { Name = "timestamp", Value = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(), Type = ParameterType.QueryString});
            request.AddOrUpdateParameter(new Parameter() { Name = "timestamp", Value = ServerTime().ServerTimestamp, Type = ParameterType.QueryString });
            //request.AddOrUpdateParameter("recvWindow", "5000", ParameterType.QueryString);

            var uri = Client.BuildUri(request);
            var query = uri.Query.Trim('?');
            request.AddOrUpdateParameter("signature", HmacSha256Digest(query, secret), ParameterType.QueryString);

            IRestResponse response = Client.Execute(request);

            if(!response.IsSuccessful)
            {
                Console.WriteLine($"Status {response.StatusCode}: {response.StatusDescription}");
                var error = JsonConvert.DeserializeObject<ErrorMessageDto>(response.Content, CamelCaseResolver);
                Console.WriteLine($"Error: {error.Code} - {error.Msg}");

                return null;
            }

            var trade = JsonConvert.DeserializeObject<AccountInformationDto>(response.Content, CamelCaseResolver);
            return trade;
        }

        public static string HmacSha256Digest(string message, string secret)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            System.Security.Cryptography.HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }


        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }

        private static string HashHMACHex(string keyHex, string message)
        {
            byte[] hash = HashHMAC(HexDecode(keyHex), StringEncode(message));
            return HashEncode(hash);
        }
    }
}
