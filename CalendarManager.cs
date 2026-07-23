using MyCalendar.Services;
using MyCalendar.Models;

namespace MyCalendar;

public class CalendarManager
{
    public async Task<List<CalendarEvent>> LoadTodayScheduleAsync()
    {
        AuthenticationService authentication = new();

        var calendar = await authentication.LoginAsync();

        GoogleCalendarService google = new(calendar);

        var events = await google.GetTodayEventsAsync();

        return events;
    }
}