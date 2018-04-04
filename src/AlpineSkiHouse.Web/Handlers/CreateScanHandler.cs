using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Command;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Events;
using AlpineSkiHouse.Web.Models;
using AlpineSkiHouse.Web.Services;
using MediatR;

namespace AlpineSkiHouse.Web.Handlers
{
    public class CreateScanHandler : IRequestHandler<CreateScan, int>
    {
        private readonly PassContext _passContext;
        private readonly IDateService _dateService;
        private readonly IMediator _mediator;

        public CreateScanHandler(PassContext passContext, IDateService dateService, IMediator mediator)
        {
            _dateService = dateService;
            _passContext = passContext;
            _mediator = mediator;
        }

        public Task<int> Handle(CreateScan message, CancellationToken cancellationToken)
        {
            var scan = new Scan
            {
                CardId = message.CardId,
                LocationId = message.LocationId,
                DateTime = _dateService.Now()
            };
            _passContext.Scans.Add(scan);
            _passContext.SaveChanges();

            _mediator.Publish(new CardScanned { ScanId = scan.Id });            
            return Task.FromResult(scan.Id);
        }
    }
}
