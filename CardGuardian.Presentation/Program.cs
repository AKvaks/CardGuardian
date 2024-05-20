using CardGuardian.Domain.Entities;
using CardGuardian.Domain.Enums;
using CardGuardian.Infrastructure.Persistance;
using CardGuardian.Presentation.Components;
using CardGuardian.Presentation.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext != null)
        {
            dbContext.Database.SetCommandTimeout(300);
            dbContext.Database.Migrate();

            await SeedCountries(dbContext).ConfigureAwait(false);
            await SeedRoles(roleManager).ConfigureAwait(false);
            await SeedAdministrator(userManager, dbContext).ConfigureAwait(false);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}

app.Run();

static async Task SeedCountries(ApplicationDbContext _dbContext)
{
    if (await _dbContext.Countries.AnyAsync().ConfigureAwait(false) == false)
    {
        List<Country> countries = new List<Country>()
        {
            new() { CountryName = "Austria", CountryCode = "AT" },
            new() { CountryName = "Belgium", CountryCode = "BE" },
            new() { CountryName = "Bulgaria", CountryCode = "BG" },
            new() { CountryName = "Croatia", CountryCode = "HR" },
            new() { CountryName = "Cyprus", CountryCode = "CY" },
            new() { CountryName = "Czech Republic", CountryCode = "CZ" },
            new() { CountryName = "Denmark", CountryCode = "DK" },
            new() { CountryName = "Estonia", CountryCode = "EE" },
            new() { CountryName = "Finland", CountryCode = "FI" },
            new() { CountryName = "France", CountryCode = "FR" },
            new() { CountryName = "Germany", CountryCode = "DE" },
            new() { CountryName = "Greece", CountryCode = "GR" },
            new() { CountryName = "Hungary", CountryCode = "HU" },
            new() { CountryName = "Ireland, Republic of(EIRE)", CountryCode = "IE" },
            new() { CountryName = "Italy", CountryCode = "IT" },
            new() { CountryName = "Latvia", CountryCode = "LV" },
            new() { CountryName = "Lithuania", CountryCode = "LT" },
            new() { CountryName = "Luxembourg", CountryCode = "LU" },
            new() { CountryName = "Malta", CountryCode = "MT" },
            new() { CountryName = "Netherlands", CountryCode = "NL" },
            new() { CountryName = "Poland", CountryCode = "PL" },
            new() { CountryName = "Portugal", CountryCode = "PT" },
            new() { CountryName = "Romania", CountryCode = "RO" },
            new() { CountryName = "Slovakia", CountryCode = "SK" },
            new() { CountryName = "Slovenia", CountryCode = "SI" },
            new() { CountryName = "Spain", CountryCode = "ES" },
            new() { CountryName = "Sweden", CountryCode = "SE" },
            new() { CountryName = "United Kingdom", CountryCode = "GB" }
        };

        await _dbContext.Countries.AddRangeAsync(countries);
        await _dbContext.SaveChangesAsync();
    }
}

static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
{
    if (await roleManager.Roles.AnyAsync().ConfigureAwait(false) == false)
    {
        foreach (var item in ApplicationRolesHelpers.GetApplicationRoles())
        {
            if (await roleManager.FindByNameAsync(item).ConfigureAwait(false) == null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = item, NormalizedName = item.ToUpper() }).ConfigureAwait(false);
            }
        }
    }
}

static async Task SeedAdministrator(UserManager<ApplicationUser> userManager, ApplicationDbContext _dbContext)
{
    const string email = "admin@cardguardian.com";
    if (await userManager.FindByEmailAsync(email).ConfigureAwait(false) == null)
    {
        const string userName = "admin";
        const string firstName = "AdminCG";
        const string lastName = "AdminCG";
        const string password = "AdminCgTest1234!";

        int countryId = 0;
        if (await _dbContext.Countries.AnyAsync(x => x.CountryCode == "HR"))
        {
            var countryCro = await _dbContext.Countries.FirstOrDefaultAsync(x => x.CountryCode == "HR").ConfigureAwait(false);
            var country = await _dbContext.Countries.FirstAsync();
            countryId = countryCro != null ? countryCro.Id : country.Id;
        }
        else
        {
            var country = await _dbContext.Countries.FirstAsync();
            countryId = country.Id;
        }

        var appAdmin = new ApplicationUser
        {
            Email = email,
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            CountryOfResidenceId = countryId,
            CreatedAt = DateTime.Now,
            LastModifiedAt = DateTime.Now,
            DeletedAt = null,
        };

        var result = await userManager.CreateAsync(appAdmin, password).ConfigureAwait(false);
        if (result.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(appAdmin).ConfigureAwait(false);
            await userManager.ConfirmEmailAsync(appAdmin, token).ConfigureAwait(false);
            await userManager.AddToRoleAsync(appAdmin, ApplicationRoles.Admin.ToString().ToUpper()).ConfigureAwait(false);
        }
    }
}