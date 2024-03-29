using Bookify.Web;
using Bookify.Web.Core.Mapper;
using Bookify.Web.Helpers;
using Bookify.Web.Seed;
using Bookify.Web.Tasks;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Dashboard;
using HashidsNet;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;
using Serilog.Context;
using System.Reflection;
using System.Security.Claims;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultUI()
	.AddDefaultTokenProviders();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();
builder.Services.AddSingleton<IHashids>(_ => new Hashids());
builder.Services.AddDataProtection( ).SetApplicationName(nameof(Bookify.Web));
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IViewToHTMLService, ViewToHTMLService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IEmailBodyBuilder, EmailBodyBuilder>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Add Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();
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
	option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

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

app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });


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
});
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
var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
var emailBodyBuilder = scope.ServiceProvider.GetRequiredService<IEmailBodyBuilder>();
var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
var webHost = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
HangfireTasks hangfireTasks= new HangfireTasks(dbContext, emailBodyBuilder,emailSender,webHost);

RecurringJob.AddOrUpdate(() => hangfireTasks.PrepareExpirationAlert(), "0 14 * * *");
RecurringJob.AddOrUpdate(() => hangfireTasks.PrepareRentalEnd(), "0 14 * * *");


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
