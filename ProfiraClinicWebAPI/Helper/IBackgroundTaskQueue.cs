namespace ProfiraClinicWebAPI.Helper
{
    public interface IBackgroundTaskQueue
    {
        ValueTask EnqueueAsync(Func<CancellationToken, Task> workItem);
        ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
