using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Handlers;
using AlpineSkiHouse.Web.Models;
using AlpineSkiHouse.Web.Services;
using AlpineSkiHouse.Web.Test.Data;
using Moq;
using Xunit;

namespace AlpineSkiHouse.Web.Test.Services
{
    public class PassResolverTests
    {
        private Mock<IDateService> _mockDateService;
        private PassContext _passContext;
        private PassTypeContext _passTypeContext;

        public PassResolverTests()
        {
            _mockDateService = new Mock<IDateService>();
            _passContext = new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>());
            _passTypeContext = new PassTypeContext(InMemoryDbContextOptionsFactory.Create<PassTypeContext>());
        }

        [Fact]
        public async Task Should_check_context_using_provided_card_id()
        {
            var context = new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>());
            var cardId = 1337;
            var verifyingPassId = 7331;
            context.Passes.Add(new Pass { CardId = cardId, Id = 7330 });
            context.Passes.Add(new Pass { CardId = cardId, Id = verifyingPassId });
            context.Passes.Add(new Pass { CardId = cardId, Id = 7332 });
            context.SaveChanges();

            var validator = new Mock<IPassValidityChecker>();
            validator.Setup(c => c.IsValid(It.IsAny<int>()))
                .Returns<int>(i => context.Passes.Any(p=>p.CardId == i));

            var handler = new ResolvePassHandler(context, validator.Object);

            await handler.Handle(new Queries.ResolvePass { CardId = cardId }, CancellationToken.None);

            validator.Verify(v => v.IsValid(It.Is<int>(i => i == verifyingPassId)), Times.Once);
            validator.Verify(v => v.IsValid(It.IsAny<int>()), Times.Exactly(3));
        }
    }
}
