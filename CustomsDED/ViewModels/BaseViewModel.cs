namespace CustomsDED.ViewModels
{
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
                         String.Format(AppResources.AnUnexpectedErrorOccurred,ex));

            }

        }
    }
}
