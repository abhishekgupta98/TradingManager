using System;
using System.Collections.Generic;
using System.Text;

namespace TradingManager
{
    public class CommissionEngine
    {
        public string BrokerName { get; set; }
        public decimal CommissionPerLot { get; set; }
        public decimal MinimumCommission { get; set; }
        public decimal LotSize { get; set; }
        public Dictionary<string , decimal> PairSpreads { get; set; }


        public CommissionEngine(string brokerName, decimal commissionPerLot, decimal minimumCommission, decimal lotSize)
        {
            BrokerName = brokerName;
            CommissionPerLot = commissionPerLot;
            MinimumCommission = minimumCommission;
            LotSize = lotSize;
            PairSpreads = new Dictionary<string, decimal>();
        }

        public void AddPairSpread(string symbol, decimal spread)
        {
            PairSpreads[symbol] = spread;
        }

        public decimal CalculateCommission(decimal quantity)
        {
            decimal commission = (quantity / LotSize) * CommissionPerLot;

            return Math.Max(commission, MinimumCommission);
        }
         public decimal CalculateSpread(string symbol , decimal quantity)
        {
            if (PairSpreads.TryGetValue(symbol, out decimal spread))  //  decimal spread = PairSpreads[symbol]; 
            {
                return spread * quantity ;
            }
            return 0; // Default spread if not found
        }

        public decimal CalculateTotalCost(decimal quantity, string symbol)
        {
            decimal commission = CalculateCommission(quantity);
            decimal spreadCost = CalculateSpread(symbol, quantity);
            return commission + spreadCost;
        }

    }

    
}
