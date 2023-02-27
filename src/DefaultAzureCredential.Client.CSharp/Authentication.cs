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
    async internal Task GetUpnFromToken(DefaultAzureCredential defaultAzureCredential, string scope)
    {
        try {
            AccessToken accessToken = await defaultAzureCredential.GetTokenAsync(
                new TokenRequestContext(new string[] {scope})
            );
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler(); 
            JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)jwtSecurityTokenHandler.ReadToken(accessToken.Token);
            Console.Write($"UPN from JWT Token: {jwtSecurityToken.Claims.First(c => c.Type=="upn").Value}"); 
        } catch (Exception exE) {
            Console.WriteLine($"Exception: {exE.Message}");
        }
    }


    internal async Task ListBlobStorageContainer(DefaultAzureCredential defaultAzureCredential, string storageAccountName)
    {

        Uri blobUri = new Uri($"https://{storageAccountName}.blob.core.windows.net");

        BlobServiceClient blobServiceClient = new BlobServiceClient(blobUri, defaultAzureCredential);   

        try {
            IAsyncEnumerable<Page<BlobContainerItem>> resultPages = 
                blobServiceClient.GetBlobContainersAsync()
                .AsPages(default, 10);

            await foreach (Page<BlobContainerItem> containerPage in resultPages)
            {
                foreach (BlobContainerItem containerItem in containerPage.Values)
                {
                    Console.WriteLine($"...Container Name: {containerItem.Name}"); 
                }
            }
            Console.WriteLine("Storage access succeeded");
        } catch (AuthenticationFailedException exE){
            Console.WriteLine($"Authentication failed: {exE.Message}");
        }
    }
}