using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpaceZD.BusinessLayer.Tests.Configuration;

public class AuthOptions
{
    public const string Issuer = "SpaceZdApi";
    public const string Audience = "Frontovik";
    private const string Key = "SpaceKeyZDMandarin228";   
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}