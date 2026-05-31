using System;
using System.Collections.Generic;

namespace TradingManager
{
    public class Account
    {
        // Properties (data members)
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal Equity { get; set; }
        public decimal UsedMargin { get; set; }
        public decimal FreeMargin { get; set; }
        public decimal Leverage { get; set; }
        public List<Trade> Trades { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BrokerName { get; set; }

        // Constructor (default)
        public Account()
        {
            AccountId = 0;
            AccountName = "Unnamed Account";  // Default name
            BrokerName = "Unknown";            // Default broker
            Balance = 0;
            Equity = 0;
            FreeMargin = 0;
            UsedMargin = 0;
            Leverage = 1;
            Trades = new List<Trade>();
            CreatedDate = DateTime.Now;
        }

        // Constructor with parameters
        public Account(int accountId, string accountName, decimal balance, string brokerName)
        {
            AccountId = accountId;
            AccountName = accountName;
            Balance = balance;
            Equity = balance;
            FreeMargin = balance;
            BrokerName = brokerName;
            Trades = new List<Trade>();
            CreatedDate = DateTime.Now;
        }

        // Method to add a trade
        public void AddTrade(Trade trade)
        {
            Trades.Add(trade);
            UpdateEquity();
        }

        // Method to calculate total profit/loss
        public decimal GetTotalProfitLoss()
        {
            decimal totalPL = 0;
            foreach (var trade in Trades)
            {
                totalPL += trade.ProfitLoss;
            }
            return totalPL;
        }

        // Method to update equity
        public void UpdateEquity()
        {
            Equity = Balance + GetTotalProfitLoss();
            FreeMargin = Equity - UsedMargin;
        }

        // Method to display account info
        public override string ToString()
        {
            return $"Account: {AccountName} | Balance: ${Balance:F2} | Equity: ${Equity:F2} | Trades: {Trades.Count}";
        }
    }
}