using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost consoleHost = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddTransient<Main>();
    })
    .Build();

Main main = consoleHost.Services.GetRequiredService<Main>();
await main.ExecuteAsync(args);

class Main
{
    public async Task<int> ExecuteAsync(string[] args)
    {
        // string hubUrl = args[0];
        // string accountName = args[1];
        //TODO: Remove and use command line parameter
        string hubUrl = "IoTHubRBAC918.azure-devices.net"; 
        string accountName = "storagerbac918";

        Authentication authentication = new Authentication(); 

        DefaultAzureCredentialOptions defaultAzureCredentialOptions = new DefaultAzureCredentialOptions() {
            ExcludeAzureCliCredential = false,
            ExcludeAzurePowerShellCredential = true,
            ExcludeEnvironmentCredential = true, 
            ExcludeInteractiveBrowserCredential = true,
            ExcludeManagedIdentityCredential = true, 
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true, 
            ExcludeVisualStudioCredential = true
        };
        DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(defaultAzureCredentialOptions);

        //Show Authentication UPN
        Console.WriteLine(await authentication.GetUpnFromToken(defaultAzureCredential));

        //Access to Blob Storage
        foreach (string blobContainer in await authentication.ListBlobStorageContainer(defaultAzureCredential, accountName)) {
            Console.WriteLine($"Blob Container Name: {blobContainer}"); 
        }

        //Access to IoT Hub
        await authentication.ListIoTHubDevices(hubUrl, defaultAzureCredential); 

        return -1;
    }
}
