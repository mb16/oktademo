using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Okta.AspNetCore;
using oktaMVC.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
})
.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOktaMvc(new OktaMvcOptions
{
    // Replace these values with your Okta configuration
    OktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain"),
    AuthorizationServerId = builder.Configuration.GetValue<string>("Okta:AuthorizationServerId"),
    ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
    ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
    Scope = new List<string> { "openid", "profile", "email", "groups" },
    GetClaimsFromUserInfoEndpoint = true,
    OpenIdConnectEvents = new OpenIdConnectEvents()
    {

        OnTokenValidated = async context =>
            {

                if (context.HttpContext.RequestServices != null && context.Principal != null)
                {
                    using var scope = context.HttpContext.RequestServices.CreateScope();
                    context.Principal = await scope.ServiceProvider.GetRequiredService<IClaimsTransformation>().TransformAsync(context.Principal);
                }
            }
    }
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
