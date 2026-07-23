namespace MyCalendar;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var calendarManager = new CalendarManager();

        var resultado = await calendarManager.LoadTodayScheduleAsync();

        var today = DateTime.Today;

        Console.Clear();
        
        Console.WriteLine($"OLÁ! SEGUE OS COMPROMISSOS DO DIA DE HOJE ({today:d})\n");

        if (resultado.Count == 0)
        {
            Console.WriteLine("Nenhum compromisso hoje.");
        }
        
        foreach (var evento in resultado)
        {
            if (evento.IsAllDay)
            {
                Console.WriteLine($"Dia inteiro - {evento.Title}");
            }
            else
            {
                Console.WriteLine($"{evento.Start:HH:mm} : {evento.End:HH:mm} - {evento.Title}");
            }
        }

        Console.WriteLine("\n\nAperte QUALQUER TECLA para fechar o programa.");
        Console.ReadKey();
    }
}