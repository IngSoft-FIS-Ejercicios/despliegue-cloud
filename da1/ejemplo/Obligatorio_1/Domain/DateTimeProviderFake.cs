namespace Domain;

public class DateTimeProviderFake : IDateTimeProvider
{
    private DateTime _dateTime;
    
    public DateTimeProviderFake(DateTime dateTime)
    {
        _dateTime = dateTime;
    }
    public DateTime GetCurrentDateTime()
    {
        return _dateTime;
    }
}