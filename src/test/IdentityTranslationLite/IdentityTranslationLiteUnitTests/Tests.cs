using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Moq;
using IdentityTranslationLite;
using IdentityTranslationLite.IotHubClient;
using Newtonsoft.Json;
using Xunit;

namespace IdentityTranslationLiteUnitTests
{
    public class Tests
    {
        [Fact]
        public async Task C2D_LeafDeviceDirectMethod()
        {
            const string leafDeviceId = "LeafDevice1";
            const string moduleDirectMethodRequestOutputName = "itmdmreqoutput";

            string directMethodRequestMessageId = null;

            // Arrange
            var moduleClientMock = new Mock<IModuleClient>();
            moduleClientMock
                .Setup(x => x.SendEventAsync(moduleDirectMethodRequestOutputName, It.IsAny<Message>()))
                .Callback<string, Message>((_, msg) => directMethodRequestMessageId = msg.MessageId);
            var leafDeviceRepoMock = new Mock<IDeviceRepository>();
            var deviceClientMock = new Mock<IDeviceClient>();
            leafDeviceRepoMock
                .Setup(x => x.Get(leafDeviceId))
                .Returns(new DeviceInfo(leafDeviceId) { DeviceClient = deviceClientMock.Object });

            var sut = new IdentityTranslationLiteModule(moduleClientMock.Object, leafDeviceRepoMock.Object);

            var methodRequestBody = new
            {
                Foo = "Bar"
            };
            var methodRequest = new MethodRequest("directMethodDummyName",
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(methodRequestBody)));
            
            // Act
            Task<MethodResponse> directMethodRequestTask = sut.LeafDeviceDirectMethod(methodRequest, leafDeviceId);
            await Task.Delay(TimeSpan.FromSeconds(1));

            // - TODO: message body necessary?
            var directMethodResponseMessage = new Message { CorrelationId = directMethodRequestMessageId };
            await sut.HandleDirectMethodResponse(directMethodResponseMessage, null);

            MethodResponse directMethodResponse = await directMethodRequestTask;

            // Assert
            Assert.NotNull(directMethodResponse);
            moduleClientMock.Verify(x => x.SendEventAsync(moduleDirectMethodRequestOutputName, It.IsAny<Message>()), Times.Exactly(1));
        }

        [Fact]
        public async Task C2D_LeafDeviceDirectMethod_UnknownDevice_ThrowsException()
        {
            const string leafDeviceId = "LeafDevice1";

            // Arrange
            var moduleClientMock = new Mock<IModuleClient>();
            var leafDeviceRepoMock = new Mock<IDeviceRepository>();
            var deviceClientMock = new Mock<IDeviceClient>();

            var sut = new IdentityTranslationLiteModule(moduleClientMock.Object, leafDeviceRepoMock.Object);

            var methodRequestBody = new
            {
                Foo = "Bar"
            };
            var methodRequest = new MethodRequest("directMethodDummyName",
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(methodRequestBody)));


            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await sut.LeafDeviceDirectMethod(methodRequest, leafDeviceId));
        }
    }
}