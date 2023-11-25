using ECommerce.DataAccess;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace ECommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration["ConnectionString"];

            builder.Services.AddSingleton<IOrderDetailsProvider, OrderDetailsProvider>();
            builder.Services.AddSingleton<IInventoryProvider>(new InventoryProvider(connectionString));
            builder.Services.AddSingleton<IProductProvider>(new ProductProvider(connectionString));
            builder.Services.AddSingleton<IInventoryUpdator>(new InventoryUpdator(connectionString));

            builder.Services.AddHttpClient("order", config =>
                config.BaseAddress = new System.Uri("https://localhost:5001/"));

            //builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
            //builder.Services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
            //        "inventory_exchange",
            //        ExchangeType.Topic));
            //builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
            //    "order_exchange",
            //    "order_response",
            //    "order.created",
            //    ExchangeType.Topic));

            // builder.Services.AddHostedService<OrderCreatedListener>();

            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Ecommerce service",
                    Version = "v1"
                });
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}