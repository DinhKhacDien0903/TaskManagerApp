namespace TaskManagerUI.Features.Pages;

public partial class SignInPage : BasePage
{
	public SignInPage()
	{
		BindingContext = ServiceHelper.GetPageModelObservable<SignInPageModel>();
		InitializeComponent();
	}
}