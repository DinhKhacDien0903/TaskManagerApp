using MediatR;

namespace TaskManagerUI.Features.PageModels;

public partial class TaskPageModel(IMediator mediator) : BasePageModel()
{
    private readonly IMediator _mediator = mediator;

}