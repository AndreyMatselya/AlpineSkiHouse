using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Events;
using AlpineSkiHouse.Web.Handlers;
using AlpineSkiHouse.Web.Services;
using AlpineSkiHouse.Web.Test.Data;
using MediatR;
using Moq;
using Xunit;

namespace AlpineSkiHouse.Web.Test.Handlers
{
    public class AddSkiPassOnPurchaseCompletedTests
    {
        public class When_handling_purchase_completed
        {
            PassPurchased passPurchased = new PassPurchased
            {
                CardId = 1,
                PassTypeId = 2,
                DiscountCode = "2016springpromotion",
                PricePaid = 200m
            };

            private static PassContext GetContext()
            {
                return new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>());
            }
            [Fact]
            public async Task Pass_is_saved_to_the_database_for_each_pass()
            {
                using (PassContext context = GetContext())
                {
                    var mediator = new Mock<IMediator>();
                    var dateService = new Mock<IDateService>();
                    var currentDate = DateTime.UtcNow;
                    dateService.Setup(x => x.Now()).Returns(currentDate);
                    var sut = new AddSkiPassOnPurchaseCompleted(context, mediator.Object, dateService.Object);
                    await sut.Handle(new Events.PurchaseCompleted
                    {
                        Passes = new List<PassPurchased>
                        {
                            passPurchased
                        }
                    },CancellationToken.None);

                    Assert.Equal(1, context.Passes.Count());
                    Assert.Equal(passPurchased.CardId, context.Passes.Single().CardId);
                    Assert.Equal(passPurchased.PassTypeId, context.Passes.Single().PassTypeId);
                    Assert.Equal(currentDate, context.Passes.Single().CreatedOn);
                }
            }

            [Fact]
            public async Task PassesAddedEvents_is_published_for_each_pass()
            {
                using (PassContext context =
                        GetContext())
                {
                    var mediator = new Mock<IMediator>();
                    var dateService = new Mock<IDateService>();
                    var currentDate = DateTime.UtcNow;
                    dateService.Setup(x => x.Now()).Returns(currentDate);
                    var sut = new AddSkiPassOnPurchaseCompleted(context, mediator.Object, dateService.Object);

                    await sut.Handle(new Events.PurchaseCompleted
                    {
                        Passes = new List<PassPurchased>
                        {
                            passPurchased
                        }
                    }, CancellationToken.None);
                    var dbPass = context.Passes.Single();
                    mediator.Verify(x => x.Publish(It.Is<PassAdded>(y => y.CardId == passPurchased.CardId &&
                                                                            y.CreatedOn == currentDate &&
                                                                            y.PassId == dbPass.Id &&
                                                                            y.PassTypeId == passPurchased.PassTypeId), It.IsAny<CancellationToken>()));
                }
            }

            [Fact]
            public async Task Empty_passes_collection_saves_nothing_to_the_database()
            {
                using (PassContext context = GetContext())
                {
                    var mediator = new Mock<IMediator>();
                    var dateService = new Mock<IDateService>();
                    var currentDate = DateTime.UtcNow;
                    dateService.Setup(x => x.Now()).Returns(currentDate);
                    var sut = new AddSkiPassOnPurchaseCompleted(context, mediator.Object, dateService.Object);
                    await sut.Handle(new Events.PurchaseCompleted { Passes = new List<PassPurchased>() }, CancellationToken.None);

                    Assert.Equal(0, context.Passes.Count());
                }
            }

            [Fact]
            public async Task Empty_passes_collection_publishes_no_messages()
            {
                using (PassContext context = GetContext())
                {
                    var mediator = new Mock<IMediator>();
                    var dateService = new Mock<IDateService>();
                    var currentDate = DateTime.UtcNow;
                    dateService.Setup(x => x.Now()).Returns(currentDate);
                    var sut = new AddSkiPassOnPurchaseCompleted(context, mediator.Object, dateService.Object);

                    await sut.Handle(new Events.PurchaseCompleted { Passes = new List<PassPurchased>() }, CancellationToken.None);

                    mediator.Verify(x => x.Publish(It.IsAny<PassAdded>(), It.IsAny<CancellationToken>()), Times.Never);
                }
            }
        }
    }
}
