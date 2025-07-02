using Application;
using AuthorApp;
using CommunityToolkit.Maui;
using DotNet.Meteor.HotReload.Plugin;
using FFImageLoading.Maui;
using Infrastructure;
using Microsoft.Extensions.Logging;
using PanCardView;
using Plugin.LocalNotification;
using Plugin.Maui.SwipeCardView;
using Sharpnado.CollectionView;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace TaskManagerUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseSwipeCardView()
			.UseSkiaSharp()
			.UseCardsView()
			.UseFFImageLoading()
			.UseSharpnadoCollectionView(loggerEnable: false, debugLogEnable: false)
			.UseLocalNotification()
			.ConfigureFonts(fonts =>
			{
				// fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				// fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Quicksand-Bold.ttf", "QuicksandBold");
				fonts.AddFont("Quicksand-Regular.ttf", "QuicksandRegular");
				fonts.AddFont("Quicksand-SemiBold.ttf", "QuicksandSemiBold");
			});

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