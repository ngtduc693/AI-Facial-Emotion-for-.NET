using Example.gRPC;

namespace Example.WebApps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddGrpcClient<EmotionDetector.EmotionDetectorClient>(o =>
            {
                // add the gRPC server here, you can build it from Example.gRPC project in this solution
                // or you can create the container from the image from the docker hub by "docker pull ngtduc693/ai.facial.emotion-net10-grpc:lastest"
                o.Address = new Uri("http://localhost:5000/");
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
