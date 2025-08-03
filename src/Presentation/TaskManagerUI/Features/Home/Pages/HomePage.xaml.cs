using TaskManagerUI.Services;

namespace TaskManagerUI.Features.Pages;

public partial class HomePage : BasePage
{
    private bool _loaded;
    public HomePage(HomePageModel pm)
    {
        BindingContext = pm;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!_loaded)
        {
            _loaded = true;
            ServiceHelper.GetService<ISystemStyleManager>().SetNavigationBarColor(Constant.AppStyle.PrimaryColor);
        }
    }
}