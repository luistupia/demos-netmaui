
using Microsoft.Extensions.Logging;
#if ANDROID
using Android.Graphics.Drawables;
using Microsoft.Maui.Handlers;
#endif

namespace CustomControls;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-regular-400.ttf", "FaRegular400");
                fonts.AddFont("fa-solid-900.ttf", "FaSolid900");
            });
        
#if ANDROID
        // 🔧 Inyectamos el handler personalizado para Entry
        EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        });

        PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}