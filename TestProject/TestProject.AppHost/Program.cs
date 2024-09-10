using TestProject.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder
    .AddAzureStorage(TestProjectConstants.TestStorageAccountAspireResourceName)
    // We use the args since it avoids api version check issues.
    .RunAsEmulator((r)=>r.WithArgs("azurite", "-l", "/data", "--blobHost", "0.0.0.0", "--queueHost", "0.0.0.0", "--tableHost", "0.0.0.0","--skipApiVersionCheck"));

var blob = storage.AddBlobs(TestProjectConstants.TestBlobAspireResourceName);

var queue = storage.AddQueues(TestProjectConstants.TestQueueAspireResourceName);

var table = storage.AddTables(TestProjectConstants.TestTableAspireResourceName);

builder.Build().Run();
