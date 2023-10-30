namespace GlobalErrorApp.Configurations
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
