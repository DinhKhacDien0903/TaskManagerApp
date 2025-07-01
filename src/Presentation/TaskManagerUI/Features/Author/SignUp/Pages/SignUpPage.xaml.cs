using TaskManagerUI.Features.PageModels;
using TaskManagerUI.Utilities.MVVM;

namespace TaskManagerUI.Features.Pages;

public partial class SignUpPage : BasePage
{
	public SignUpPage(SignUpPageModel pm)
	{
		BindingContext = pm;
		InitializeComponent();
	}

	private async void OnSignInTapped(object sender, TappedEventArgs e)
	{
		await DisplayAlert("Navigation", "Navigate to Sign In page.", "OK");
	}
}