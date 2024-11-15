using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace FileCopyService
{
    public partial class MyFileCopyService : ServiceBase
    {
        private Timer _timer;
        private readonly int _interval = 300000; // 5 minutes
        // Dedicated base path for logging and log database
        private readonly string _logBasePath = @"C:\Users\Public\FileCopyService";
        private string _logDbPath;

        // Configurable source and destination folders
        private string _sourceFolder = @"C:\Users\Public\FileCopyService\ServerDrive";
        private string _destinationFolder = @"C:\Users\Public\FileCopyService\FileShare";

        public MyFileCopyService()
        {
            // InitializeComponent();
            InitializePaths();
        }

        private void InitializePaths()
        {
            // Initialize paths for logging
            _logDbPath = Path.Combine(_logBasePath, "FileCopyLog.db");

            // Ensure logging directory exists
            Directory.CreateDirectory(_logBasePath);

            // Ensure source and destination directories exist
            Directory.CreateDirectory(_sourceFolder);
            Directory.CreateDirectory(_destinationFolder);
        }

        public void Start() // Custom method for console debugging
        {
            OnStart(null);
        }

        public void Stop() // Custom method for console debugging
        {
            OnStop();
        }


        protected override void OnStart(string[] args)
        {
            InitializeDatabase();
            _timer = new Timer(_interval); // 5 minutes
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
            Log("Service started.");
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
            Log("Service stopped.");
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                CopyFiles();
                Log("Timer elapsed, performing file copy operation...");
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        private void CopyFiles()
        {
            var sourceDirectory = new DirectoryInfo(_sourceFolder);

            // Ensure destination directory exists
            Directory.CreateDirectory(_destinationFolder);

            // Get top 10 files by creation date
            var filesToCopy = sourceDirectory.GetFiles()
                .OrderBy(f => f.CreationTime)
                .Take(10)
                .ToList();

            using (var connection = new SQLiteConnection($"Data Source={_logDbPath};Version=3;"))
            {
                connection.Open();

                foreach (var file in filesToCopy)
                {
                    string fileName = file.Name;

                    // Check if file has already been copied
                    var command = new SQLiteCommand(
                        "SELECT COUNT(*) FROM CopiedFiles WHERE FileName = @FileName",
                        connection
                    );
                    command.Parameters.AddWithValue("@FileName", fileName);

                    if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    {
                        continue; // Skip already copied files
                    }

                    // Copy file
                    var destinationPath = Path.Combine(_destinationFolder, fileName);
                    File.Copy(file.FullName, destinationPath, true);

                    // Log the file as copied
                    command = new SQLiteCommand(
                        "INSERT INTO CopiedFiles (FileName, CopiedOn) VALUES (@FileName, @CopiedOn)",
                        connection
                    );
                    command.Parameters.AddWithValue("@FileName", fileName);
                    command.Parameters.AddWithValue("@CopiedOn", DateTime.UtcNow);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(_logDbPath))
            {
                SQLiteConnection.CreateFile(_logDbPath);
                using (var connection = new SQLiteConnection($"Data Source={_logDbPath};Version=3;"))
                {
                    connection.Open();
                    var command = new SQLiteCommand(
                        "CREATE TABLE CopiedFiles (Id INTEGER PRIMARY KEY AUTOINCREMENT, FileName TEXT, CopiedOn DATETIME)",
                        connection
                    );
                    command.ExecuteNonQuery();
                }
            }
        }


        private void Log(string message)
        {
            string messageLogsPath = Path.Combine(_logBasePath, "FileCopyServiceDebug.log");
            File.AppendAllText(messageLogsPath, $"{DateTime.Now}: {message}\n");
        }

        private void LogError(string message)
        {
            string errorLogPath = Path.Combine(_logBasePath, "FileCopyServiceError.log");

            File.AppendAllText(errorLogPath, $"{DateTime.Now}: {message}\n");
        }
    }
}
