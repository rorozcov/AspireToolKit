namespace AspireToolKit.Hosting.Testing.Extensions.DistributedApplicationExtensions;

using Aspire.Hosting.Testing;

/// <summary>
/// A class for distributed application extensions in Aspire.Hosting.Testing projects.
/// </summary>
public static partial class DistributedApplicationExtensions
{
    /// <summary>
    /// Gets an Azure client asynchronously from the distributed application.
    /// This method mimics the Aspire provided Azure client host builder extensions,
    /// to provide a similar experience for the testing projects.
    /// </summary>
    /// <typeparam name="T">The type of Azure client to initialize.</typeparam>
    /// <param name="app">The distributed application to use </param>
    /// <param name="resourceName">The name of the resource in your application's AppHost project.</param>
    /// <param name="options">The Azure Client's options.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An Azure client configured to connect a resource in your distributed application.</returns>
    /// <exception cref="ArgumentException">An exception thrown when the connection string for the resource is not found.</exception>
    /// <exception cref="InvalidOperationException">An exception thown when we cannot initialize a client of type T.</exception>
    public static async Task<T> GetAzureClientAsync<T>(this DistributedApplication app, string resourceName, object options, CancellationToken cancellationToken = default)
        where T : class
    {
        string? connectionString = await app.GetConnectionStringAsync(resourceName, cancellationToken);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException($"Connection string for resource '{resourceName}' was not found.");
        }

        return (T?)Activator.CreateInstance(typeof(T), connectionString, options) ?? throw new InvalidOperationException($"Failed to create an instance of '{typeof(T).Name}'.");
    }
}
