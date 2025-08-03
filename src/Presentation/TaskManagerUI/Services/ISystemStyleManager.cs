namespace TaskManagerUI.Services
{
    public interface ISystemStyleManager
    {
        void SetStatusBarColor(string hexColor, bool isAnimated = false);

        void SetNavigationBarColor(string hexColor);

        void SetBackGroundDrawable(string? hexColor = "");
    }
}