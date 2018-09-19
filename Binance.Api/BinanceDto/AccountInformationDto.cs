using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class AccountInformationDto
    {
        public int MakerCommission { get; set; }
        public int TakerCommission { get; set; }
        public int BuyerCommission { get; set; }
        public int SellerCommission { get; set; }
        public bool CanTrade { get; set; }
        public bool CanWithdraw { get; set; }
        public bool CanDeposit { get; set; }
        public long UpdateTime { get; set; }
        public IEnumerable<BalanceDto> GetBalance { get; set; }
  
    }
}
