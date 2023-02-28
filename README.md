# FTA.DefaultAzureCredentialSample

Sample to show the usage of DefaultAzureCredential()

The DefaultAzureCredential class provided by the Azure SDKs allows applications to use different authentication methods against Azure Services.

By using the DefaultAzureCredential class, you can simplify the authentication process for your application, improve its security, make it more consistent, and future-proof it against changes in authentication methods.

You configure the appropriate authentication method for each environment, and DefaultAzureCredential automatically detects and uses that authentication method. The use of DefaultAzureCredential is preferred over manually coding conditional logic or feature flags to use different authentication methods in different environments.

## Sequence

The default authentication sequence in which DefaultAzureCredential looks for credentials is shown in the following diagram and table:

![Sequence](img/Sequence.png)

However, you may not want to use the default behavior. In such cases, you can customize the class by specifying parameters or options that best suit your application's requirements.

By configuring the parameters or options of the DefaultAzureCredential class, you can customize the authentication process to meet your specific needs by excluding authentication methods.

| Authentication Type | Description |
|---------------------|-------------|
|Environment|The DefaultAzureCredential object reads a set of environment variables to determine if an application service principal was set for the application. $Env:AZURE_CLIENT_ID = $principalAppId  $Env:AZURE_TENANT_ID = $principalTenant  $ENV:AZURE_CLIENT_SECRET = $principalPassword|
|Managed Identity|If the application is deployed in an Azure host with managed identity enabled, DefaultAzureCredential authenticates the app to Azure by using that managed identity.|
|Visual Studio (Code)|The DefaultAzureCredential will authenticate with the account authenticated via the Visual Studio Code Azure Account plugin.|
|Azure CLI|If you have authenticated to Azure by using the az login command in the Azure CLI, DefaultAzureCredential can authenticate the application to Azure by using that same account.|
|Azure Power Shell|If you have authenticated to Azure by using the Connect-AzAccount cmdlet from Azure PowerShell, DefaultAzureCredential can authenticate the app to Azure by using that same account.|
|Interactive|DefaultAzureCredential interactively authenticates you via the current system's default browser.|



## Known Issues

[GitHub Error Report - VS Code Add-In](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/TROUBLESHOOTING.md#troubleshoot-visualstudiocodecredential-authentication-issues)
