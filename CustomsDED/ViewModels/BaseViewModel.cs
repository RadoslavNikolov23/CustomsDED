namespace CustomsDED.ViewModels
{
    using System.Text;
    using System.Text.RegularExpressions;
    using CommunityToolkit.Mvvm.ComponentModel;

    using CustomsDED.Common.Helpers;
    using CustomsDED.Resources.Localization;

    public abstract class BaseViewModel : ObservableObject
    {
        protected BaseViewModel()
        {

        }

        protected async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }

        protected string? FilterInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            Dictionary<char, char> mapChar = new Dictionary<char, char>
                                                    {
                                                        { 'А', 'A' },
                                                        { 'В', 'B' },
                                                        { 'Е', 'E' },
                                                        { 'К', 'K' },
                                                        { 'М', 'M' },
                                                        { 'Н', 'H' },
                                                        { 'О', 'O' },
                                                        { 'Р', 'P' },
                                                        { 'С', 'C' },
                                                        { 'Т', 'T' },
                                                        { 'У', 'Y' },
                                                        { 'Х', 'X' }
                                                    };

            input = input.ToUpperInvariant();

            StringBuilder resultSB = new StringBuilder(input.Length);

            foreach (char character in input)
            {
                if (mapChar.ContainsKey(character))
                {
                    resultSB.Append(mapChar[character]);
                }
                else
                {
                    resultSB.Append(character);
                }
            }

            string cleaned = Regex.Replace(resultSB.ToString(), @"[^A-Z0-9]", "");

            if (Regex.IsMatch(cleaned, @"^\d+$"))
                return null;


            return string.IsNullOrEmpty(cleaned) ? null : cleaned;
        }

        protected async Task SendEmailWithReport()
        {
            try
            {
                await Logger.SaveLogWithSignatureAsync();
                string logFilePath = Logger.GetLogFilePath();

                EmailMessage? message = null;

                if (File.Exists(logFilePath))
                {
                    EmailAttachment logAttachment = new EmailAttachment(logFilePath, "text/plain");
                    message = new EmailMessage
                    {
                        Subject = "App Customs DED Problem Report",
                        Body = "Please describe the problem you're experiencing:\n\n",
                        To = new List<string> { "custom_rd@abv.bg" },
                        Attachments = new List<EmailAttachment> { logAttachment }
                    };
                }
                else
                {
                    message = new EmailMessage
                    {
                        Subject = "App Customs DED Problem Report",
                        Body = "Please describe the problem you're experiencing:\n\n",
                        To = new List<string> { "custom_rd@abv.bg" }
                    };

                }

                await Email.Default.ComposeAsync(message);

                Logger.ClearLog();
            }
            catch (FeatureNotSupportedException ex)
            {
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.EmailNotSupportedOnDevice);

                await Logger.LogAsync(ex, String.Format(AppResources.AnUnexpectedErrorOccurred, ex));
            }
            catch (Exception ex)
            {
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.SomethingFailedPleaseTryAgainContactDevelepors);

                await Logger.LogAsync(ex, String.Format(AppResources.AnUnexpectedErrorOccurred, ex));
            }

        }
    }
}
