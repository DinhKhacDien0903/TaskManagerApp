using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace TaskManagerUI.Features.PageModels
{
    public partial class WellcomePageModel(IMediator mediator) : BasePageModel()
    {
        private readonly IMediator _mediator = mediator;

        [RelayCommand]
        async Task OpenHomePage()
        {
            await AppHelper.RefreshAppAsync();
            await Task.Delay(100);
        }
    }
}