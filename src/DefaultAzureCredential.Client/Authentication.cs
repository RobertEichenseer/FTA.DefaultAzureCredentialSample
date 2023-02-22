using System.IdentityModel.Tokens.Jwt;
using Azure; 
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Devices;
using Microsoft.IdentityModel.Tokens;

internal class Authentication
{
    async internal Task<string> GetUpnFromToken(DefaultAzureCredential defaultAzureCredential)
    {
        string claim = "https://storage.azure.com"; 
        AccessToken accessToken = await defaultAzureCredential.GetTokenAsync(
            new TokenRequestContext(new string[] {claim})
        );
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler(); 
        JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)jwtSecurityTokenHandler.ReadToken(accessToken.Token);
        return jwtSecurityToken.Claims.First(c => c.Type=="upn").Value; 
    }


    internal async Task<List<string>> ListBlobStorageContainer(DefaultAzureCredential defaultAzureCredential, string storageAccountName)
    {

        List<string> blobContainer = new List<string>(); 

        Uri blobUri = new Uri($"https://{storageAccountName}.blob.core.windows.net");

        BlobServiceClient blobServiceClient = new BlobServiceClient(blobUri, defaultAzureCredential);   

        IAsyncEnumerable<Page<BlobContainerItem>> resultPages = 
            blobServiceClient.GetBlobContainersAsync()
            .AsPages(default, 10);

        await foreach (Page<BlobContainerItem> containerPage in resultPages)
        {
            foreach (BlobContainerItem containerItem in containerPage.Values)
            {
                blobContainer.Add(containerItem.Name); 
            }
        }
        return blobContainer; 

    }

    async internal Task<List<string>> ListIoTHubDevices(string hubUrl, DefaultAzureCredential defaultAzureCredential) 
    {
        List<string> registeredDevices = new List<string>(); 

        RegistryManager registryManager = RegistryManager.Create(hubUrl, defaultAzureCredential); 
        IQuery iQuery = registryManager.CreateQuery("Select * from devices", 10); 
        while (iQuery.HasMoreResults)
        {
            IEnumerable<string> devices = await iQuery.GetNextAsJsonAsync();
            foreach (string device in devices) {
                registeredDevices.Add(device);
            }
        }

        return registeredDevices; 
    }

}