using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class IdendityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdendityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        await EmailAndPasswordControl(context);
    }

    // user email and password control
    private async Task EmailAndPasswordControl(ResourceOwnerPasswordValidationContext context)
    {
        var existUser = await _userManager.FindByEmailAsync(context.UserName);
        if (existUser == null)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "Email veya şifreniz yanlış.");
            return;
        }

        var passwordControl = await _userManager.CheckPasswordAsync(existUser, context.Password);
        if (!passwordControl)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "Email veya şifreniz yanlış.");
            return;
        }

        context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
    }
}
