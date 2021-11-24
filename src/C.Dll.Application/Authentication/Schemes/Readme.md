# ChallengeAsync
  
  This will instruct your browser on where to go to be authenticated. For example:
  
  Cookies will redirect you to your own login page (e.g. /Account/Login)
  Azure AD will redirect you to the Microsoft login page
  etc..

# AuthenticateAsync
  
  This step handles whatever information comes from the authentication page (where you were redirected to by the Challenge step), and uses it to create a ClaimsPrincipal instance that identify the logged in user.

 That ClaimsPrincipal is then assigned to HttpContext.User.

# SignInAsync
  
  This step takes the ClaimsPrincipal built from the previous step, and persists it. The most common way is of course the cookies.
  
  Note that based on the source code in https://github.com/aspnet/Security/, it seems to be the only way to persist the ClaimsPrincipal.
  
# SignOutAsync
  
  This is the reverse step of the SignIn step. It instructs the middleware to delete any persisted data.
  
  Cookies will delete the stored cookie
  Azure AD will redirect you to their Microsoft logout page
  etc..