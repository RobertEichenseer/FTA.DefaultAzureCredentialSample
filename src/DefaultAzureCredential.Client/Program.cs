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

    internal static string _fileToUpload = "DemoFile.txt";

    public async Task<int> ExecuteAsync(string[] args)
    {
        string hubUrl = args[0];
        //TODO: Remove and use command line parameter
        hubUrl = "IoTHubDefaultAzureCredential242.azure-devices.net"; 

        Authentication authentication = new Authentication(hubUrl); 

        DefaultAzureCredentialOptions defaultAzureCredentialOptions = new DefaultAzureCredentialOptions() {
            ExcludeAzureCliCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeEnvironmentCredential = true, 
            ExcludeInteractiveBrowserCredential = false,
            ExcludeManagedIdentityCredential = true, 
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true, 
            ExcludeVisualStudioCredential = true
        };
        DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(defaultAzureCredentialOptions);

        await authentication.ListDevices(defaultAzureCredential); 

        return -1;
    }
}
