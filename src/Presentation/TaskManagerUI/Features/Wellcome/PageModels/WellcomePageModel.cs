using MediatR;
using TaskManagerUI.Navigation;

namespace TaskManagerUI.Features.PageModels
{
    public partial class WellcomePageModel
    (IMediator mediator,
    INavigationService navigationService,
    INavigationOtherShellService navigationOtherShellService) : BasePageModel(navigationService)
    {
        private readonly IMediator _mediator = mediator;

        private readonly INavigationOtherShellService _navigationOtherShellService = navigationOtherShellService;


    }
}