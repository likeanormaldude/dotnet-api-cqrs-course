using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions) : IBlobStorageService
{
    public readonly BlobStorageSettings _blobStorageSettings = blobStorageSettingsOptions.Value;

    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        BlobServiceClient blobServiceClient = new(_blobStorageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);
        return blobClient.Uri.ToString();
    }

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null)
            return null;

        BlobServiceClient blobServiceClient = new(_blobStorageSettings.ConnectionString);

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            BlobName = GetBlobNameFromUrl(blobUrl),
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sharedKeyCredential = new Azure.Storage.StorageSharedKeyCredential(
            blobServiceClient.AccountName,
            _blobStorageSettings.AccountKey
        );

        string sasToken = sasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();

        return $"{blobUrl}?{sasToken}";
    }

    public string GetBlobNameFromUrl(string blobUrl)
    {
        if (string.IsNullOrWhiteSpace(blobUrl))
            throw new NotFoundException("Blob URL", nameof(blobUrl));

        var uri = new Uri(blobUrl);

        return uri.Segments.Last();
    }
}
