using TradingManager.Database;
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


        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }    
}