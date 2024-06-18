namespace NXTBackend.API.Models.Responses.Auth;

public class AuthResponseDto : BaseResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
}

// Client -> Frontend Login GitHub -> Frontend Server (yo ich redirecte dich zur App Kacke f�r OpenID Connect flow) -> GitHub -> Frontend Server (yo client du hast Ja gesagt, hier ist dein GitHub access token) -> Client
// -> Frontend Server (yo mach mal den login finished f�r meinen shit, hier ist mein github access token) -> Backend (yo jemand will einloggen, hier ist sein github access token) -> Backend: GitHub User Info -> Frontend Server (yo konnte user info getten, hier ist JWT f�r API)
// -> Frontned Server: Store JWT in cookies -> Client (yo klappt alles, du bist eingeloggt) + redirect -> Client: Happy

// 2 Tabs, 1 action A, 1 action B
// -> beide Tabs haben in den Cookies old JWT -> deine Middleware im Frontend sagt, jo jimmy wir m�ssen Refresh Token usen damit neuer g�ltiger JWT spawned -> beide tabs machen das = race condition -> doppelt usage von Refresh Token
// -> du bist am ass

// = BIG ASS TECHNICAL DEBT

// Du hast:

// eigenen OpenID Connect Server: JimmyAuthServer
// -> Du supportest weitere OpenID Connect Provider -> Facebook, Google, Twitter, ...
// -> Du hast eigene clients 
// -> Kontrolle �ber alle ausgegebenen access tokens + welcher client (Service)
// -> Was f�r 2FA willst du supporten
// -> Mailing
// -> Monitoring
// -> Du musst UI machen f�r "Login", "Register", "Authorize" (zeigen was die Clients f�r Infos accessen wollen)
// -> 3 Client Types = implicit (einmal authorize required), explicit (jedes mal authorize required), internal (immer authorized)
// -> Hier nur User Information (name, email, phone number, location)
// -> Ich erlaube custom clients
// -> Security: PKCE, Asymmetrische Verschl�sseling (modern), Signiert
// .API, .Core, .Domain -> .API (!!WEAK!!)
// Wenn du zu weak bist -> https://github.com/keycloak/keycloak

// Frontend Service f�r: Account Settings
// -> Hat client in: JimmyAuthServer -> JimmyAccountSettingsClient (client_secret, client_name, ...) -> Internal Client Type -> User hat automatisch "authorize" accepted
// -> Wenn du "Account Settings" willst, kommt der JimmyAuthFlow f�r dein Client (user, pass, 2fa, alles macht der obere Service)
// -> Du hast "id_token" (user information), "access_token" (user access token f�r API action), "refresh_token" (token f�r neuer id und- access token)

// Game Service f�r: Game
// -> Hat client in: JimmyAuthServer -> JimmyAccountSettingsClient (client_token, client_name, ...) -> Internal Client Type -> User hat automatisch "authorize" accepted
// -> Wenn du "Account Settings" willst, kommt der JimmyAuthFlow f�r dein Client (user, pass, 2fa, alles macht der obere Service)
// -> Du hast "id_token" (user information), "access_token" (user access token f�r API action), "refresh_token" (token f�r neuer id und- access token)
// -> eigene DB f�r Kills/Deaths/XP

// Privacy: Du bist gezwungen einen Button zu haben wo du **alle** Daten von einem User in ein Humanes Format zur�ckgeben musst.

// -> Du kannst jedes OAuth2.0/OpenID Connect library usen wie auth-js aka next-auth (svelte, react, ...) und bist unabh�ngig