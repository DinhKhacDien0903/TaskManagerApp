namespace TaskManagerUI.Features.Pages;

public partial class SignUpPage : BasePage
{
	public SignUpPage()
	{
		BindingContext = ServiceHelper.GetPageModelObservable<SignUpPageModel>();
		InitializeComponent();
	}
}