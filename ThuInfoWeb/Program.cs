using ThuInfoWeb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.AllowTrailingCommas = true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = new PathString("/Home/Login");
        options.AccessDeniedPath = new PathString("/deny");
    });
builder.Services.AddSingleton<Data>(new Data(builder.Configuration.GetConnectionString("Test"), builder.Environment.IsDevelopment()));
builder.Services.AddScoped<UserManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.MapFallbackToFile("/", "index.html");
app.MapFallbackToFile("/about", "about.html");
app.MapFallbackToFile("/privacy", "privacy.html");
app.MapFallbackToFile("/privacy/zh", "privacyzh.html");
app.MapFallbackToFile("/privacy/en", "privacyen.html");
app.MapFallback("/deny", async r => await r.Response.WriteAsync("access denied"));

app.Run();
