namespace cv6
{
    public class FileMiddleware
    {
        private RequestDelegate next;
        public FileMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            string path = ctx.Request.Path.ToString().TrimStart(new char[] { '/', '.'});

            string dir = "./";
            string fullPath = Path.GetFullPath(Path.Combine(dir, path));

            if(fullPath.EndsWith(".png"))
            {
                throw new NotImplementedException();
            }

            if(File.Exists(fullPath))
            {
                ctx.Response.Headers.Add("Content-Type", "image/jpeg");
                await ctx.Response.SendFileAsync(fullPath);
                // ctx.Response.Body
            }
            else
            {
                // ctx.Response.Headers.Add("Content-Type", "text/html");
                // await ctx.Response.WriteAsync("cesta: " + fullPath);
                await next(ctx);
            }

           
        }
    }
}
