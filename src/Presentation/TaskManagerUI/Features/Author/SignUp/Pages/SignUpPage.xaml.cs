using TaskManagerUI.Features.PageModels;
using TaskManagerUI.Helpers;
using TaskManagerUI.Utilities.MVVM;

namespace TaskManagerUI.Features.Pages;

public partial class SignUpPage : BasePage
{
	public SignUpPage()
	{
		BindingContext = ServiceHelper.GetPageModelObservable<SignUpPageModel>();
		InitializeComponent();
	}
}