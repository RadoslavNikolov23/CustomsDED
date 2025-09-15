namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;

    using CustomsDED.Common.Helpers;
    using CustomsDED.Resources.Localization;
    using System.Text;
    using System.Text.RegularExpressions;

    public abstract class BaseViewModel : ObservableObject
    {
        protected BaseViewModel()
        {

        }

        protected async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }

        protected string FilterInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Map Cyrillic lookalikes to Latin
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

            // Convert to uppercase
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

            // Finally, keep only A–Z and 0–9
            return Regex.Replace(resultSB.ToString(), @"[^A-Z0-9]", "");
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
            catch (FeatureNotSupportedException)
            {
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.EmailNotSupportedOnDevice);

            }
            catch (Exception ex)
            {
                await ShowPopupMessage(AppResources.Error,
                         String.Format(AppResources.AnUnexpectedErrorOccurred, ex));

            }

        }
    }
}
