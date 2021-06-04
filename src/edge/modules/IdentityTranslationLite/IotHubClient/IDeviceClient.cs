using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport;
using Microsoft.Azure.Devices.Shared;

namespace IdentityTranslationLite.IotHubClient
{
    public interface IDeviceClient : IDisposable
    {
        //
        // Summary:
        //     Puts a received message back onto the device queue
        //
        // Parameters:
        //   lockToken:
        //     The message lockToken.
        //
        // Returns:
        //     The previously received message
        //
        // Remarks:
        //     You cannot Abandon a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task AbandonAsync(string lockToken);

        //
        // Summary:
        //     Puts a received message back onto the device queue
        //
        // Parameters:
        //   message:
        //     The message.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The lock identifier for the previously received message
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        //
        // Remarks:
        //     You cannot Abandon a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task AbandonAsync(Message message, CancellationToken cancellationToken);

        //
        // Summary:
        //     Puts a received message back onto the device queue
        //
        // Parameters:
        //   message:
        //     The message.
        //
        // Returns:
        //     The lock identifier for the previously received message
        //
        // Remarks:
        //     You cannot Abandon a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task AbandonAsync(Message message);
        //
        // Summary:
        //     Puts a received message back onto the device queue
        //
        // Parameters:
        //   lockToken:
        //     The message lockToken.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The previously received message
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        //
        // Remarks:
        //     You cannot Abandon a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task AbandonAsync(string lockToken, CancellationToken cancellationToken);

        //
        // Summary:
        //     Close the DeviceClient instance
        //
        // Parameters:
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task CloseAsync(CancellationToken cancellationToken);

        //
        // Summary:
        //     Close the DeviceClient instance
        Task CloseAsync();

        //
        // Summary:
        //     Deletes a received message from the device queue
        //
        // Parameters:
        //   lockToken:
        //     The message lockToken.
        //
        // Returns:
        //     The lock identifier for the previously received message
        Task CompleteAsync(string lockToken);

        //
        // Summary:
        //     Deletes a received message from the device queue
        //
        // Parameters:
        //   lockToken:
        //     The message lockToken.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The lock identifier for the previously received message
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task CompleteAsync(string lockToken, CancellationToken cancellationToken);

        //
        // Summary:
        //     Deletes a received message from the device queue
        //
        // Parameters:
        //   message:
        //     The message.
        //
        // Returns:
        //     The previously received message
        Task CompleteAsync(Message message);

        //
        // Summary:
        //     Deletes a received message from the device queue
        //
        // Parameters:
        //   message:
        //     The message.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The previously received message
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task CompleteAsync(Message message, CancellationToken cancellationToken);

        //
        // Summary:
        //     Notify IoT Hub that a device's file upload has finished. See this documentation
        //     for more details
        //
        // Parameters:
        //   notification:
        //     The notification details, including if the file upload succeeded.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     The task to await.
        Task CompleteFileUploadAsync(FileUploadCompletionNotification notification, CancellationToken cancellationToken = default);
        
        //
        // Summary:
        //     Get a file upload SAS URI which the Azure Storage SDK can use to upload a file
        //     to blob for this device. See this documentation for more details
        //
        // Parameters:
        //   request:
        //     The request details for getting the SAS URI, including the destination blob name.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     The file upload details to be used with the Azure Storage SDK in order to upload
        //     a file from this device.
        Task<FileUploadSasUriResponse> GetFileUploadSasUriAsync(FileUploadSasUriRequest request, CancellationToken cancellationToken = default);

        //
        // Summary:
        //     Retrieve the device twin properties for the current device. For the complete
        //     device twin object, use Microsoft.Azure.Devices.RegistryManager.GetTwinAsync(string
        //     deviceId).
        //
        // Parameters:
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The device twin object for the current device
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task<Twin> GetTwinAsync(CancellationToken cancellationToken);

        //
        // Summary:
        //     Retrieve the device twin properties for the current device. For the complete
        //     device twin object, use Microsoft.Azure.Devices.RegistryManager.GetTwinAsync(string
        //     deviceId).
        //
        // Returns:
        //     The device twin object for the current device
        Task<Twin> GetTwinAsync();

        //
        // Summary:
        //     Explicitly open the DeviceClient instance. A cancellation token to cancel the
        //     operation. Thrown when the operation has been canceled.
        Task OpenAsync(CancellationToken cancellationToken);

        //
        // Summary:
        //     Explicitly open the DeviceClient instance.
        Task OpenAsync();

        //
        // Summary:
        //     Receive a message from the device queue using the default timeout. After handling
        //     a received message, a client should call Microsoft.Azure.Devices.Client.DeviceClient.CompleteAsync(Microsoft.Azure.Devices.Client.Message),
        //     Microsoft.Azure.Devices.Client.DeviceClient.AbandonAsync(Microsoft.Azure.Devices.Client.Message),
        //     or Microsoft.Azure.Devices.Client.DeviceClient.RejectAsync(Microsoft.Azure.Devices.Client.Message),
        //     and then dispose the message.
        //
        // Returns:
        //     The receive message or null if there was no message until the default timeout
        //
        // Remarks:
        //     You cannot Reject or Abandon messages over MQTT protocol. For more details, see
        //     https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task<Message> ReceiveAsync();

        //
        // Summary:
        //     Receive a message from the device queue using the cancellation token. After handling
        //     a received message, a client should call Microsoft.Azure.Devices.Client.DeviceClient.CompleteAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     Microsoft.Azure.Devices.Client.DeviceClient.AbandonAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     or Microsoft.Azure.Devices.Client.DeviceClient.RejectAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     and then dispose the message.
        //
        // Parameters:
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The receive message or null if there was no message until CancellationToken Expired
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        //
        // Remarks:
        //     You cannot Reject or Abandon messages over MQTT protocol. For more details, see
        //     https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task<Message> ReceiveAsync(CancellationToken cancellationToken);

        //
        // Summary:
        //     Receive a message from the device queue using the cancellation token. After handling
        //     a received message, a client should call Microsoft.Azure.Devices.Client.DeviceClient.CompleteAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     Microsoft.Azure.Devices.Client.DeviceClient.AbandonAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     or Microsoft.Azure.Devices.Client.DeviceClient.RejectAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     and then dispose the message.
        //
        // Returns:
        //     The receive message or null if there was no message until the specified time
        //     has elapsed
        //
        // Remarks:
        //     You cannot Reject or Abandon messages over MQTT protocol. For more details, see
        //     https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task<Message> ReceiveAsync(TimeSpan timeout);

        //
        // Summary:
        //     Deletes a received message from the device queue and indicates to the server
        //     that the message could not be processed.
        //
        // Parameters:
        //   message:
        //     The message.
        //
        // Returns:
        //     The lock identifier for the previously received message
        //
        // Remarks:
        //     You cannot Reject a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task RejectAsync(Message message);

        //
        // Summary:
        //     Deletes a received message from the device queue and indicates to the server
        //     that the message could not be processed.
        //
        // Parameters:
        //   lockToken:
        //     The message lockToken.
        //
        // Returns:
        //     The previously received message
        //
        // Remarks:
        //     You cannot Reject a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task RejectAsync(string lockToken);

        //
        // Summary:
        //     Deletes a received message from the device queue and indicates to the server
        //     that the message could not be processed.
        //
        // Parameters:
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        //   lockToken:
        //     The message lockToken.
        //
        // Returns:
        //     The previously received message
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        //
        // Remarks:
        //     You cannot Reject a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task RejectAsync(string lockToken, CancellationToken cancellationToken);

        //
        // Summary:
        //     Deletes a received message from the device queue and indicates to the server
        //     that the message could not be processed.
        //
        // Parameters:
        //   message:
        //     The message.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The lock identifier for the previously received message
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        //
        // Remarks:
        //     You cannot Reject a message over MQTT protocol. For more details, see https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-c2d#the-cloud-to-device-message-life-cycle.
        Task RejectAsync(Message message, CancellationToken cancellationToken);

        //
        // Summary:
        //     Sends an event to a hub
        //
        // Parameters:
        //   message:
        //     The message to send. Should be disposed after sending.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The task to await
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task SendEventAsync(Message message, CancellationToken cancellationToken);

        //
        // Summary:
        //     Sends an event to a hub
        //
        // Parameters:
        //   message:
        //     The message to send. Should be disposed after sending.
        //
        // Returns:
        //     The task to await
        Task SendEventAsync(Message message);

        //
        // Summary:
        //     Sends a batch of events to IoT hub. Use AMQP or HTTPs for a true batch operation.
        //     MQTT will just send the messages one after the other.
        //
        // Parameters:
        //   messages:
        //     A list of one or more messages to send. The messages should be disposed after
        //     sending.
        //
        // Returns:
        //     The task to await
        Task SendEventBatchAsync(IEnumerable<Message> messages);

        //
        // Summary:
        //     Sends a batch of events to IoT hub. Use AMQP or HTTPs for a true batch operation.
        //     MQTT will just send the messages one after the other.
        //
        // Parameters:
        //   messages:
        //     An IEnumerable set of Message objects.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Returns:
        //     The task to await
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task SendEventBatchAsync(IEnumerable<Message> messages, CancellationToken cancellationToken);

        //
        // Summary:
        //     Sets a new delegate for the connection status changed callback. If a delegate
        //     is already associated, it will be replaced with the new delegate. Note that this
        //     callback will never be called if the client is configured to use HTTP, as that
        //     protocol is stateless. The name of the method to associate with the delegate.
        void SetConnectionStatusChangesHandler(ConnectionStatusChangesHandler statusChangesHandler);

        //
        // Summary:
        //     Set a callback that will be called whenever the client receives a state update
        //     (desired or reported) from the service. Set callback value to null to clear.
        //
        // Parameters:
        //   callback:
        //     Callback to call after the state update has been received and applied
        //
        //   userContext:
        //     Context object that will be passed into callback
        //
        // Remarks:
        //     This has the side-effect of subscribing to the PATCH topic on the service.
        Task SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback callback, object userContext);

        //
        // Summary:
        //     Set a callback that will be called whenever the client receives a state update
        //     (desired or reported) from the service. Set callback value to null to clear.
        //
        // Parameters:
        //   callback:
        //     Callback to call after the state update has been received and applied
        //
        //   userContext:
        //     Context object that will be passed into callback
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        //
        // Remarks:
        //     This has the side-effect of subscribing to the PATCH topic on the service.
        Task SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback callback, object userContext, CancellationToken cancellationToken);

        //
        // Summary:
        //     Sets a new delegate that is called for a method that doesn't have a delegate
        //     registered for its name. If a default delegate is already registered it will
        //     replace with the new delegate. A method handler can be unset by passing a null
        //     MethodCallback.
        //
        // Parameters:
        //   methodHandler:
        //     The delegate to be used when a method is called by the cloud service and there
        //     is no delegate registered for that method name.
        //
        //   userContext:
        //     Generic parameter to be interpreted by the client code.
        Task SetMethodDefaultHandlerAsync(MethodCallback methodHandler, object userContext);

        //
        // Summary:
        //     Sets a new delegate that is called for a method that doesn't have a delegate
        //     registered for its name. If a default delegate is already registered it will
        //     replace with the new delegate. A method handler can be unset by passing a null
        //     MethodCallback.
        //
        // Parameters:
        //   methodHandler:
        //     The delegate to be used when a method is called by the cloud service and there
        //     is no delegate registered for that method name.
        //
        //   userContext:
        //     Generic parameter to be interpreted by the client code.
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task SetMethodDefaultHandlerAsync(MethodCallback methodHandler, object userContext, CancellationToken cancellationToken);

        //
        // Summary:
        //     Sets a new delegate for the named method. If a delegate is already associated
        //     with the named method, it will be replaced with the new delegate. A method handler
        //     can be unset by passing a null MethodCallback. The name of the method to associate
        //     with the delegate. The delegate to be used when a method with the given name
        //     is called by the cloud service. generic parameter to be interpreted by the client
        //     code. A cancellation token to cancel the operation. Thrown when the operation
        //     has been canceled.
        Task SetMethodHandlerAsync(string methodName, MethodCallback methodHandler, object userContext, CancellationToken cancellationToken);

        //
        // Summary:
        //     Sets a new delegate for the named method. If a delegate is already associated
        //     with the named method, it will be replaced with the new delegate. A method handler
        //     can be unset by passing a null MethodCallback. The name of the method to associate
        //     with the delegate. The delegate to be used when a method with the given name
        //     is called by the cloud service. generic parameter to be interpreted by the client
        //     code.
        Task SetMethodHandlerAsync(string methodName, MethodCallback methodHandler, object userContext);

        //
        // Summary:
        //     Sets a new delegate for receiving a message from the device queue using the default
        //     timeout. After handling a received message, a client should call Microsoft.Azure.Devices.Client.DeviceClient.CompleteAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     Microsoft.Azure.Devices.Client.DeviceClient.AbandonAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     or Microsoft.Azure.Devices.Client.DeviceClient.RejectAsync(Microsoft.Azure.Devices.Client.Message,System.Threading.CancellationToken),
        //     and then dispose the message. If a null delegate is passed, it will disable the
        //     callback triggered on receiving messages from the service. The delegate to be
        //     used when a could to device message is received by the client. Generic parameter
        //     to be interpreted by the client code. A cancellation token to cancel the operation.
        Task SetReceiveMessageHandlerAsync(ReceiveMessageCallback messageHandler, object userContext, CancellationToken cancellationToken = default);

        //
        // Summary:
        //     Sets the retry policy used in the operation retries. The change will take effect
        //     after any in-progress operations.
        //
        // Parameters:
        //   retryPolicy:
        //     The retry policy. The default is new ExponentialBackoff(int.MaxValue, TimeSpan.FromMilliseconds(100),
        //     TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(100));
        void SetRetryPolicy(IRetryPolicy retryPolicy);

        //
        // Summary:
        //     Push reported property changes up to the service.
        //
        // Parameters:
        //   reportedProperties:
        //     Reported properties to push
        //
        //   cancellationToken:
        //     A cancellation token to cancel the operation.
        //
        // Exceptions:
        //   T:System.OperationCanceledException:
        //     Thrown when the operation has been canceled.
        Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties, CancellationToken cancellationToken);

        //
        // Summary:
        //     Push reported property changes up to the service.
        //
        // Parameters:
        //   reportedProperties:
        //     Reported properties to push
        Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties);
    }
}
