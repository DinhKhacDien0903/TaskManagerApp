namespace TaskManagerUI.Features.Pages;

public partial class TaskPage : BasePage
{
    public TaskPage(TaskPageModel pm)
    {
        BindingContext = pm;
        InitializeComponent();
    }
}