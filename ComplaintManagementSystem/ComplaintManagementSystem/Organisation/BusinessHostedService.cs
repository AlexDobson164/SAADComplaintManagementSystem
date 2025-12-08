public class BusinessHostedService
{
    public ValidateApiKeyResponse ValidateApiKey(ValidateApiKeyRequest request)
    {
        return new ValidateApiKeyResponse
        {
            BusinessReference = BusinessTable.GetBusinessRefByApiKey(request.ApiKey)
        };
    }
}

