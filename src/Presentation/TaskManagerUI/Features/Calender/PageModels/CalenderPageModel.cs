using MediatR;

namespace TaskManagerUI.Features.PageModels;

public partial class CalenderPageModel(IMediator mediator) : BasePageModel()
{
    private readonly IMediator _mediator = mediator;
}