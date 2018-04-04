using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Command;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Events;
using AlpineSkiHouse.Web.Models;
using MediatR;

namespace AlpineSkiHouse.Web.Handlers
{
    public class ActivatePassHandler : IRequestHandler<ActivatePass, int>
    {
        private readonly PassContext _passContext;
        private readonly IMediator _mediator;

        public ActivatePassHandler(PassContext passContext, IMediator mediator)
        {
            _passContext = passContext;
            _mediator = mediator;
        }

        public Task<int> Handle(ActivatePass message, CancellationToken cancellationToken)
        {
            PassActivation activation = new PassActivation
            {
                PassId = message.PassId,
                ScanId = message.ScanId
            };
            _passContext.PassActivations.Add(activation);
            _passContext.SaveChanges();

            _mediator.Publish(new PassActivated { PassActivationId = activation.Id });

            return Task.FromResult(activation.Id);
        }
    }
}
