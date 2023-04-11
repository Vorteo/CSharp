using System.Net;

namespace cv6
{
    public class FormMiddleware
    {
        private RequestDelegate next;
        public FormMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            if(!ctx.Request.Path.StartsWithSegments("/form"))
            {
                await next(ctx);
                return;
            }


            string name = ctx.Request.Method == "POST" ? ctx.Request.Form["jmeno"].ToString() : null;

            if (name == null)
            {
                ctx.Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                await ctx.Response.WriteAsync(@"
                    <html>
                        <body> 
                            <form method=""post"">
                            Jmeno: <input name=""jmeno"" />
                            <button type="" submit="">Odeslat</button>
                            </form>
                        </body> 
                    </html>
                ");
            }
            else
            {
                await ctx.Response.WriteAsync(WebUtility.HtmlEncode(name));
            }
        }
    }
}
