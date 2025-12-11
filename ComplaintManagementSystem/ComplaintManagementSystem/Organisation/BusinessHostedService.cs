public class BusinessHostedService
{
    public ValidateApiKeyResponse ValidateApiKey(ValidateApiKeyRequest request)
    {
        return new ValidateApiKeyResponse
        {
            BusinessReference = BusinessTable.GetBusinessRefByApiKey(request.ApiKey)
        };
    }
    public GetBusinessReferenceFromHttpContextResponse GetBusinessReferenceFromHttpContext(GetBusinessReferenceFromHttpContextRequest request)
    {
        request.Context.Request.Headers.TryGetValue("x-api-key", out var apiKey);
        return new GetBusinessReferenceFromHttpContextResponse
        {
            BusinessReference = ValidateApiKey(new ValidateApiKeyRequest
            { 
                ApiKey = Guid.Parse(apiKey) // this can not be null as it would not got through the middleware if there was no api key
            }).BusinessReference
        };
    }
}

