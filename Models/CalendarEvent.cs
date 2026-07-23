namespace MyCalendar.Models;

public class CalendarEvent
{
    public string Id { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public DateTimeOffset? Start { get; set; }
    
    public DateTimeOffset? End { get; set; }
    
    public bool IsAllDay { get; set; }
}