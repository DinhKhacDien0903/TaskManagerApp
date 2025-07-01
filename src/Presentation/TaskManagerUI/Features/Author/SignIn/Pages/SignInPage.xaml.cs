using TaskManagerUI.Features.PageModels;
using TaskManagerUI.Utilities.MVVM;

namespace TaskManagerUI.Features.Pages;

public partial class SignInPage : BasePage
{
	public SignInPage(SignInPageModel pm)
	{
		BindingContext = pm;
		InitializeComponent();
	}
}