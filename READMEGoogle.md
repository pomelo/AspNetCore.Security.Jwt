## 2. In your Startup.cs

```C#
	using AspNetCore.Security.Jwt;
	using Swashbuckle.AspNetCore.Swagger;
```

```C#
        public void ConfigureServices(IServiceCollection services)
        {
            .
            .
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "XXX API", Version = "v1" });
            });

            var securitySettings = new SecuritySettings();
            this.Configuration.Bind("SecuritySettings", securitySettings);

            services.AddSecurity(securitySettings, true)
                    .AddGoogleSecurity();            

            services.AddMvc().AddGoogleSecurity();
        }
```

```C#
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            .
            .
            .
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "XXX API V1");
            });

            app.UseSecurity(true);

            app.UseMvc();
        }
```

## 3. In your appsettings.json

```javascript
{
  "SecuritySettings": {
    "SystemSettings": {
      .
      .
      "GoogleAuthSettings": {
        "TokenUrl": "https://accounts.google.com/o/oauth2/token"
      }
    },
    .
    .
    .
    "GoogleSecuritySettings": {
       "ClientId": "<client id>",
       "ClientSecret": "<client secret>",
       "RedirectUri": "http://localhost:59039/",
       "APIKey": "Your Google endpoint access key"
    }
  }
  .
  .
  .
}
```
Add your Google security settings. 

Do not change the **SystemSettings**.

The **GoogleSecuritySettings** are configurable.

## GoogleController

The out of the box **GoogleController** passes the Google access token to the client.

The **GoogleContoller** has a POST Method (/google) which you can call with a Google **Authorization Code**.
And an API Key for the endpoint, specified in GoogleSecuritySettings earlier.

The access token issued by Google is returned to the client.

Sample request to /google endpoint:

```javascript
{
  "authorizationCode": "4/qwBI9KdU2ZS-AyAow4iloIA16kfUHi_epeAwk6y1uiIWP2uVuOga9v2TOQBZ8qR7XRotKoHtX5CKxamUS19TNrs",
  "apiKey": "c1bba8a7-8a68-4697-82a6-33b4563ca895"
}
```

Sample response:

```javascript
{
  "access_token": "ya29.GltoBsrJ_RRZzI-DEzU3l6nDz_qwy7RFM-zFv7MA1z6ZeU3IijEZa_ECHG70V-cFz7omdplXraYVjTvrZkkYqdaf0Z8-vnQ6NiLeOXW3GLCqnlYjabwf59RMaUv8",
  "refresh_token": "1/PN_s59ZaV_pPDvYjM-EDeUlxg9OHTh8gzdAGmgsERrM",
  "token_type": "Bearer",
  "expires_in": 3600,
  "scope": "https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/adwords",
  "isAuthenticated": true
}
```

## Swagger UI integration

You will get a **Google endpoint (/google)** automatically in Swagger UI.

You can POST to this endpoint with a Google **Authorization Code** and endpoint API Key.

If authenticated by Google, you will get an Google access token back.

![Google Swagger UI integration](https://github.com/VeritasSoftware/AspNetCore.Security.Jwt/blob/master/GoogleSwaggerIntegration.jpg)