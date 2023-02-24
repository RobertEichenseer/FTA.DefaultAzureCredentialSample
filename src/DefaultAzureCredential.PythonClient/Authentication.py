from azure.core.exceptions import ResourceNotFoundError
from azure.storage.blob import BlobServiceClient
from azure.identity import DefaultAzureCredential
import jwt


class Authentication:
    async def get_upn_from_token(self, default_azure_credential: DefaultAzureCredential, scope: str) -> None:
        try:
            access_token = await default_azure_credential.get_token(scope)
            jwt_security_token_handler = jwt.JWT()
            jwt_security_token = jwt_security_token_handler.decode(access_token.token)
            print(f"UPN from JWT Token: {jwt_security_token.get('upn')}")
        except Exception as ex:
            print(f"Exception: {ex}")

    async def list_blob_storage_container(self, default_azure_credential: DefaultAzureCredential, storage_account_name: str) -> None:
        blob_uri = f"https://{storage_account_name}.blob.core.windows.net"
        blob_service_client = BlobServiceClient(blob_uri, credential=default_azure_credential)
        try:
            containers = blob_service_client.list_containers()
            async for container in containers:
                print(f"...Container Name: {container.name}")
            print("Storage access succeeded")
        except ResourceNotFoundError as ex:
            print(f"Resource not found error: {ex}")
        except Exception as ex:
            print(f"Exception: {ex}")