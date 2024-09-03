namespace SharedKernal;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}