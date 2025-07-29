using MediatR;

namespace TaskManagerUI.Features.PageModels;

public partial class NotePageModel(IMediator mediator) : BasePageModel()
{
    private readonly IMediator _mediator = mediator;

}