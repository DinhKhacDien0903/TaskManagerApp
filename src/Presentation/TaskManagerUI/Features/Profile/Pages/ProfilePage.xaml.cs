namespace TaskManagerUI.Features.Pages;

public partial class ProfilePage : BasePage
{
    public ProfilePage(ProfilePageModel pm)
    {
        BindingContext = pm;
        InitializeComponent();
    }
}