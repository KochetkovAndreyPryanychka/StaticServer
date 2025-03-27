using Nito.AsyncEx;
using ServerAsync.Exceptions;

namespace ServerAsync;

public static class Server
{
    private static int _count = 0;

    private static readonly AsyncReaderWriterLock Lock = new AsyncReaderWriterLock();

    public static async Task<int> GetCount()
    {
        using (await Lock.ReaderLockAsync())
        {
            return _count;
        }
    }

    public static async Task AddToCount(int value, CancellationToken cancellationToken = default)
    {
        using (await Lock.WriterLockAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                _count += value;
            }
            catch (OverflowException e)
            {
                throw new TooBigCounterException($"Too big counter value after adding {value} to counter {_count}");
            }
        }
    }
}