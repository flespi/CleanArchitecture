namespace CleanArchitecture.Application.Common.Types;

public readonly struct Hex
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

    public static bool operator ==(Hex left, Hex right) => string.Equals(left._value, right._value);

    public static bool operator !=(Hex left, Hex right) => !string.Equals(left._value, right._value);

    public override bool Equals(object? obj) => obj switch
    {
        Hex other => other == this,
        byte[] other => other == this,
        string other => other == this,
        _ => false,
    };

    public override int GetHashCode() => _value.GetHashCode();

    public override string ToString() => _value;

    public byte[] ToByteArray() => Convert.FromHexString(_value);
}
