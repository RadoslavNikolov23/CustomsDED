namespace CustomsDED.Views;

using CommunityToolkit.Maui.Extensions;
using CustomsDED.Common.Helpers;
using CustomsDED.Resources.Localization;
using CustomsDED.ViewModels;

public partial class MrzPersonPage : ContentPage
{
    public MrzPersonPage(MrzPersonViewModel mrzPersonViewModel)
    {
        InitializeComponent();
        this.BindingContext = mrzPersonViewModel;
    }

    private async void OpenCameraViewClicked(object sender, EventArgs e)
    {
        this.CameraViewGrid.IsVisible = true;
        this.CameraViewButtos.IsVisible = true;

        PermissionStatus status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert(AppResources.Error,
                               AppResources.CameraPermissionNotGranted,
                               "OK");
            await CloseCameraCommon();
            return;
        }

        try
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            await this.CameraView.StartCameraPreview(cts.Token);

        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.Error,
                              "Your device camera could not start. Please close and try again.",
                              "OK");
            await Logger.LogAsync(ex, "Error in OnAppearing, in the MrzPersonPage class.");
            await CloseCameraCommon();
        }
    }

    private async void CloseCameraViewClicked(object sender, EventArgs e)
    {
        await CloseCameraCommon();
    }

    private async void TakeMRZPictureClicked(object sender, EventArgs e)
    {
        CreatingPopup creatingPopup = new CreatingPopup();
        Shell.Current.ShowPopup(creatingPopup);

        try
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            Stream stream = await this.CameraView.CaptureImage(cts.Token);

            if (stream == null)
            {
                await creatingPopup.CloseAsync();
                await DisplayAlert(AppResources.Error,
                                   AppResources.CameraCaptureCanceled,
                                   "OK");
                return;
            }

            byte[] photoBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                photoBytes = ms.ToArray();
            }

            string[] textMessage = new string[2];
            if (BindingContext is MrzPersonViewModel vm)
                textMessage = await vm.ProcessCapturedImageAsync(photoBytes);


            await creatingPopup.CloseAsync();
            await DisplayAlert(textMessage[0],
                               textMessage[1],
                               "OK");

        }
        catch (Exception ex)
        {
            await creatingPopup.CloseAsync();

            await Logger.LogAsync(ex, "Error in OnTakePlatePictureClicked, in the MrzPersonPage class.");
            await DisplayAlert(AppResources.Error,
                                   AppResources.AnErrorOccurredWhileSavingPerson,
                                   "OK");

        }
        finally
        {
            //Do i need a creatingPopup.Close(); here? See other options for a if condition
            await CloseCameraCommon();
        }
    }

    private async Task CloseCameraCommon()
    {
        await Task.Yield();
        this.CameraView.StopCameraPreview();

        this.CameraViewGrid.IsVisible = false;
        this.CameraViewButtos.IsVisible = false;
    }
}
