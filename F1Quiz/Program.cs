using F1Quiz.Data;
using F1Quiz.Repositories;
using F1Quiz.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

//Functionality to add roles to database if they don't exist already
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };
    foreach(var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if(!roleExists)
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}

//Add admin role to my user if not already
async Task AssignAdminRole(IServiceProvider serviceProvider, string adminEmail)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var user = await userManager.FindByEmailAsync(adminEmail);
    if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
        await userManager.AddToRoleAsync(user, "Admin");
}


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QuizContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:QuizConnection"]);
});

//Add repositories for dependency injection
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddScoped<IScoreRepository, ScoreRepository>();
builder.Services.AddScoped<ScoreCalculation>();

//Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // !Require email confirmation
})
    .AddEntityFrameworkStores<QuizContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); //Necessary to use login etc.
builder.Services.AddTransient<IEmailSender, EmailSender>(); //Necessary to register user (send email)

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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Create roles
using(var scope = app.Services.CreateScope())
{ 
    var services = scope.ServiceProvider;
    await CreateRoles(services);
    await AssignAdminRole(services, "l.sjodsholm@gmail.com");
}

app.Run();
