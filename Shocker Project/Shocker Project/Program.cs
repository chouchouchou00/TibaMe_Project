using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Data;
using Shocker_Project.Models;


namespace Shocker_Project
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the DI container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			var db_a98a02_thm101team1001ConnectionString = builder.Configuration.GetConnectionString("db_a98a02_thm101team1001") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<db_a98a02_thm101team1001Context>(options =>
				options.UseSqlServer(db_a98a02_thm101team1001ConnectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

               builder.Services.AddEndpointsApiExplorer();

               builder.Services.AddSwaggerGen();

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

               if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

			app.UseHttpsRedirection();
			app.UseStaticFiles();


			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
						{
							endpoints.MapControllerRoute(
							  name: "areas",
							  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
							);
						});

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

               //app.MapOrdersEndpoints();

			app.Run();
		}
	}
}