using Azure.Identity;
using Microsoft.Azure.Devices;

internal class Authentication
{
    string _hubUrl = ""; 

    public Authentication(string hubUrl)
    {
        _hubUrl = hubUrl; 
    }

    async internal Task<bool> ListDevices(DefaultAzureCredential defaultAzureCredential) 
    {
        var registryManager = RegistryManager.Create(_hubUrl, defaultAzureCredential); 
        IQuery iQuery = registryManager.CreateQuery("Select * from devices", 10); 
        while (iQuery.HasMoreResults)
        {
            IEnumerable<string> devices = await iQuery.GetNextAsJsonAsync();
            foreach (string device in devices) {
                Console.WriteLine(device);
            }
        }
        

        return true; 
    }

}