

using TradingManager.Database;
using TradingManager.Repositories;
using System.Windows.Forms;

namespace TradingManager;


static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {

        // Initialize database FIRST
        DatabaseManager dbManager = new DatabaseManager();
        dbManager.InitializeDatabase();


        // Create a new account
        CreateNewAccount();



        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

    // Create new account - gets input from user
    static void CreateNewAccount()
    {
        try
        {
            string message = "=== ADD NEW ACCOUNT ===\n\n";

            // Get input from user
            string accountName = PromptUser("Enter Account Name:", "My Trading Account");
            if (accountName == null) return;

            string brokerName = PromptUser("Enter Broker Name:", "EXNESS");
            if (brokerName == null) return;

            string balanceStr = PromptUser("Enter Initial Balance ($):", "5000");
            if (balanceStr == null) return;

            string leverageStr = PromptUser("Enter Leverage (50, 100, 200):", "50");
            if (leverageStr == null) return;

            // Validate inputs
            if (!decimal.TryParse(balanceStr, out decimal balance))
            {
                MessageBox.Show("Invalid balance amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(leverageStr, out decimal leverage))
            {
                MessageBox.Show("Invalid leverage amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            message += "=== CREATING ACCOUNT ===\n\n";
            message += $"Account Name: {accountName}\n";
            message += $"Broker: {brokerName}\n";
            message += $"Balance: ${balance:F2}\n";
            message += $"Leverage: {leverage}:1\n\n";

            // CREATE: Save account using AccountRepository
            Account newAccount = new Account(0, accountName, balance, brokerName);
            newAccount.Leverage = leverage;

            AccountRepository repo = new AccountRepository();
            repo.AddAccount(newAccount);

            message += "✓ Account saved to database!\n\n";

            // READ: Retrieve all accounts
            message += "=== ALL ACCOUNTS IN DATABASE ===\n\n";
            var allAccounts = repo.GetAllAccounts();
            message += $"Total Accounts: {allAccounts.Count}\n\n";

            // DISPLAY: Show all accounts
            foreach (var account in allAccounts)
            {
                message += $"─────────────────────────\n";
                message += $"ID: {account.AccountId}\n";
                message += $"Name: {account.AccountName}\n";
                message += $"Broker: {account.BrokerName}\n";
                message += $"Balance: ${account.Balance:F2}\n";
                message += $"Equity: ${account.Equity:F2}\n";
                message += $"Leverage: {account.Leverage}:1\n";
                message += $"Created: {account.CreatedDate:yyyy-MM-dd HH:mm:ss}\n";
            }
            message += $"─────────────────────────\n";

            MessageBox.Show(message, "Account Created Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ERROR: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Helper method to get user input via popup dialog
    static string PromptUser(string prompt, string defaultValue)
    {
        // Create a simple input form
        Form form = new Form();
        form.Text = "Input";
        form.Width = 300;
        form.Height = 150;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MaximizeBox = false;
        form.MinimizeBox = false;

        Label label = new Label() { Left = 20, Top = 20, Text = prompt, Width = 260 };
        TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 260, Text = defaultValue };
        Button okButton = new Button() { Text = "OK", Left = 120, Width = 80, Top = 80, DialogResult = DialogResult.OK };
        Button cancelButton = new Button() { Text = "Cancel", Left = 200, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

        form.Controls.Add(label);
        form.Controls.Add(textBox);
        form.Controls.Add(okButton);
        form.Controls.Add(cancelButton);
        form.AcceptButton = okButton;
        form.CancelButton = cancelButton;

        return form.ShowDialog() == DialogResult.OK ? textBox.Text : null;
    }


    static void TestAccountRepository()
    {
        try
        {
            string message = "=== TESTING ACCOUNT REPOSITORY ===\n\n";

            // Show database path
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tradingmanager.db");
            message += $"Database: {dbPath}\n\n";

            // Create repository
            AccountRepository repo = new AccountRepository();

            // Read all accounts (don't create new ones!)
            message += "=== RETRIEVING ACCOUNTS FROM DATABASE ===\n\n";
            var allAccounts = repo.GetAllAccounts();
            message += $"Total Accounts Found: {allAccounts.Count}\n\n";

            // Display accounts
            if (allAccounts.Count > 0)
            {
                message += "=== ACCOUNT DETAILS ===\n";
                foreach (var account in allAccounts)
                {
                    message += $"─────────────────────────\n";
                    message += $"ID: {account.AccountId}\n";
                    message += $"Name: {account.AccountName}\n";
                    message += $"Broker: {account.BrokerName}\n";
                    message += $"Balance: ${account.Balance:F2}\n";
                    message += $"Equity: ${account.Equity:F2}\n";
                    message += $"Leverage: {account.Leverage}:1\n";
                    message += $"Created: {account.CreatedDate}\n";
                }
                message += $"─────────────────────────\n\n";
                message += "✓ DATA RETRIEVAL TEST PASSED!\n";
            }
            else
            {
                message += "ℹ️ No accounts in database yet.\n";
                message += "The database is working - tables are created.\n";
                message += "Manager will create accounts via the form.\n";
            }

            MessageBox.Show(message, "Account Repository Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"ERROR: {ex.Message}\n\n{ex.StackTrace}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}