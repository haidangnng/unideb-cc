namespace QuestionLair.Web.Services.Materials;
public class ProgressStream : Stream
{
    private readonly Stream _inner;
    private readonly Action<long> _progress;
    private long _totalRead;

    public ProgressStream(Stream inner, Action<long> progress)
    {
        _inner = inner;
        _progress = progress;
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        int bytesRead = await _inner.ReadAsync(buffer, offset, count, cancellationToken);
        _totalRead += bytesRead;
        _progress(_totalRead);
        return bytesRead;
    }

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        int bytesRead = await _inner.ReadAsync(buffer, cancellationToken);
        _totalRead += bytesRead;
        _progress(_totalRead);
        return bytesRead;
    }

    // Completely block synchronous reads
    public override int Read(byte[] buffer, int offset, int count)
        => throw new NotSupportedException("Synchronous reads are not supported in Blazor.");

    public override bool CanRead => _inner.CanRead;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => _inner.Length;

    public override long Position
    {
        get => _inner.Position;
        set => throw new NotSupportedException();
    }

    public override void Flush() => _inner.Flush();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}

