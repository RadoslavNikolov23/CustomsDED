namespace CustomsDED.Views;

using CustomsDED.Common.Helpers;
using CustomsDED.Resources.Localization;
using CustomsDED.ViewModels;

public partial class MrzPersonPage : ContentPage
{
    private bool isCameraPreviewing = false;

    public MrzPersonPage(MrzPersonViewModel mrzPersonViewModel)
    {
        InitializeComponent();
        this.BindingContext = mrzPersonViewModel;
    }

    private async void OpenCameraViewClicked(object sender, EventArgs e)
    {
        this.CameraViewGrid.IsVisible = true;
        this.CameraViewButtos.IsVisible = true;

        if (!this.isCameraPreviewing)
        {
            PermissionStatus status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert(AppResources.Error,
                                   AppResources.CameraPermissionNotGranted,
                                   "OK");
                CloseCameraCommon();
                return;
            }

            try
            {
                using CancellationTokenSource cts = new CancellationTokenSource();
                await this.CameraView.StartCameraPreview(cts.Token);
                this.isCameraPreviewing = true;


            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in OnAppearing, in the MrzPersonPage class.");
                CloseCameraCommon();
            }
        }
    }

    private void CloseCameraViewClicked(object sender, EventArgs e)
    {
        CloseCameraCommon();
    }

    private async void TakeMRZPictureClicked(object sender, EventArgs e)
    {
        try
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            Stream stream = await this.CameraView.CaptureImage(cts.Token);

            if (stream == null)
            {
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


            if (BindingContext is MrzPersonViewModel vm)
                await vm.ProcessCapturedImageAsync(photoBytes);
        }
        catch (Exception ex)
        {
            await Logger.LogAsync(ex, "Error in OnTakePlatePictureClicked, in the MrzPersonPage class.");

        }
        finally
        {
            CloseCameraCommon();
        }
    }

    private void CloseCameraCommon()
    {

        this.CameraView.StopCameraPreview();
        this.isCameraPreviewing = false;

        this.CameraViewGrid.IsVisible = false;
        this.CameraViewButtos.IsVisible = false;
    }
}