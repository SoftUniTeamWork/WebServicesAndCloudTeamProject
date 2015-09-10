using Moq;
using WebChat.Services.Providers;

namespace WebChat.Tests.MockedObjects
{
    public class IdProviderMock
    {
        public static Mock<IIdProvider> GetUserIdProvider()
        {
            var userIdProviderMock = new Mock<IIdProvider>();

            userIdProviderMock.Setup(x => x.GetId()).
                Returns(WebChatDataMock.MockedUserId);

            return userIdProviderMock;
        }
    }
}
