namespace cv6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // services
            builder.Services.AddScoped<ErrorHandlerService>();
            // builder.Services.AddScoped<ILogger, TxtLogger>();
            builder.Services.AddScoped<ILogger, JsonLogger>();


            var app = builder.Build();

            // app.MapGet("/", () => "Hello World!");

            app.UseMiddleware<ErrorMiddleware>();

            app.UseMiddleware<BrowserAuthMiddleware>();

            app.UseMiddleware<FormMiddleware>();

            app.UseMiddleware<FileMiddleware>();

            app.Use(async (HttpContext ctx, RequestDelegate next) => 
            {
                ctx.Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");

                await ctx.Response.WriteAsync(@"
                    <html>
                        <head>
                            <title>Titulek stránky</title>
                        </head>
                        <body>
                            <h1> Nadpis </h1>
                        </body>
                    </html>
                ");
            });

            
            app.Run();
        }
    }
}