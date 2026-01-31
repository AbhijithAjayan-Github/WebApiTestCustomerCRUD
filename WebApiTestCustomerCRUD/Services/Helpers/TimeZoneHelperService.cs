namespace WebApiTestCustomerCRUD.Services.Helpers
{
    public class TimeZoneHelperService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<TimeZoneHelperService> logger; 
        public TimeZoneHelperService(ILogger<TimeZoneHelperService> logger,IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }
        public DateTime ConvertToIST(DateTime utcDateTime)
        {
           DateTime convertedTime = new DateTime();
           try
           {
                var timeZoneId = configuration["TimeZone"];
                logger.LogInformation($"TimeZone set in configuration {timeZoneId} ");
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                convertedTime= TimeZoneInfo.ConvertTimeFromUtc(utcDateTime,timeZoneInfo);
           }
           catch(Exception ex) {
                logger.LogError($"Error Converting time : {ex.Message}");
           }
            return convertedTime;
        }
    }
}
