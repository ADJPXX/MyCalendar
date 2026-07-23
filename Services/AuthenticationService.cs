using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace MyCalendar.Services;

public class AuthenticationService
{
    public async Task<CalendarService> LoginAsync()
    {
        var credentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.json");

        await using var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read);
        
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets, 
            new[]{CalendarService.Scope.Calendar}, 
            "user", 
            CancellationToken.None,
            new FileDataStore("Token"));

        return new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "MyCalendar"
        });
    }
}