using Lexicon_LMS_G1.Automapper;
using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<IBorderColorService, BorderColorService>();

builder.Services.AddTransient<ICourseRepository, CourseRepository>();
builder.Services.AddTransient<IModuleRepository, ModuleRepository>();
builder.Services.AddTransient<IBaseRepository<Course>, CourseRepository>();
builder.Services.AddTransient<IBaseRepository<Module>, ModuleRepository>();
builder.Services.AddTransient<IBaseRepository<Activity>, ActivityRepository>();
builder.Services.AddTransient<IBaseRepository<ActivityType>, ActivityTypeRepository>();
builder.Services.AddTransient<ICourseSelectListService, CourseSelectListService>();
builder.Services.AddTransient<IActivityTypeSelectListService, ActivityTypeSelectListService>();

builder.Services.AddAutoMapper(typeof(CourseProfile));
builder.Services.AddAutoMapper(typeof(ModuleProfile));
builder.Services.AddAutoMapper(typeof(ActivityProfile));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    ApplicationDbContext db = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        SeedData.InitAsync(db, services).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        //throw;
    }
}

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Courses}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
