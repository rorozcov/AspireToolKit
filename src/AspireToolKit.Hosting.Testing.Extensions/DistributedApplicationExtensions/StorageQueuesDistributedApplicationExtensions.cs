namespace AspireToolKit.Hosting.Testing.Extensions.DistributedApplicationExtensions;

using Azure.Storage.Queues;

/// <summary>
/// A class for distributed application extensions in Aspire.Hosting.Testing projects for Storage Queues.
/// </summary>
public static partial class DistributedApplicationExtensions
{
    /// <summary>
    /// Gets a QueueServiceClient asynchronously from the distributed application.
    /// This method mimics the experience provided by <see cref="Microsoft.Extensions.Hosting.AspireQueueStorageExtensions.AddAzureQueueClient(Microsoft.Extensions.Hosting.IHostApplicationBuilder, string, Action{Aspire.Azure.Storage.Queues.AzureStorageQueuesSettings}?, Action{Azure.Core.Extensions.IAzureClientBuilder{QueueServiceClient, QueueClientOptions}}?)"/>
    /// where the user provides the connection name and will receive the client, though not through Dependency Injection, but directly.
    /// </summary>
    /// <param name="app">The distributed applicaiton to use </param>
    /// <param name="resourceName">The name of the storage queue in your application's AppHost project.</param>
    /// <param name="options">The options to configure the QueueServiceClient.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An <see cref="QueueServiceClient"/> configured to work with your storage queue registered in the AppHost project.</returns>
    public static async Task<QueueServiceClient> GetQueueServiceClientAsync(this DistributedApplication app, string resourceName, QueueClientOptions? options = null, CancellationToken cancellationToken = default)
    {
        return await app.GetAzureClientAsync<QueueServiceClient>(resourceName, options ?? new QueueClientOptions(), cancellationToken);
    }
}
