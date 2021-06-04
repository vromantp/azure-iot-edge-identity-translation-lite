using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport;
using Microsoft.Azure.Devices.Shared;

namespace IdentityTranslationLite.IotHubClient
{
    public class DeviceClientAdapter : IDeviceClient
    {
        private readonly DeviceClient _deviceClient;

        public DeviceClientAdapter(DeviceClient deviceClient)
        {
            _deviceClient = deviceClient;
        }

        public Task AbandonAsync(string lockToken) => _deviceClient.AbandonAsync(lockToken);

        public Task AbandonAsync(Message message, CancellationToken cancellationToken) =>
            _deviceClient.AbandonAsync(message, cancellationToken);

        public Task AbandonAsync(Message message) => _deviceClient.AbandonAsync(message);

        public Task AbandonAsync(string lockToken, CancellationToken cancellationToken) =>
            _deviceClient.AbandonAsync(lockToken, cancellationToken);

        public Task CloseAsync(CancellationToken cancellationToken) => _deviceClient.CloseAsync(cancellationToken);

        public Task CloseAsync() => _deviceClient.CloseAsync();

        public Task CompleteAsync(string lockToken) => _deviceClient.CompleteAsync(lockToken);

        public Task CompleteAsync(string lockToken, CancellationToken cancellationToken) =>
            _deviceClient.CompleteAsync(lockToken, cancellationToken);

        public Task CompleteAsync(Message message) => _deviceClient.CompleteAsync(message);

        public Task CompleteAsync(Message message, CancellationToken cancellationToken) =>
            _deviceClient.CompleteAsync(message, cancellationToken);

        public Task CompleteFileUploadAsync(FileUploadCompletionNotification notification,
            CancellationToken cancellationToken = default) =>
            _deviceClient.CompleteFileUploadAsync(notification, cancellationToken);

        public Task<FileUploadSasUriResponse> GetFileUploadSasUriAsync(FileUploadSasUriRequest request,
            CancellationToken cancellationToken = default) =>
            _deviceClient.GetFileUploadSasUriAsync(request, cancellationToken);

        public Task<Twin> GetTwinAsync(CancellationToken cancellationToken) =>
            _deviceClient.GetTwinAsync(cancellationToken);

        public Task<Twin> GetTwinAsync() => _deviceClient.GetTwinAsync();

        public Task OpenAsync(CancellationToken cancellationToken) => _deviceClient.OpenAsync(cancellationToken);

        public Task OpenAsync() => _deviceClient.OpenAsync();

        public Task<Message> ReceiveAsync() => _deviceClient.ReceiveAsync();

        public Task<Message> ReceiveAsync(CancellationToken cancellationToken) =>
            _deviceClient.ReceiveAsync(cancellationToken);

        public Task<Message> ReceiveAsync(TimeSpan timeout) => _deviceClient.ReceiveAsync(timeout);

        public Task RejectAsync(Message message) => _deviceClient.RejectAsync(message);

        public Task RejectAsync(string lockToken) => _deviceClient.RejectAsync(lockToken);

        public Task RejectAsync(string lockToken, CancellationToken cancellationToken) =>
            _deviceClient.RejectAsync(lockToken, cancellationToken);

        public Task RejectAsync(Message message, CancellationToken cancellationToken) =>
            _deviceClient.RejectAsync(message, cancellationToken);

        public Task SendEventAsync(Message message, CancellationToken cancellationToken) =>
            _deviceClient.SendEventAsync(message, cancellationToken);

        public Task SendEventAsync(Message message) => _deviceClient.SendEventAsync(message);

        public Task SendEventBatchAsync(IEnumerable<Message> messages) => _deviceClient.SendEventBatchAsync(messages);

        public Task SendEventBatchAsync(IEnumerable<Message> messages, CancellationToken cancellationToken) =>
            _deviceClient.SendEventBatchAsync(messages, cancellationToken);

        public void SetConnectionStatusChangesHandler(ConnectionStatusChangesHandler statusChangesHandler) =>
            _deviceClient.SetConnectionStatusChangesHandler(statusChangesHandler);

        public Task SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback callback, object userContext) =>
            _deviceClient.SetDesiredPropertyUpdateCallbackAsync(callback, userContext);

        public Task SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback callback, object userContext,
            CancellationToken cancellationToken) =>
            _deviceClient.SetDesiredPropertyUpdateCallbackAsync(callback, userContext, cancellationToken);

        public Task SetMethodDefaultHandlerAsync(MethodCallback methodHandler, object userContext) =>
            _deviceClient.SetMethodDefaultHandlerAsync(methodHandler, userContext);

        public Task SetMethodDefaultHandlerAsync(MethodCallback methodHandler, object userContext,
            CancellationToken cancellationToken) =>
            _deviceClient.SetMethodDefaultHandlerAsync(methodHandler, userContext, cancellationToken);

        public Task SetMethodHandlerAsync(string methodName, MethodCallback methodHandler, object userContext,
            CancellationToken cancellationToken) =>
            _deviceClient.SetMethodHandlerAsync(methodName, methodHandler, userContext, cancellationToken);

        public Task SetMethodHandlerAsync(string methodName, MethodCallback methodHandler, object userContext) =>
            _deviceClient.SetMethodHandlerAsync(methodName, methodHandler, userContext);

        public Task SetReceiveMessageHandlerAsync(ReceiveMessageCallback messageHandler, object userContext,
            CancellationToken cancellationToken = default) =>
            _deviceClient.SetReceiveMessageHandlerAsync(messageHandler, userContext, cancellationToken);

        public void SetRetryPolicy(IRetryPolicy retryPolicy) => _deviceClient.SetRetryPolicy(retryPolicy);

        public Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties,
            CancellationToken cancellationToken) =>
            _deviceClient.UpdateReportedPropertiesAsync(reportedProperties, cancellationToken);

        public Task UpdateReportedPropertiesAsync(TwinCollection reportedProperties) =>
            _deviceClient.UpdateReportedPropertiesAsync(reportedProperties);


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _deviceClient.Dispose();
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
