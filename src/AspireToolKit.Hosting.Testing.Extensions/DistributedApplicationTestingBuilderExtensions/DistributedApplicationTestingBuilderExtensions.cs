namespace AspireToolKit.Hosting.Testing.Extensions.DistributedApplicationTestingBuilderExtensions;

using Aspire.Hosting.Testing;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Azure;

public static partial class DistributedApplicationTestingBuilderExtensions
{
    public static IResourceBuilder<AzureStorageResource> AzuriteEmulatorWithBindMount(this IDistributedApplicationTestingBuilder app, string storageAccountAspireResourceName, string bindMountPath, bool isReadOnly = false)
    {
        if (app.Resources.Single(
                resource => string.Equals(resource.Name, storageAccountAspireResourceName)) is not AzureStorageResource { IsEmulator: true } storageResource)
        {
            throw new InvalidOperationException($"The given resource with name {storageAccountAspireResourceName} is not an AzureStorageResource emulator.");
        }
        
        IResourceBuilder<AzureStorageResource> storageBuilder = app.CreateResourceBuilder(storageResource);

        return storageBuilder.WithAnnotation(new ContainerMountAnnotation(
            bindMountPath,
            "/data",
            ContainerMountType.BindMount,
            isReadOnly));
    }
}