import { UserManagerSettings } from "oidc-client";

export const authSettings = (options: { issuer: string, clientId: string }): UserManagerSettings => ({
    "authority": options.issuer,
    "client_id": options.clientId,
    "redirect_uri": `${location.origin}/authentication/login-callback`,
    "post_logout_redirect_uri": `${location.origin}/authentication/logout-callback`,
    "response_type": "code",
    "scope": "openid profile CleanArchitecture"
});
