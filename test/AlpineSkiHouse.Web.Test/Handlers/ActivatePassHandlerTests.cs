using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Command;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Events;
using AlpineSkiHouse.Web.Handlers;
using AlpineSkiHouse.Web.Test.Data;
using MediatR;
using Moq;
using Xunit;

namespace AlpineSkiHouse.Web.Test.Handlers
{
    public class ActivatePassHandlerTests
    {
        public class When_handling_activate_pass_command
        {
            ActivatePass activatePass = new ActivatePass
            {
                PassId = 15,
                ScanId = 45
            };

            private PassContext GetContext()
            {
                return new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>());
            }

            [Fact]
            public void A_new_pass_activation_is_saved_to_the_database()
            {
                using (PassContext context = GetContext())
                {
                    var currentDate = DateTime.UtcNow;
                    Mock<IMediator> mediatorMock = new Mock<IMediator>();

                    var sut = new ActivatePassHandler(context, mediatorMock.Object);
                    sut.Handle(activatePass, CancellationToken.None);

                    Assert.Equal(1, context.PassActivations.Count());
                    var passActivateThatWasAdded = context.PassActivations.Single();
                    Assert.Equal(activatePass.PassId, passActivateThatWasAdded.PassId);
                    Assert.Equal(activatePass.ScanId, passActivateThatWasAdded.ScanId);
                }
            }

            [Fact]
            public async Task The_pass_activated_event_is_raised()
            {
                using (PassContext context = GetContext())
                {
                    Mock<IMediator> mediatorMock = new Mock<IMediator>();

                    var sut = new ActivatePassHandler(context, mediatorMock.Object);
                    var activationId = await sut.Handle(activatePass, CancellationToken.None);

                    mediatorMock.Verify(m => m.Publish(It.Is<PassActivated>(c => c.PassActivationId == activationId), It.IsAny<CancellationToken>()));
                }
            }
        }
    }
}
