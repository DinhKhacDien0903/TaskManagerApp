namespace TaskManagerUI.Features.Pages;

public partial class WellcomePage : BasePage
{
	public WellcomePage()
	{
		BindingContext = ServiceHelper.GetPageModelObservable<WellcomePageModel>();
		InitializeComponent();
	}
}