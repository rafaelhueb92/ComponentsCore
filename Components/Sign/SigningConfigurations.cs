using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Components.Sign
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
                SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
            }
        }

        public void ConfigurarAutenticacao(IServiceCollection services, IConfiguration Configuration)
        {
            //Configure Signing and Token
            var signingConfigurations = new SigningConfigurations();
            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations")
            ).Configure(tokenConfigurations);

            services.AddSingleton(signingConfigurations);
            services.AddSingleton(tokenConfigurations);

            //Set authentication
            services.AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(_ =>
            {
                var paramsValidation = _.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                //Validation
                //key
                paramsValidation.ValidateIssuerSigningKey = true;
                //lifetime
                paramsValidation.ValidateLifetime = true;
                //setting token expiration time tolerance
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            //Activate token system
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }

    }
}
