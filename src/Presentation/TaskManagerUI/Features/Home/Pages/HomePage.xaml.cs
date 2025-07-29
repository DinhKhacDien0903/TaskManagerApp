namespace TaskManagerUI.Features.Pages;

public partial class HomePage : BasePage
{
    public HomePage(HomePageModel pm)
    {
        BindingContext = pm;
        InitializeComponent();
    }
}