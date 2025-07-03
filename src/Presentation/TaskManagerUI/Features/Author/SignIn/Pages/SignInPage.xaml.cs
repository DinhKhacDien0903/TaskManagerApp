using TaskManagerUI.Features.PageModels;
using TaskManagerUI.Helpers;
using TaskManagerUI.Utilities.MVVM;

namespace TaskManagerUI.Features.Pages;

public partial class SignInPage : BasePage
{
	public SignInPage()
	{
		BindingContext = ServiceHelper.GetPageModelObservable<SignInPageModel>();
		InitializeComponent();
	}
}