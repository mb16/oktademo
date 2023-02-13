using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace oktaMVC.Utils;

public class ClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();

        if (!principal.HasClaim(claim => claim.Type == ClaimTypes.Role))
        {

            foreach(var claim in principal.Claims){

                /* Note the claims are coming is as an OpenIdConnect json array.  Not sure
                what config is responsible for either source. */

                if (claim.Type == "groups" && claim.Issuer == ""){
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, claim.Value));
                }
            }
        }

        principal.AddIdentity(claimsIdentity);
        return Task.FromResult(principal);
    }
}