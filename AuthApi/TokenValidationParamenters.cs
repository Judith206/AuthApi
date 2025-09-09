using Microsoft.IdentityModel.Tokens;

internal class TokenValidationParamenters : TokenValidationParameters
{
    public bool ValidateIssuer { get; set; }
    public string ValidIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public string ValidAudience { get; set; }
    public bool validationLifetime { get; set; }
    public bool ValidaterIssuerSigningKey { get; set; }
    public SymmetricSecurityKey IssuerSingningKey { get; set; }
}