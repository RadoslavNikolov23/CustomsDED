namespace CustomsDED.Common.Helpers
{
    using System.Security.Cryptography;
    using System.Text;

    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine
                                                        (FileSystem.AppDataDirectory, "app_log.txt");

        public static async Task LogAsync(Exception ex, string context = "")
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {context}\n{ex}\n\n";
                await File.AppendAllTextAsync(LogFilePath, logEntry);
            }
            catch
            {
                // For avoid recursive logging problems!
            }
        }

        public static async Task SaveLogWithSignatureAsync()
        {
            try
            {
                if (!File.Exists(LogFilePath))
                    return;

                string logContent = await File.ReadAllTextAsync(LogFilePath);

                string secretKey = "**RadoOneCust61#$";
                string hash = ComputeHMACSHA(logContent, secretKey);

                string signedContent = $"--HMAC:{hash}\n{logContent}";
                await File.WriteAllTextAsync(LogFilePath, signedContent);
            }
            catch
            {
                // For avoid recursive logging problems!
            }
        }

        public static string GetLogFilePath() => LogFilePath;

        public static async Task<string> ReadLogAsync()
        {
            try
            {
                if (File.Exists(LogFilePath))
                    return await File.ReadAllTextAsync(LogFilePath);

                return "No logs found.";
            }
            catch
            {
                return "Failed to read logs.";
            }
        }

        public static void ClearLog()
        {
            try
            {
                if (File.Exists(LogFilePath))
                    File.Delete(LogFilePath);
            }
            catch { }
        }

        private static string ComputeHMACSHA(string content, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            using HMACSHA256 hmac = new HMACSHA256(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(contentBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
