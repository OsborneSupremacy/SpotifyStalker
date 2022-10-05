namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IDateTimeProvider))]
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetCurrentDateTime() => DateTime.Now;
}
