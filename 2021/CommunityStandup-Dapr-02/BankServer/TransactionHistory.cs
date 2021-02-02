using System;

namespace BankServer
{
    public class TransactionHistory
    {
        public string Username { get; set; }

        public decimal StartingBalance { get; set; }

        public decimal EndingBalance { get; set; }

        public DateTime Time { get; set; }
    }
}