using System; 
using System.Linq; 
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

    string _hubUrl = ""; 
    string _storageName = ""; 


    public async Task<int> ExecuteAsync(string[] args)
    {
        if (!CheckEnvironmentVariables())
            return 0; 

        Authentication authentication = new Authentication(); 
        DefaultAzureCredentialOptions defaultAzureCredentialOptions; 
        string scope = "https://storage.azure.com/";

        #region Access blob storage using Service Principal
        Console.WriteLine("List storage container using Service Principal ..."); 
        defaultAzureCredentialOptions = new DefaultAzureCredentialOptions() {
            ExcludeAzureCliCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeEnvironmentCredential = false, 
            ExcludeInteractiveBrowserCredential = true,
            ExcludeManagedIdentityCredential = true, 
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true, 
            ExcludeVisualStudioCredential = true
        };
        DefaultAzureCredential defaultAzureCredentialSP = new DefaultAzureCredential(defaultAzureCredentialOptions);
        await authentication.ListBlobStorageContainer(defaultAzureCredentialSP, _storageName);
        await authentication.GetUpnFromToken(defaultAzureCredentialSP, scope);
        #endregion 

        #region Access blob storage using CLI login
        Console.WriteLine("\nList storage container using CLI login"); 
        defaultAzureCredentialOptions = new DefaultAzureCredentialOptions() {
            ExcludeAzureCliCredential = false,
            ExcludeAzurePowerShellCredential = true,
            ExcludeEnvironmentCredential = true, 
            ExcludeInteractiveBrowserCredential = true,
            ExcludeManagedIdentityCredential = true, 
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true, 
            ExcludeVisualStudioCredential = true
        };
        DefaultAzureCredential defaultAzureCredentialCLI = new DefaultAzureCredential(defaultAzureCredentialOptions);
        
        //Access to Blob Storage
        await authentication.ListBlobStorageContainer(defaultAzureCredentialCLI, _storageName);
        #endregion

        //Show Authentication UPN
        await authentication.GetUpnFromToken(defaultAzureCredentialCLI, scope);

        return -1;
    }

    private bool CheckEnvironmentVariables()
    {
        Console.WriteLine ("Checking Environment Variables ...");        
        _storageName = Environment.GetEnvironmentVariable("RBACSAMPLE_STORAGE_NAME"); 
        if (string.IsNullOrEmpty(_storageName)) {
            Console.WriteLine($"Environment Variable: RBACSAMPLE_STORAGE_NAME not set!");
            return false;
        }

        string hubName = Environment.GetEnvironmentVariable("RBACSAMPLE_HUB_NAME");
        if (string.IsNullOrEmpty(hubName)) {
            Console.WriteLine($"Environment Variable: RBACSAMPLE_STORAGE_NAME not set!");
            return false; 
        } else {
            _hubUrl = $"{hubName}.azure-devices.net";
        }

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_CLIENT_ID"))) {
            Console.WriteLine($"Environment Variable: AZURE_CLIENT_ID not set!");
            return false; 
        }
            
        
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_TENANT_ID"))) {
            Console.WriteLine($"Environment Variable: AZURE_TENANT_ID not set!");
            return false; 
        }

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET"))) {
            Console.WriteLine($"Environment Variable: AZURE_CLIENT_SECRET not set!");
            return false; 
        }

        Console.WriteLine ("Environment Check succeeded");
        return true; 
    }
}
