namespace Infra.Utils;

public class ProgressStreamUtil(Stream baseStream) : Stream
{
    public long BytesRead { get; private set; }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var read = baseStream.Read(buffer, offset, count);
        BytesRead += read;
        return read;
    }
    
    public override bool CanRead => baseStream.CanRead;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotSupportedException();
    public override long Position { get => BytesRead; set => throw new NotSupportedException(); }
    public override void Flush() => baseStream.Flush();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}
