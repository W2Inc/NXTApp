namespace NXTBackend.API.Models.Responses.Auth;

public class AuthResponseDto : BaseResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
}

// Client -> Frontend Login GitHub -> Frontend Server (yo ich redirecte dich zur App Kacke für OpenID Connect flow) -> GitHub -> Frontend Server (yo client du hast Ja gesagt, hier ist dein GitHub access token) -> Client
// -> Frontend Server (yo mach mal den login finished für meinen shit, hier ist mein github access token) -> Backend (yo jemand will einloggen, hier ist sein github access token) -> Backend: GitHub User Info -> Frontend Server (yo konnte user info getten, hier ist JWT für API)
// -> Frontned Server: Store JWT in cookies -> Client (yo klappt alles, du bist eingeloggt) + redirect -> Client: Happy

// 2 Tabs, 1 action A, 1 action B
// -> beide Tabs haben in den Cookies old JWT -> deine Middleware im Frontend sagt, jo jimmy wir müssen Refresh Token usen damit neuer gültiger JWT spawned -> beide tabs machen das = race condition -> doppelt usage von Refresh Token
// -> du bist am ass

// = BIG ASS TECHNICAL DEBT

// Du hast:

// eigenen OpenID Connect Server: JimmyAuthServer
// -> Du supportest weitere OpenID Connect Provider -> Facebook, Google, Twitter, ...
// -> Du hast eigene clients 
// -> Kontrolle über alle ausgegebenen access tokens + welcher client (Service)
// -> Was für 2FA willst du supporten
// -> Mailing
// -> Monitoring
// -> Du musst UI machen für "Login", "Register", "Authorize" (zeigen was die Clients für Infos accessen wollen)
// -> 3 Client Types = implicit (einmal authorize required), explicit (jedes mal authorize required), internal (immer authorized)
// -> Hier nur User Information (name, email, phone number, location)
// -> Ich erlaube custom clients
// -> Security: PKCE, Asymmetrische Verschlüsseling (modern), Signiert
// .API, .Core, .Domain -> .API (!!WEAK!!)
// Wenn du zu weak bist -> https://github.com/keycloak/keycloak

// Frontend Service für: Account Settings
// -> Hat client in: JimmyAuthServer -> JimmyAccountSettingsClient (client_secret, client_name, ...) -> Internal Client Type -> User hat automatisch "authorize" accepted
// -> Wenn du "Account Settings" willst, kommt der JimmyAuthFlow für dein Client (user, pass, 2fa, alles macht der obere Service)
// -> Du hast "id_token" (user information), "access_token" (user access token für API action), "refresh_token" (token für neuer id und- access token)

// Game Service für: Game
// -> Hat client in: JimmyAuthServer -> JimmyAccountSettingsClient (client_token, client_name, ...) -> Internal Client Type -> User hat automatisch "authorize" accepted
// -> Wenn du "Account Settings" willst, kommt der JimmyAuthFlow für dein Client (user, pass, 2fa, alles macht der obere Service)
// -> Du hast "id_token" (user information), "access_token" (user access token für API action), "refresh_token" (token für neuer id und- access token)
// -> eigene DB für Kills/Deaths/XP

// Privacy: Du bist gezwungen einen Button zu haben wo du **alle** Daten von einem User in ein Humanes Format zurückgeben musst.

// -> Du kannst jedes OAuth2.0/OpenID Connect library usen wie auth-js aka next-auth (svelte, react, ...) und bist unabhängig