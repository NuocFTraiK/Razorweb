using CS54.Models;
using CS54.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;

namespace CS54
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //For entity framework
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<MyBlogContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

			
			builder.Services.AddDefaultIdentity<AppUser>()
				.AddEntityFrameworkStores<MyBlogContext>() //Thiết lập nó làm việc vs csdl nào
				.AddDefaultTokenProviders();

			// Truy cập IdentityOptions
			builder.Services.Configure<IdentityOptions>(options => {
				// Thiết lập về Password
				options.Password.RequireDigit = false; // Không bắt phải có số
				options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
				options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
				options.Password.RequireUppercase = false; // Không bắt buộc chữ in
				options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
				options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

				// Cấu hình Lockout - khóa user
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
				options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
				options.Lockout.AllowedForNewUsers = true;

				// Cấu hình về User.
				options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = true;  // Email là duy nhất

				// Cấu hình đăng nhập.
				options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
				options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

			});


			//Add Email Configs
			var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
			builder.Services.AddSingleton(emailConfig);
			//builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddSingleton<IEmailSender, EmailService>();

			// Add services to the container.
			builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}