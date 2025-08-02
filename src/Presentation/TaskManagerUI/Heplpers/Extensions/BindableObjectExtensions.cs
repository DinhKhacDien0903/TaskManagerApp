namespace TaskManagerUI.Heplpers.Extensions
{
    public static class BindableObjectExtensions
    {
        public static Page GetCurrentPage(this Page mainPage) =>
           _getCurrentPage(mainPage);

        private static Func<Page, Page> _getCurrentPage = mainPage =>
        {
            var page = mainPage;
            Page child;

            var lastModal = page.Navigation.ModalStack.LastOrDefault();
            if (lastModal != null)
                page = lastModal;

            if (page is AppShell appShell)
                child = appShell.CurrentPage;
            else
                child = page;
            return child;
        };
    }
}