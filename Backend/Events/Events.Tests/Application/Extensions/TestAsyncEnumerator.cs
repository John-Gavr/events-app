namespace Events.Tests.Application.Extensions;

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

    public T Current => _inner.Current;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
