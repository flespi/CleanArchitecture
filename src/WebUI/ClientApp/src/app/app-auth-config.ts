import { UserManagerSettings } from "oidc-client";
import { environment } from "src/environments/environment";

export const authSettings : UserManagerSettings = {
    "authority": environment.auth.authority,
    "client_id": environment.auth.client_id,
    "redirect_uri": `${location.origin}/authentication/login-callback`,
    "post_logout_redirect_uri": `${location.origin}/authentication/logout-callback`,
    "response_type": "code",
    "scope": "openid profile CleanArchitecture_API"
};