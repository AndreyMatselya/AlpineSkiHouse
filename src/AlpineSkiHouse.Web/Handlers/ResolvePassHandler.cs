using AlpineSkiHouse.Web.Queries;
using AlpineSkiHouse.Web.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Models;

namespace AlpineSkiHouse.Web.Handlers
{
    public class ResolvePassHandler : IRequestHandler<ResolvePass, Pass>
    {
        private readonly PassContext _context;
        private readonly IPassValidityChecker _validator;

        public ResolvePassHandler(PassContext context, IPassValidityChecker validator)
        {
            _context = context;
            _validator = validator;
        }

        public Task<Pass> Handle(ResolvePass message, CancellationToken cancellationToken)
        {
            var passes = _context.Passes.Where(p => p.CardId == message.CardId);

            foreach (var pass in passes)
            {
                if (_validator.IsValid(pass.Id))
                    return Task.FromResult(pass);
            }

            return Task.FromResult<Pass>(null);
        }

    }
}
