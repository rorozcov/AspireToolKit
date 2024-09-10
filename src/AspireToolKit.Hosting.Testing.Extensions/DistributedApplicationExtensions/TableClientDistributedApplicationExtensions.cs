namespace AspireToolKit.Hosting.Testing.Extensions.DistributedApplicationExtensions;

using Azure.Data.Tables;

/// <summary>
/// A class for distributed application extensions in Aspire.Hosting.Testing projects for Table store.
/// </summary>
public static partial class DistributedApplicationExtensions
{
    /// <summary>
    /// Gets a TableServiceClient asynchronously from the distributed application.
    /// This method mimics the experience provided by <see cref="Microsoft.Extensions.Hosting.AspireTablesExtensions.AddAzureTableClient(Microsoft.Extensions.Hosting.IHostApplicationBuilder, string, Action{Aspire.Azure.Data.Tables.AzureDataTablesSettings}?, Action{Azure.Core.Extensions.IAzureClientBuilder{TableServiceClient, TableClientOptions}}?)"/>
    /// where the user provides the connection name and will receive the client, though not through Dependency Injection, but directly.
    /// </summary>
    /// <param name="app">The distributed application to use </param>
    /// <param name="resourceName">The name of the table in your application's AppHost project.</param>
    /// <param name="options">The options to configure the TableServiceClient.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An <see cref="TableServiceClient"/> configured to work with your table registered in the AppHost project.</returns>
    public static async Task<TableServiceClient> GetTableServiceClientAsync(this DistributedApplication app, string resourceName, TableClientOptions? options = null, CancellationToken cancellationToken = default)
    {
        return await app.GetAzureClientAsync<TableServiceClient>(resourceName, options ?? new TableClientOptions(), cancellationToken);
    }
}
