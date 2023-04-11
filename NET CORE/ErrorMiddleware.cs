namespace cv6
{
    public class ErrorMiddleware
    {
        private RequestDelegate next;
        public ErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext ctx, ErrorHandlerService handler)
        {
            try
            {
                await next(ctx);
            }
            catch(Exception ex)
            {
                await handler.Handle(ex);

                await ctx.Response.WriteAsync("Doslo k chybe");
            }

        }
    }
}
