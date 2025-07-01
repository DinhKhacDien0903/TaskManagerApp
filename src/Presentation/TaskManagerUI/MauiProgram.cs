using Application;
using AuthorApp;
using CommunityToolkit.Maui;
using DotNet.Meteor.HotReload.Plugin;
using Infrastructure;
using Microsoft.Extensions.Logging;

namespace TaskManagerUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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