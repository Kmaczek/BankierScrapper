using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Common
{
    public class ConfigProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BankierUrlWithParameters
        {
            get {  return _configuration["BankierScrapper:FullUrl"]; }
        }
    }
}
