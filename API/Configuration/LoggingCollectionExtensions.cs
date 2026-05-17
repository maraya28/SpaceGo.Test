namespace API
{
    public static class LoggingCollectionExtensions
    {
        public static ILoggingBuilder AddLoggingSetup(this ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            return logging;
        }
    }
}