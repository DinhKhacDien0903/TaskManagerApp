namespace TaskManagerUI.Features.Pages;

public partial class NotePage : BasePage
{
    public NotePage(NotePageModel pm)
    {
        BindingContext = pm;
        InitializeComponent();
    }
}