using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using TradingManager.Database;

namespace TradingManager.Repositories
{
    public class AccountRepository
    {
        private string connectionString;

        public AccountRepository()
        {
            // Get connection string from DatabaseManager
            DatabaseManager dbManager = new DatabaseManager();
            connectionString = dbManager.GetConnectionString();
        }

        // CREATE: Add new account to database
        public void AddAccount(Account account)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO Accounts (ClientName, BrokerName, Balance, Equity, Leverage, Status, CreatedDate)
                        VALUES (@clientName, @brokerName, @balance, @equity, @leverage, @status, @createdDate)";

                    using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientName", account.AccountName);
                        cmd.Parameters.AddWithValue("@brokerName", account.BrokerName);
                        cmd.Parameters.AddWithValue("@balance", account.Balance);
                        cmd.Parameters.AddWithValue("@equity", account.Equity);
                        cmd.Parameters.AddWithValue("@leverage", account.Leverage);
                        cmd.Parameters.AddWithValue("@status", "Active");
                        cmd.Parameters.AddWithValue("@createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                Console.WriteLine($"✓ Account '{account.AccountName}' added to database");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding account: {ex.Message}");
            }
        }

        // READ: Get account by ID
        public Account GetAccountById(int accountId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM Accounts WHERE AccountId = @accountId";

                    using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@accountId", accountId);

                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Account account = new Account();
                                account.AccountId = (int)reader["AccountId"];
                                account.AccountName = (string)reader["ClientName"];
                                account.BrokerName = (string)reader["BrokerName"];
                                account.Balance = (decimal)(double)reader["Balance"];
                                account.Equity = (decimal)(double)reader["Equity"];
                                account.Leverage = (decimal)(double)reader["Leverage"];
                                account.CreatedDate = DateTime.Parse((string)reader["CreatedDate"]);

                                return account;
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting account: {ex.Message}");
            }

            return null;
        }

        // READ: Get all accounts
        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();

            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM Accounts";

                    using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account();

                                // Read values safely
                                account.AccountId = Convert.ToInt32(reader["AccountId"]);
                                account.AccountName = reader["ClientName"].ToString();
                                account.BrokerName = reader["BrokerName"].ToString();
                                account.Balance = Convert.ToDecimal(reader["Balance"]);
                                account.Equity = Convert.ToDecimal(reader["Equity"]);
                                account.Leverage = Convert.ToDecimal(reader["Leverage"]);

                                if (reader["CreatedDate"] != DBNull.Value)
                                {
                                    account.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
                                }

                                accounts.Add(account);

                                Console.WriteLine($"✓ Read account: {account.AccountName}");
                            }
                        }
                    }

                    conn.Close();
                }
                Console.WriteLine($"Total accounts retrieved: {accounts.Count}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all accounts: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return accounts;
        }

        // UPDATE: Update existing account
        public void UpdateAccount(Account account)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"
                        UPDATE Accounts 
                        SET ClientName = @clientName, 
                            Balance = @balance, 
                            Equity = @equity, 
                            Leverage = @leverage 
                        WHERE AccountId = @accountId";

                    using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@accountId", account.AccountId);
                        cmd.Parameters.AddWithValue("@clientName", account.AccountName);
                        cmd.Parameters.AddWithValue("@balance", account.Balance);
                        cmd.Parameters.AddWithValue("@equity", account.Equity);
                        cmd.Parameters.AddWithValue("@leverage", account.Leverage);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                Console.WriteLine($"✓ Account '{account.AccountName}' updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating account: {ex.Message}");
            }
        }

        // DELETE: Delete account by ID
        public void DeleteAccount(int accountId)
        {
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    string sql = "DELETE FROM Accounts WHERE AccountId = @accountId";

                    using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@accountId", accountId);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                Console.WriteLine($"✓ Account deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting account: {ex.Message}");
            }
        }
    }
}