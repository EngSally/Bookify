using BookTest.Core.Mapper;
using BookTest.Seed;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BookTest.Data;
using Microsoft.Data.SqlClient;
using BookTest.Helpers;
using Microsoft.Extensions.Options;
using BookTest.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.DataProtection;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.IdentityModel.Tokens;
using BookTest.Tasks;
using HashidsNet;
using Serilog;
using CloudinaryDotNet;
using DocumentFormat.OpenXml.InkML;
using Serilog.Context;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();
builder.Services.AddSingleton<IHashids>(_=>new Hashids());
builder.Services.AddDataProtection().SetApplicationName(nameof(BookTest));
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IViewToHTMLService, ViewToHTMLService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IEmailBodyBuilder, EmailBodyBuilder>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.Configure<AuthorizationOptions>
    (Options =>
Options.AddPolicy("AdminOnly", policy =>
{
    policy.RequireAuthenticatedUser();
    policy.RequireRole(AppRole.Admin);

}));


builder.Services.AddExpressiveAnnotations();
builder.Services.AddMvc(option =>
{
    option.Filters.Add( new  AutoValidateAntiforgeryTokenAttribute());
});
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseExceptionHandler("/Home/Error");

//app.UseStatusCodePages(async statusCodeContext =>
//{
//    // using static System.Net.Mime.MediaTypeNames;
//    statusCodeContext.HttpContext.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Plain;

//    await statusCodeContext.HttpContext.Response.WriteAsync(
//        $"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
//});

//app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions {Secure=CookieSecurePolicy.Always });


app.Use(async (context, next) => 
{
    context.Response.Headers.Add("X-Frame-Options", "Deny");
    await next();
});
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/HangFire", new DashboardOptions
{
    DashboardTitle = "Bookify-Dashboard",
    IsReadOnlyFunc = (DashboardContext context) => true,
    Authorization = new IDashboardAuthorizationFilter[]
    {
        new HangFireAuthorizationFilter("AdminOnly")
    }
}) ;
//Add Serilog
app.Use(async (context, next) =>
{
	LogContext.PushProperty("UserId", context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
	LogContext.PushProperty("UserName", context.User.FindFirst(ClaimTypes.Name)?.Value);

	await next();
});
app.UseSerilogRequestLogging();
var scopeFactory=app.Services.GetRequiredService<IServiceScopeFactory>();
using  var scope = scopeFactory.CreateScope();
var roleManger=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManger=scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
await DefaultRoles.SeedRoles(roleManger);
await DefaultUser.SeedAdmin(userManger);
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var emailBodyBuilder = scope.ServiceProvider.GetRequiredService<IEmailBodyBuilder>();
var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
var webHost = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
HangfireTasks hangfireTasks= new HangfireTasks(dbContext, emailBodyBuilder,emailSender,webHost);

RecurringJob.AddOrUpdate( () =>  hangfireTasks.PrepareExpirationAlert(), "0 14 * * *");
RecurringJob.AddOrUpdate(() => hangfireTasks.PrepareRentalEnd(), "0 14 * * *");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
