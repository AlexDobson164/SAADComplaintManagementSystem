public class BusinessHostedService
{
    public async Task<ValidateApiKeyResponse> ValidateApiKey(ValidateApiKeyRequest request, CancellationToken cancellationToken)
    {
        return new ValidateApiKeyResponse
        {
            BusinessReference = await BusinessTable.GetBusinessRefByApiKey(request.ApiKey, cancellationToken).ConfigureAwait(false)
        };
    }
    public async Task<GetBusinessReferenceFromHttpContextResponse> GetBusinessReferenceFromHttpContext(GetBusinessReferenceFromHttpContextRequest request, CancellationToken cancellationToken)
    {
        request.Context.Request.Headers.TryGetValue("x-api-key", out var apiKey);
  
        var businessReference = await ValidateApiKey(new ValidateApiKeyRequest
        {
            ApiKey = Guid.Parse(apiKey),
        }, cancellationToken).ConfigureAwait(false);

        return new GetBusinessReferenceFromHttpContextResponse
        {
            BusinessReference = businessReference.BusinessReference,
        };
    }
}

