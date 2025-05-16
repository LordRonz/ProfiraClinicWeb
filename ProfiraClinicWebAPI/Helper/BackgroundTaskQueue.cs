using System.Threading.Channels;

namespace ProfiraClinicWebAPI.Helper
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue =
            Channel.CreateUnbounded<Func<CancellationToken, Task>>();

        public async ValueTask EnqueueAsync(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null) throw new ArgumentNullException(nameof(workItem));
            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct) =>
            await _queue.Reader.ReadAsync(ct);
    }
}
