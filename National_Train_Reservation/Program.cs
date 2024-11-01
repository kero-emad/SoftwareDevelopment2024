using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using National_Train_Reservation.Data;
using Microsoft.AspNetCore.Identity;
public class Program {
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Cookie.Name = "National_Train_Reservation.Cookie";
            options.LoginPath = "/Users/SignIn";
            options.LogoutPath = "/Usees/SignOut";
           
        });
        builder.Services.AddDbContext<ApplicationDBcontext>
            (Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("myConnection"));
        });

        builder.Services.AddSession(op =>
        {
            op.Cookie.Name = "session";
            op.IdleTimeout = TimeSpan.FromMinutes(60);
            op.Cookie.HttpOnly = true;
            op.Cookie.IsEssential = true;

        });

    

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
        app.UseSession();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
