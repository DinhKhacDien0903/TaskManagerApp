namespace TaskManagerUI.Features.Pages;

public partial class CalenderPage : BasePage
{
    public CalenderPage(CalenderPageModel pm)
    {
        BindingContext = pm;
        InitializeComponent();
    }
}