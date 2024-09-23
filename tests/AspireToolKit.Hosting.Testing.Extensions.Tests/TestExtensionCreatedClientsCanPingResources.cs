using Aspire.Hosting.Azure;

namespace AspireToolKit.Hosting.Testing.Extensions.Tests;

using System.Threading.Tasks;
using DistributedApplicationExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.AppHost;

[TestClass]
public class TestExtensionCreatedClientsCanPingResources
{
    [TestMethod]
    public async Task ThenClientsReturnOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TestProject_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        var resource = (AzureStorageResource)appHost.Resources.Single(resource =>
            string.Equals(resource.Name, TestProjectConstants.TestStorageAccountAspireResourceName));
        
        var storageBuilder = appHost.CreateResourceBuilder(resource);

        storageBuilder.WithAnnotation(new ContainerMountAnnotation(
            "/home/rorozcov/Github/AspireToolKit/tools/azuriteDataGenerator/test/data", "/data",
            ContainerMountType.BindMount, false));

        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        await resourceNotificationService.WaitForResourceAsync(TestProjectConstants.TestBlobAspireResourceName, KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync(TestProjectConstants.TestQueueAspireResourceName, KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync(TestProjectConstants.TestTableAspireResourceName, KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));
        
        // Create Clients
        var blobServiceClient = await app.GetBlobServiceClientAsync(TestProjectConstants.TestBlobAspireResourceName);
        var containerClient = blobServiceClient.GetBlobContainerClient("testcontainer");
        
        var tableServiceClient = await app.GetTableServiceClientAsync(TestProjectConstants.TestTableAspireResourceName);
        var tableClient = tableServiceClient.GetTableClient("testtable");
        
        var queueServiceClient = await app.GetQueueServiceClientAsync(TestProjectConstants.TestQueueAspireResourceName);
        var queueClient = queueServiceClient.GetQueueClient("testqueue");
        
        // Create test resources to ping the storage account
        var blobResponse = await containerClient.CreateIfNotExistsAsync();
        var queueResponse = await queueClient.CreateIfNotExistsAsync();
        var tableResponse = await tableClient.CreateIfNotExistsAsync();

        // Assert
        Assert.AreEqual(201, blobResponse.GetRawResponse().Status);
        Assert.AreEqual(204, tableResponse.GetRawResponse().Status);
        Assert.AreEqual(201, queueResponse.Status);
    }
}
