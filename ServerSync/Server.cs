using System.Globalization;
using System.Threading;
using ServerSync.Exceptions;

namespace ServerSync;

public static class Server
{
    private static int _count = 0;

    private static readonly ReaderWriterLockSlim LockSlimObject = new ReaderWriterLockSlim();

    public static Task<int> GetCount()
    {
        LockSlimObject.EnterReadLock();
        try
        {
            return Task.FromResult(_count);
        }
        finally
        {
            LockSlimObject.ExitReadLock();
        }
    }

    public static Task AddToCount(int value, CancellationToken cancellationToken = default)
    {
        LockSlimObject.EnterWriteLock();
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            _count += value;
            return Task.CompletedTask;
        }
        catch (OverflowException)
        {
            throw new TooBigCounterException($"Too big counter value after adding {value} to counter {_count}");
        }
        finally
        {
            LockSlimObject.ExitWriteLock();
        }
    }
}