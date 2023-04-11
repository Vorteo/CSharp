using System.Text;

namespace cv6
{
    public class ErrorHandlerService
    {
        private ILogger logger;
        public ErrorHandlerService(TxtLogger logger)
        {
            this.logger = logger;
        }

        public async Task Handle(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine();

            await logger.Log(sb.ToString());
        }
    }
}
