using System;
using System.Threading;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Command;
using AlpineSkiHouse.Web.Controllers;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Models;
using AlpineSkiHouse.Web.Queries;
using AlpineSkiHouse.Web.Services;
using AlpineSkiHouse.Web.Test.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AlpineSkiHouse.Web.Test.Controllers
{
    public class ScanCardControllerTests
    {

        public class WhenCardIsScanned
        {
            [Fact]
            public void CreateScanCommandShouldBeInvoked()
            {
                using (PassContext context =
                        new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>()))
                {
                    var dateServiceMock = new Mock<IDateService>();
                    dateServiceMock.Setup(d => d.Now()).Returns(DateTime.Now);

                    var mediatorMock = new Mock<IMediator>();
                    var controller = new ScanCardController(context, mediatorMock.Object, dateServiceMock.Object);

                    var result = controller.Get(124, 432);

                    mediatorMock.Verify(m => m.Send(It.Is<CreateScan>(c => c.CardId == 124 && c.LocationId == 432), It.IsAny<CancellationToken>()));
                }
            }
        }

        public class WhenCardIsScannedWithNoValidPass
        {
            [Fact]
            public async Task ResultShouldBeFalse()
            {
                using (PassContext context =
                        new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>()))
                {

                    var mediatorMock = new Mock<IMediator>();
                    mediatorMock.Setup(m => m.Send(It.Is<ResolvePass>(r => r.CardId == 124 && r.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(default(Pass));

                    var dateServiceMock = new Mock<IDateService>();
                    dateServiceMock.Setup(d => d.Now()).Returns(DateTime.Now);

                    var controller = new ScanCardController(context, mediatorMock.Object, dateServiceMock.Object);

                    var result = await controller.Get(124, 432);

                    Assert.IsType<OkObjectResult>(result);
                    OkObjectResult okObjectResult = (OkObjectResult)result;
                    Assert.Equal(false, okObjectResult.Value);
                }
            }
        }


        public class WhenCardIsScannedWithValidPassWithNoPreviousActivation
        {
            [Fact]
            public async Task ResultShouldBeTrue()
            {
                using (PassContext context =
                        new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>()))
                {
                    var pass = new Pass { CardId = 124, CreatedOn = DateTime.Today.AddDays(-4)};
                    context.Passes.Add(pass);
                    context.SaveChanges();

                    var mediatorMock = new Mock<IMediator>();
                    mediatorMock.Setup(m => m.Send(It.Is<ResolvePass>(r => r.CardId == 124 && r.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(pass);

                    var dateServiceMock = new Mock<IDateService>();
                    dateServiceMock.Setup(d => d.Now()).Returns(DateTime.Now);

                    var controller = new ScanCardController(context, mediatorMock.Object, dateServiceMock.Object);

                    var result = await controller.Get(124, 432);

                    Assert.IsType<OkObjectResult>(result);
                    OkObjectResult okObjectResult = (OkObjectResult)result;
                    Assert.Equal(true, okObjectResult.Value);
                }
            }

            [Fact]
            public void ThePassShouldBeActivated()
            {
                using (PassContext context =
                        new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>()))
                {
                    var pass = new Pass { CardId = 124, CreatedOn = DateTime.Today.AddDays(-4) };
                    context.Passes.Add(pass);
                    context.SaveChanges();

                    var mediatorMock = new Mock<IMediator>();
                    mediatorMock.Setup(m => m.Send(It.Is<CreateScan>(c => c.CardId == 124 && c.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(555);
                    mediatorMock.Setup(m => m.Send(It.Is<ResolvePass>(r => r.CardId == 124 && r.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(pass);

                    var dateServiceMock = new Mock<IDateService>();
                    dateServiceMock.Setup(d => d.Now()).Returns(DateTime.Now);

                    var controller = new ScanCardController(context, mediatorMock.Object, dateServiceMock.Object);

                    var result = controller.Get(124, 432);

                    mediatorMock.Verify(m => m.Send(It.Is<ActivatePass>(p => p.PassId == pass.Id && p.ScanId == 555), It.IsAny<CancellationToken>()));
                }
            }

            //Pass should be activated
        }

        public class WhenCardIsScannedWithValidPassWithPreviousActivation
        {
            [Fact]
            public async Task ResultShouldBeTrue()
            {
                using (PassContext context =
                        new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>()))
                {
                    var pass = new Pass { CardId = 124, CreatedOn = DateTime.Today.AddDays(-4) };
                    context.Passes.Add(pass);
                    context.SaveChanges();

                    var passActivation = new PassActivation { PassId = pass.Id, ScanId = 555 };
                    context.PassActivations.Add(passActivation);
                    context.SaveChanges();

                    var mediatorMock = new Mock<IMediator>();
                    mediatorMock.Setup(m => m.Send(It.Is<ResolvePass>(r => r.CardId == 124 && r.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(pass);

                    var dateServiceMock = new Mock<IDateService>();
                    dateServiceMock.Setup(d => d.Now()).Returns(DateTime.Now);

                    var controller = new ScanCardController(context, mediatorMock.Object, dateServiceMock.Object);

                    var result = await controller.Get(124, 432);

                    Assert.IsType<OkObjectResult>(result);
                    OkObjectResult okObjectResult = (OkObjectResult)result;
                    Assert.Equal(true, okObjectResult.Value);
                }
            }

            [Fact]
            public void ThePassShouldNotBeActivatedAgain()
            {
                using (PassContext context =
                        new PassContext(InMemoryDbContextOptionsFactory.Create<PassContext>()))
                {
                    var pass = new Pass { CardId = 124, CreatedOn = DateTime.Today.AddDays(-4) };
                    context.Passes.Add(pass);
                    context.SaveChanges();

                    var passActivation = new PassActivation { PassId = pass.Id, ScanId = 555 };
                    context.PassActivations.Add(passActivation);
                    context.SaveChanges();

                    var mediatorMock = new Mock<IMediator>();
                    mediatorMock.Setup(m => m.Send(It.Is<CreateScan>(c => c.CardId == 124 && c.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(555);
                    mediatorMock.Setup(m => m.Send(It.Is<ResolvePass>(r => r.CardId == 124 && r.LocationId == 432), It.IsAny<CancellationToken>())).ReturnsAsync(pass);

                    var dateServiceMock = new Mock<IDateService>();
                    dateServiceMock.Setup(d => d.Now()).Returns(DateTime.Now);

                    var controller = new ScanCardController(context, mediatorMock.Object, dateServiceMock.Object);

                    var result = controller.Get(124, 432);

                    mediatorMock.Verify(m => m.Send(It.Is<ActivatePass>(p => p.PassId == pass.Id && p.ScanId == 555), It.IsAny<CancellationToken>()),Times.Never);
                }
            }
        }
    }
}

