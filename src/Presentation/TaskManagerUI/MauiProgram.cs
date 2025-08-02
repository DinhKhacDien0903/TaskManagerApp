using Application;
using AuthorApp;
using CommunityToolkit.Maui;
using DotNet.Meteor.HotReload.Plugin;
using FFImageLoading.Maui;
using Fonts;
using Infrastructure;
using Microsoft.Extensions.Logging;
using PanCardView;
using Plugin.LocalNotification;
using Plugin.Maui.SwipeCardView;
using Sharpnado.CollectionView;
using SkiaSharp.Views.Maui.Controls.Hosting;
using TaskManagerUI.Navigation;

namespace TaskManagerUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.RegisterHandlers()
			.UseSwipeCardView()
			.UseSkiaSharp()
			.UseCardsView()
			.UseFFImageLoading()
			.UseSharpnadoCollectionView(loggerEnable: false, debugLogEnable: false)
			.UseLocalNotification()
			.RegisterCompatibilityRenderer()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Quicksand-Bold.ttf", "QuicksandBold");
				fonts.AddFont("Quicksand-Regular.ttf", "QuicksandRegular");
				fonts.AddFont("Quicksand-SemiBold.ttf", "QuicksandSemiBold");
				fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
				fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
				fonts.AddFont("MaterialIconsRound-Regular.otf", "MaterialIconsRound");
            });
		builder.Services.AddSingleton<INavigationOtherShellService>(sp =>
						new NavigationOtherShellService(type => (sp.GetService(type) as ContentPage)!));
		builder.Services.AddApplicationServices();
		builder.Services.AddInfrastructureServices();
		builder.Services.AddApplications();
		builder.Services.RegisterPageModels();
		builder.Services.RegisterPages();

#if DEBUG
		builder.Logging.AddDebug();
		builder.EnableHotReload();
#endif

		return builder.Build();
	}
}