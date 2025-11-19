using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CleanArchitecture.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IIdentityAccessor> _identityAccessor = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();
        _identityAccessor = new Mock<IIdentityAccessor>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _identityAccessor.Setup(x => x.User).Returns(new BasicUser(Guid.NewGuid().ToString()));

        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object, _identityAccessor.Object, _identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        _identityAccessor.Setup(x => x.User).Returns(new BasicUser(string.Empty));

        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object, _identityAccessor.Object, _identityService.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
