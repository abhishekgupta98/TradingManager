using System;

namespace TradingManager
{
    public class Trade
    {
        public int TradeId { get; set; }
        public string Symbol { get; set; }
        public string Direction { get; set; }
        public decimal Quantity { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
        public decimal ProfitLoss { get; set; }
        public decimal Commission { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string Status { get; set; }

        // Constructor
        public Trade()
        {
            TradeId = 0;
            Symbol = "UNKNOWN";
            Direction = "UNKNOWN";
            Quantity = 0;
            EntryPrice = 0;
            ExitPrice = 0;
            ProfitLoss = 0;
            Commission = 0;
            Status = "OPEN";
            EntryTime = DateTime.Now;
            ExitTime = null;  // Not closed yet
        
        }

        // Method to close trade
        public void CloseTrade(decimal exitPrice)
        {
            ExitPrice = exitPrice;
            ExitTime = DateTime.Now;
            Status = "CLOSED";
            CalculateProfitLoss();
        }

        // Method to calculate profit/loss
        public void CalculateProfitLoss()
        {
            if (Direction.ToUpper() == "BUY")
            {
                ProfitLoss = (ExitPrice - EntryPrice) * Quantity - Commission;
            }
            else if (Direction.ToUpper() == "SELL")
            {
                ProfitLoss = (EntryPrice - ExitPrice) * Quantity - Commission;
            }
        }

        // Display trade info
        public override string ToString()
        {
            return $"{Symbol} {Direction} {Quantity} @ {EntryPrice:F5} | P&L: ${ProfitLoss:F2}";
        }
    }
}