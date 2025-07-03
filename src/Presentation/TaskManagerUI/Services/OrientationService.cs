using Android.Content.PM;
using TaskManagerUI.IServices;

namespace TaskManagerUI.Services
{
    public class OrientationService : IOrientationService
    {
        public void Portrait()
        {
            if (Platform.CurrentActivity == null)
                return;

            Platform.CurrentActivity.RequestedOrientation = ScreenOrientation.Portrait;
        }
    }
}