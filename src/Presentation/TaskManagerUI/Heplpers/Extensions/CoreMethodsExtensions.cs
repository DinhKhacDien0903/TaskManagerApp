using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerUI.Heplpers.Extensions
{
    public static class CoreMethodsExtensions
    {
        //public static async Task<Page> SwitchTabAsync<T>()
        //    where T : Page
        //{
        //    var tabbar = Shell.Current?.CurrentItem;
        //    if (tabbar == null)
        //        return null;

        //    if (typeof(T) == typeof(HomePage))
        //    {
        //        tabbar.CurrentItem = tabbar.Items[0];
        //    }
        //    else if (typeof(T) == typeof(MessagePage))
        //    {
        //        tabbar.CurrentItem = tabbar.Items[1];
        //    }
        //    else if (typeof(T) == typeof(MembershipCardPage))
        //    {
        //        tabbar.CurrentItem = tabbar.Items[2];
        //    }
        //    else if (typeof(T) == typeof(DealerPage))
        //    {
        //        tabbar.CurrentItem = tabbar.Items[3];
        //    }
        //    else if (typeof(T) == typeof(SettingsPage))
        //    {
        //        tabbar.CurrentItem = tabbar.Items[4];
        //    }

        //    var shellContent = (IShellContentController)tabbar.CurrentItem.CurrentItem;
        //    if (shellContent.Page == null)
        //    {
        //        var awaiter = new TaskCompletionSource<bool>();

        //        void ShellContent_IsPageVisibleChanged(object sender, EventArgs e)
        //        {
        //            awaiter.TrySetResult(true);
        //        }

        //        shellContent.IsPageVisibleChanged += ShellContent_IsPageVisibleChanged;
        //        await awaiter.Task;
        //        shellContent.IsPageVisibleChanged -= ShellContent_IsPageVisibleChanged;
        //    }

        //    return shellContent.Page.GetPage();
        //}

#if ANDROID
        // TODO: Fix, android will crashed if shell SwitchTabAsync before (by re-setting it).
        public static void RefreshCurrentTab(this Shell shell)
        {
            int idx = GetCurrentTabIndex();
            if (idx >= 0)
            {
                shell.CurrentItem.CurrentItem = shell.CurrentItem.Items[idx];
            }

            int GetCurrentTabIndex()
            {
                return shell?.CurrentItem?.CurrentItem == null
                    ? -1
                    : shell.CurrentItem.Items.IndexOf(shell.CurrentItem.CurrentItem);
            }
        }
#endif

        public static void TearDown(this IVisualTreeElement vte)
        {
            tearDownImpl(vte, true);

            return;

            void tearDownImpl(IVisualTreeElement vte, bool isRoot)
            {
                if (vte is not BindableObject bindableObject)
                    return;

                foreach (IVisualTreeElement childElement in vte.GetVisualChildren().ToList())
                {
                    tearDownImpl(childElement, false);
                }

                if (vte is VisualElement visualElement)
                {
                    // clear the BindingContext
                    visualElement.BindingContext = null;

                    // isolate the element.
                    visualElement.Parent = null;
                    switch (vte)
                    {
                        case ListView listView:
                            listView.ItemsSource = null;
                            break;
                        case ContentView contentView:
                            contentView.Content = null;
                            break;
                        case Border border:
                            border.Content = null;
                            break;
                        case ContentPage contentPage:
                            contentPage.Content = null;
                            break;
                        case ScrollView scrollView:
                            scrollView.Content = null;
                            break;
                        //case NoAnimationCoverFlowView cards:
                        //    cards.ItemsSource = null;
                        //    cards.CleanUnusedViews();
                        //    cards.Clear();
                        //    break;
                        case CollectionView collectionView:
                            collectionView.ItemsSource = null;
                            break;
                    }

                    visualElement.ClearLogicalChildren();

                    // disconnect the handler.
                    if (visualElement.Handler != null)
                    {
                        visualElement.Handler?.DisconnectHandler();
                        processDisposeView(visualElement.Handler);
                    }

                    visualElement.Behaviors?.Clear();
                    visualElement.Triggers?.Clear();
                    visualElement.Resources?.Clear();
                    processDisposeView(visualElement);
                }
                else if (vte is Element element)
                {
                    element.BindingContext = null;
                    element.Parent = null;

                    element.ClearLogicalChildren();
                    if (element.Handler != null)
                    {
#if IOS
                        // Fixes issue specific to ListView on iOS, where RealCell is not nulled out.
                        if (element is ViewCell && element.Handler.PlatformView is IDisposable disposablePlatformView)
                            disposablePlatformView.Dispose();
#endif
                        processDisposeView(element.Handler);
                        element.Handler?.DisconnectHandler();
                    }
                }
            }

            void processDisposeView(object obj)
            {
                if (obj is IDisposable disposable)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
            }
        }

        public static async Task ForceGarbageCollectorAsync()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            await Task.Delay(500);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}