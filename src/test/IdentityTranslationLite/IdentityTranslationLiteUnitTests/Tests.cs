using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Moq;
using Newtonsoft.Json;
using Xunit;
using IdentityTranslationLite;
using IdentityTranslationLite.IotHubClient;

namespace IdentityTranslationLiteUnitTests
{
    public class Tests
    {
        /// <summary>
        /// Tests the <see cref="IdentityTranslationLiteModule"/> C2D DirectMethod invocation flow:
        /// - Module receives DirectMethod handler call destined for a particular leaf device
        /// - Sends out a IoT Edge message to a separate module
        /// - Pauses its execution to wait for an incoming IoT Edge message with the response from the leaf device
        /// - Unpauses the DirectMethod handler call to return a synchronous response
        /// </summary>
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

        /// <summary>
        /// Tests the <see cref="IdentityTranslationLiteModule"/> C2D DirectMethod invocation flow, in the
        /// case where the module receives a DirectMethod handler call for an unknown device.
        /// This should result in an exception being thrown.
        /// </summary>
        [Fact]
        public async Task C2D_LeafDeviceDirectMethod_UnknownDevice_ThrowsException()
        {
            const string leafDeviceId = "LeafDevice1";

            // Arrange
            var moduleClientMock = new Mock<IModuleClient>();
            var leafDeviceRepoMock = new Mock<IDeviceRepository>();

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

        /// <summary>
        /// Tests the <see cref="IdentityTranslationLiteModule"/> C2D DirectMethod invocation flow, in the
        /// case where the module does not receive a DirectMethod invocation response message from a particular
        /// leaf device in time.
        /// This should result in an exception being thrown, which will be forwarded to IoT Hub
        /// </summary>
        [Fact]
        public async Task C2D_LeafDeviceDirectMethod_ResponseNotReceivedBeforeRequestTimeout_ThrowsException()
        {
            const string leafDeviceId = "LeafDevice1";
            const string moduleDirectMethodRequestOutputName = "itmdmreqoutput";

            // Arrange
            var moduleClientMock = new Mock<IModuleClient>();
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
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(methodRequestBody)),
                responseTimeout: TimeSpan.FromSeconds(2),
                connectionTimeout: null);

            // Act + Assert
            await Assert.ThrowsAsync<TimeoutException>(
                async () => await sut.LeafDeviceDirectMethod(methodRequest, leafDeviceId));
            moduleClientMock.Verify(x => x.SendEventAsync(moduleDirectMethodRequestOutputName, It.IsAny<Message>()), Times.Exactly(1));
        }
    }
}