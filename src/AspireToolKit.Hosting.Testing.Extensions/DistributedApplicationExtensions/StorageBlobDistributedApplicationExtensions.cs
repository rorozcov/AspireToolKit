namespace AspireToolKit.Hosting.Testing.Extensions.DistributedApplicationExtensions;

using Azure.Storage.Blobs;

/// <summary>
/// A class for distributed application extensions in Aspire.Hosting.Testing projects for Blob Storage.
/// </summary>
public static partial class DistributedApplicationExtensions
{
    /// <summary>
    /// Gets an BlobServiceClient asynchronously from the distributed application.
    /// This method mimics the experience provided by <see cref="Microsoft.Extensions.Hosting.AspireBlobStorageExtensions.AddAzureBlobClient(Microsoft.Extensions.Hosting.IHostApplicationBuilder, string, Action{Aspire.Azure.Storage.Blobs.AzureStorageBlobsSettings}?, Action{Azure.Core.Extensions.IAzureClientBuilder{BlobServiceClient, BlobClientOptions}}?)"/>
    /// where the user provides the connection name and wll receive the client, though not through Dependency Injection, but directly.
    /// </summary>
    /// <param name="app">The distributed application to use </param>
    /// <param name="resourceName">The name of the blob in your application's AppHost project.</param>
    /// <param name="options">The options to configure the BlobServiceClient.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An <see cref="BlobServiceClient"/> configured to work with your storage blob registered in the AppHost project.</returns>
    public static async Task<BlobServiceClient> GetBlobServiceClientAsync(this DistributedApplication app, string resourceName, BlobClientOptions? options = null, CancellationToken cancellationToken = default)
    {
        return await app.GetAzureClientAsync<BlobServiceClient>(resourceName, options ?? new BlobClientOptions(), cancellationToken: cancellationToken);
    }
}
