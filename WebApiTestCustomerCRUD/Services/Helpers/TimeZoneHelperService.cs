namespace WebApiTestCustomerCRUD.Services.Helpers
{
    public class TimeZoneHelperService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<TimeZoneHelperService> logger;
        public TimeZoneHelperService(ILogger<TimeZoneHelperService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }
        public DateTime ConvertToIST(DateTime utcDateTime)
        {
            var timeZoneId = configuration["TimeZone"];
            if (string.IsNullOrWhiteSpace(timeZoneId)) { 
                logger.LogError("TimeZone configuration is missing."); 
                throw new InvalidOperationException("TimeZone configuration is missing."); 
            }
            logger.LogInformation($"TimeZone set in configuration {timeZoneId} ");
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
        }
    }
}
