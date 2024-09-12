using AutoMapper;
using Moq;

namespace Events.Tests.Application;

public class ApplicationTestBase
{

    protected readonly Mock<IMapper> _mapperMock;
    protected ApplicationTestBase()
    {
        _mapperMock = new Mock<IMapper>();
    }
}
