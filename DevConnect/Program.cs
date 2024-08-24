using DevConnect.Data;
using DevConnect.Hubs;
using DevConnect.Repository;
using DevConnect.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(x =>
{
    x.Limits.MaxRequestLineSize = 2*1024 * 1024;
    x.Limits.MaxRequestHeadersTotalSize =2*1024 * 1024;
    x.Limits.MaxRequestBodySize = 2 * 1024 * 1024;
    x.Limits.MaxRequestBufferSize = 2 * 1024 * 1024;
});

builder.Services.AddControllersWithViews();

string conStr = builder.Configuration.GetConnectionString("BaseDbContext")!;
builder.Services.AddDbContext<BaseDbContext>(x => x.UseSqlServer(conStr));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IUserIdProvider, UserIdService>();

builder.Services.AddHttpClient<SearchApiService>(client=>
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseSearchUrl"]!)
);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddRouting();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=Index}");

app.MapHub<ChatHub>("/chat");

app.Run();
