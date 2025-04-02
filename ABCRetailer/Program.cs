using ABCRetailer.Models;
using ABCRetailer.Models.Services;


namespace ABCRetailer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            string? connectionString = configuration["AzureTableStorage:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString) )
            {
                throw new InvalidOperationException("Azure Table Storage connection string is missing.");
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddSingleton(new AzureTableStorage(builder.Configuration["AzureTableStorage:ConnectString"]));
            //builder.Services.AddSingleton(new AzureBlobStorage(builder.Configuration["AzureStorage:BlobConnectionString"], "product-images"));

            //registration for azure table storage
            builder.Services.AddSingleton<AzureTableStorage>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration["AzureTableStorage:ConnectionString"];

                return new AzureTableStorage(connectionString);
            });

            //register azure blob storage
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Services.AddSingleton<AzureBlobStorage>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                string? ConnectionString = configuration["AzureStorage:BlobConnectionString"];
                string? containerName = configuration["AzureStorage:BlobContainerName"];//setting container name

                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                {
                    throw new InvalidOperationException("Azure Blob Storage connection string is missing or empty.");
                }

                return new AzureBlobStorage(connectionString, containerName);
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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<QueueStorage>();
            services.AddSingleton<FileStorage>();
        }
    }
}
