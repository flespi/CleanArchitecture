namespace CleanArchitecture.Application.Common.Types;

public readonly record struct Hex
{
    private readonly string _value;

    private Hex(string value) : this(Convert.FromHexString(value))
    {
    }

    private Hex(byte[] value)
    {
        _value = Convert.ToHexString(value);
    }

    public static implicit operator Hex(string value) => new(value);

    public static implicit operator Hex(byte[] value) => new(value);

    public static implicit operator Hex?(string? value) => value is null ? null : new(value);

    public static implicit operator Hex?(byte[]? value) => value is null ? null : new(value);

    public override string ToString() => _value;

    public byte[] ToByteArray() => Convert.FromHexString(_value);
}
