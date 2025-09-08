namespace CustomsDED.Views;

using CustomsDED.Common.Helpers;
using CustomsDED.ViewModels;

public partial class LicensePlatePage : ContentPage
{
    private bool isCameraPreviewing = false;

    public LicensePlatePage(LicensePlateViewModel licensePlateViewModel)
	{
		InitializeComponent();
        this.BindingContext = licensePlateViewModel;
    }

    private async void OnAppearing(object sender, EventArgs e)
    {
        LicensePlateViewModel? vm = BindingContext as LicensePlateViewModel;

        if (vm != null)
        {

            if (vm.IsCameraViewVisible && !this.isCameraPreviewing)
            {
                PermissionStatus status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Error", "Camera permission not granted", "OK");
                    vm.CloseCameraViewCommand.Execute(null);
                    return;
                }

                try
                {
                    // Start camera preview
                    using CancellationTokenSource cts = new CancellationTokenSource();
                    await this.CameraView.StartCameraPreview(cts.Token);
                    this.isCameraPreviewing = true;


                }
                catch (Exception ex)
                {
                    await Logger.LogAsync(ex, "Error in OnAppearing, in the LicensePlatePage class.");
                }
            }
        }
    }

    private async void TakePlatePictureClicked(object sender, EventArgs e)
    {
        try
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            Stream stream = await this.CameraView.CaptureImage(cts.Token);

            if (stream == null)
            {
                //TODO : User canceled or error, see what is best to do
                await DisplayAlert("Error", "Camera Capture canceled.", "OK");
                return;
            }

            // Convert to byte[]
            byte[] photoBytes;
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                photoBytes = ms.ToArray();
            }


            if (BindingContext is LicensePlateViewModel vm)
                await vm.ProcessCapturedImageAsync(photoBytes);

            
        }
        catch (Exception ex)
        {
            await Logger.LogAsync(ex, "Error in OnTakePlatePictureClicked, in the LicensePlatePage class.");

        }
        finally
        {
            // Stop preview so we can reopen later
            if (this.isCameraPreviewing)
            {
                this.CameraView.StopCameraPreview();
                this.isCameraPreviewing = false;
            }

            if (BindingContext is LicensePlateViewModel vm)
                vm.CloseCameraViewCommand.Execute(null);
        }
    }
}