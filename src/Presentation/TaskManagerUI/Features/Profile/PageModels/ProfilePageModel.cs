using MediatR;

namespace TaskManagerUI.Features.PageModels;

public partial class ProfilePageModel(IMediator mediator) : BasePageModel()
{
    private readonly IMediator _mediator = mediator;

}