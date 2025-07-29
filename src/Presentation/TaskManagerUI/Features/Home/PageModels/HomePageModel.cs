using MediatR;

namespace TaskManagerUI.Features.PageModels;

public partial class HomePageModel(IMediator mediator) : BasePageModel()
{
    private readonly IMediator _mediator = mediator;

}