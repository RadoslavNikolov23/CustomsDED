
namespace CustomsDED
{
    using CommunityToolkit.Maui;
    using CustomsDED.Data.Connection;
    using CustomsDED.Data.Repository;
    using CustomsDED.Data.Repository.Contracts;
    using CustomsDED.Services.PersonServices;
    using CustomsDED.Services.PersonServices.Contract;
    using CustomsDED.Services.VehicleServices;
    using CustomsDED.Services.VehicleServices.Contract;
    using CustomsDED.ViewModels;
    using CustomsDED.Views;
    using Microsoft.Extensions.Logging;
    using Plugin.Maui.OCR;
    using SkiaSharp.Views.Maui.Controls.Hosting;

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseOcr()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            RegisterDateBaseComponents(builder.Services);
            RegisterServices(builder.Services);
            RegisterPages(builder.Services);
            RegisterVM(builder.Services);


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static void RegisterDateBaseComponents(IServiceCollection services)
        {
            services.AddScoped<CustomsDB>();
            services.AddScoped(typeof(IBaseAsyncRepository<>), typeof(BaseAsyncRepository<>));
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IVehicleService, VehicleService>();
        }
        public static void RegisterPages(IServiceCollection services)
        {
            services.AddTransient<MainPage>();
            services.AddTransient<LicensePlatePage>();
            services.AddTransient<MrzPersonPage>();
            services.AddTransient<VehicleSearchPage>();
            services.AddTransient<PersonSearchPage>();
            services.AddTransient<DateSearchPage>();
        }

        public static void RegisterVM(IServiceCollection services)
        {
            services.AddTransient<DateSearchViewModel>();
            services.AddTransient<LicensePlateViewModel>();
            services.AddTransient<MrzPersonViewModel>();
            services.AddTransient<PersonSearchViewModel>();
            services.AddTransient<VehicleSearchViewModel>();
        }
    }
}
