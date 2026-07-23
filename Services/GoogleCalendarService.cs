using Google.Apis.Calendar.v3;
using MyCalendar.Models;

namespace MyCalendar.Services;

public class GoogleCalendarService
{
    private readonly CalendarService _calendarService;

    public GoogleCalendarService(CalendarService calendarService)
    {
        _calendarService = calendarService;
    }


    public async Task<List<CalendarEvent>> GetTodayEventsAsync()
    {
        var today = DateTimeOffset.Now.Date;
        
        var startOfDay = new DateTimeOffset(today, DateTimeOffset.Now.Offset);

        var endOfDay = startOfDay.AddDays(1);

        var request = _calendarService.Events.List("primary");

        request.TimeMinDateTimeOffset = startOfDay;

        request.TimeMaxDateTimeOffset = endOfDay;

        request.SingleEvents = true;

        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var response = await request.ExecuteAsync();

        List<CalendarEvent> events = [];

        foreach (var item in response.Items)
        {
            events.Add(new CalendarEvent
            {
                Id = item.Id,
                Title = item.Summary ?? "Sem título",
                Start = item.Start.DateTimeDateTimeOffset,
                End = item.End.DateTimeDateTimeOffset,
                IsAllDay = item.Start.Date != null
            });
        }

        return events;
    }
}