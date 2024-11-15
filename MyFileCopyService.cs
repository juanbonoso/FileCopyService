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
        private string _sourceFolder = @"C:\ServerDrive";
        private string _destinationFolder = @"C:\FileShare";
        private string _logDbPath = @"C:\FileCopyLog.db";

        public MyFileCopyService()
        {
           // InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            InitializeDatabase();
            _timer = new Timer(300000); // 5 minutes
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                CopyFiles();
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

        private void LogError(string message)
        {
            File.AppendAllText(@"C:\FileCopyError.log", $"{DateTime.Now}: {message}\n");
        }
    }
}
