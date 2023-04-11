namespace cv6
{
    public class BrowserAuthMiddleware
    {
        private RequestDelegate next;
        public BrowserAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext ctx)
        {
            
            string ua = ctx.Request.Headers["User-Agent"].ToString();
            if(ua.Contains("Chrome/") && !ua.Contains("Edg/"))
            {
                await next(ctx);
            }
            else
            {
                ctx.Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                ctx.Response.StatusCode = 403;
                await ctx.Response.WriteAsync("Pouzij chrome");
            }
        }
    }
}
