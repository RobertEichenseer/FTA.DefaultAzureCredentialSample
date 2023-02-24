import os
from typing import List
from azure.identity import DefaultAzureCredential
from Authentication import Authentication
import asyncio

class Main:
    
    def __init__(self):
        self._storage_name = ""

    async def execute_async(self,) -> int:
        if not self.check_environment_variables():
            return 0
        authentication = Authentication()

        scope = "https://storage.azure.com/"

        # Access blob storage using Service Principal
        print("List storage container using Service Principal...")
        default_azure_credential_sp = DefaultAzureCredential(
            exclude_azure_cli_credential=True,
            exclude_azure_powershell_credential=True,
            exclude_environment_credential=False,
            exclude_interactive_browser_credential=True,
            exclude_managed_identity_credential=True,
            exclude_shared_token_cache_credential=True,
            exclude_visual_studio_code_credential=True,
            exclude_visual_studio_credential=True
        )
        await authentication.list_blob_storage_container(default_azure_credential_sp, self._storage_name)
        await authentication.get_upn_from_token(default_azure_credential_sp, scope)

        # Access blob storage using CLI login
        print("\nList storage container using CLI login")
        default_azure_credential_cli = DefaultAzureCredential(
            exclude_azure_cli_credential=False,
            exclude_azure_powershell_credential=True,
            exclude_environment_credential=True,
            exclude_interactive_browser_credential=True,
            exclude_managed_identity_credential=True,
            exclude_shared_token_cache_credential=True,
            exclude_visual_studio_code_credential=True,
            exclude_visual_studio_credential=True
        )

        # Access to Blob Storage
        await authentication.list_blob_storage_container(default_azure_credential_cli, self._storage_name)

        # Show Authentication UPN
        await authentication.get_upn_from_token(default_azure_credential_cli, scope)

        return -1


    def check_environment_variables(self) -> bool:
        print("Checking Environment Variables ...")
        self._storage_name = os.environ.get("RBACSAMPLE_STORAGE_NAME")
        if not self._storage_name:
            print("Environment Variable: RBACSAMPLE_STORAGE_NAME not set!")
            return False

        if not os.environ.get("AZURE_CLIENT_ID"):
            print("Environment Variable: AZURE_CLIENT_ID not set!")
            return False

        if not os.environ.get("AZURE_TENANT_ID"):
            print("Environment Variable: AZURE_TENANT_ID not set!")
            return False

        if not os.environ.get("AZURE_CLIENT_SECRET"):
            print("Environment Variable: AZURE_CLIENT_SECRET not set!")
            return False

        print("Environment Check succeeded")
        return True

if __name__ == "__main__":
    main = Main()
    asyncio.run(main.execute_async())
