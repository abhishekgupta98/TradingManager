using System;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace TradingManager.Database
{
    public class DatabaseManager
    {
        // Database file path
//        private string dbPath = "tradingmanager.db";
  
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tradingmanager.db");
        private string connectionString;

        public DatabaseManager()
        {
            // Create connection string
        
        //private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tradingmanager.db");

        connectionString = $"Data Source={dbPath};";
        }

        // Method to initialize database (create tables)
        public void InitializeDatabase()
        {
            try
            {

                // Check if database file exists
                //  if (!File.Exists(dbPath))
                //  {
                // Create new database
                //     SQLiteConnection.CreateFile(dbPath);
                //     Console.WriteLine("Database file created!");
                // }


                string message = "";
                message += $"Starting database initialization...\n";
                message += $"Database path: {dbPath}\n";
                message += $"Full path: {Path.GetFullPath(dbPath)}\n\n";


                // Create all tables
                CreateTables();

                message += "Database initialized successfully!\n\n";

                // Check if file exists
                if (File.Exists(dbPath))
                {
                    FileInfo fileInfo = new FileInfo(dbPath);
                    message += $"✓ Database file created!\n";
                    message += $"✓ Location: {Path.GetFullPath(dbPath)}\n";
                    message += $"✓ File size: {fileInfo.Length} bytes";
                }
                else
                {
                    Console.WriteLine("✗ Database file was NOT created!");
                }

                MessageBox.Show(message, "Database Initialization", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: {ex.Message}\n\n{ex.StackTrace}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to create all tables
        private void CreateTables()
        {
            //  using (SQLiteConnection conn = new SQLiteConnection(connectionString))  // this is normal one. 

                using (SqliteConnection conn = new SqliteConnection(connectionString))  // this is microsoft 
                
            {
                conn.Open();

                // Create Accounts table
                CreateAccountsTable(conn);

                // Create Trades table
                CreateTradesTable(conn);

                // Create Commissions table
                CreateCommissionsTable(conn);

                // Create Alerts table
                CreateAlertsTable(conn);

                // Create Prices table
                CreatePricesTable(conn);

                // Create Messages table
                CreateMessagesTable(conn);

                conn.Close();
            }
        }

        // Create Accounts Table
        private void CreateAccountsTable(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Accounts (
                    AccountId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientName TEXT NOT NULL,
                    BrokerName TEXT NOT NULL,
                    Balance REAL NOT NULL,
                    Equity REAL NOT NULL,
                    Leverage REAL NOT NULL,
                    Status TEXT NOT NULL,
                    CreatedDate TEXT NOT NULL
                )";

            ExecuteSQL(conn, sql);
           // Console.WriteLine("✓ Accounts table created");
        }

        // Create Trades Table
        private void CreateTradesTable(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Trades (
                    TradeId INTEGER PRIMARY KEY AUTOINCREMENT,
                    AccountId INTEGER NOT NULL,
                    Symbol TEXT NOT NULL,
                    Direction TEXT NOT NULL,
                    Quantity REAL NOT NULL,
                    EntryPrice REAL NOT NULL,
                    ExitPrice REAL,
                    Commission REAL NOT NULL,
                    OpenTime TEXT NOT NULL,
                    CloseTime TEXT,
                    Status TEXT NOT NULL,
                    ProfitLoss REAL,
                    FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId)
                )";

            ExecuteSQL(conn, sql);
           //Console.WriteLine("✓ Trades table created");
        }

        // Create Commissions Table
        private void CreateCommissionsTable(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Commissions (
                    CommissionId INTEGER PRIMARY KEY AUTOINCREMENT,
                    AccountId INTEGER NOT NULL,
                    TradeId INTEGER NOT NULL,
                    Amount REAL NOT NULL,
                    SpreadCost REAL NOT NULL,
                    DateCharged TEXT NOT NULL,
                    FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId),
                    FOREIGN KEY (TradeId) REFERENCES Trades(TradeId)
                )";

            ExecuteSQL(conn, sql);
         //   Console.WriteLine("✓ Commissions table created");
        }

        // Create Alerts Table
        private void CreateAlertsTable(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Alerts (
                    AlertId INTEGER PRIMARY KEY AUTOINCREMENT,
                    AccountId INTEGER NOT NULL,
                    Symbol TEXT NOT NULL,
                    TargetPrice REAL NOT NULL,
                    Status TEXT NOT NULL,
                    CreatedDate TEXT NOT NULL,
                    TriggeredDate TEXT,
                    FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId)
                )";

            ExecuteSQL(conn, sql);
         // Console.WriteLine("✓ Alerts table created");
        }

        // Create Prices Table
        private void CreatePricesTable(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Prices (
                    PriceId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Symbol TEXT NOT NULL UNIQUE,
                    Bid REAL NOT NULL,
                    Ask REAL NOT NULL,
                    Spread REAL NOT NULL,
                    LastUpdate TEXT NOT NULL
                )";

            ExecuteSQL(conn, sql);
         //   Console.WriteLine("✓ Prices table created");
        }

        // Create Messages Table
        private void CreateMessagesTable(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS Messages (
                    MessageId INTEGER PRIMARY KEY AUTOINCREMENT,
                    FromManager TEXT NOT NULL,
                    ToAccountId INTEGER NOT NULL,
                    MessageText TEXT NOT NULL,
                    SentDate TEXT NOT NULL,
                    IsRead INTEGER NOT NULL,
                    FOREIGN KEY (ToAccountId) REFERENCES Accounts(AccountId)
                )";

            ExecuteSQL(conn, sql);
         //   Console.WriteLine("✓ Messages table created");
        }

        // Helper method to execute SQL
        private void ExecuteSQL(SqliteConnection conn, string sql)
        {
            try
            {
                using (SqliteCommand cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL: {ex.Message}");
            }
        }

        // Method to get connection string
        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}