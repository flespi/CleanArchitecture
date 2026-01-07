namespace CleanArchitecture.Domain.Types;

public readonly struct Hex
{
    private byte[] ByteArray { get; }

    private Hex(byte[] value) => ByteArray = value;


    private static Hex FromByteArray(byte[] value)
        => new(value);

    private static Hex FromString(string value)
        => new(Convert.FromHexString(value));


    public static implicit operator Hex(string value) => FromString(value);

    public static implicit operator Hex(byte[] value) => FromByteArray(value);

    public static implicit operator Hex?(string? value) => value is not null ? FromString(value) : (Hex?)null;

    public static implicit operator Hex?(byte[]? value) => value is not null ? FromByteArray(value) : (Hex?)null;


    public static implicit operator string(Hex value) => value.ToString();

    public static implicit operator byte[](Hex value) => value.ByteArray;

    public static implicit operator string?(Hex? value) => value.HasValue ? value.Value.ToString() : null;

    public static implicit operator byte[]?(Hex? value) => value.HasValue ? value.Value.ByteArray : null;


    public static bool operator ==(Hex left, Hex right) => Enumerable.SequenceEqual(left.ByteArray, right.ByteArray);

    public static bool operator !=(Hex left, Hex right) => !Enumerable.SequenceEqual(left.ByteArray, right.ByteArray);


    public override bool Equals(object? obj) => obj switch
    {
        Hex other => other == this,
        byte[] other => (Hex)other == this,
        string other => (Hex)other == this,
        _ => false,
    };

    public override int GetHashCode() => ByteArray.GetHashCode();

    public override string ToString() => Convert.ToHexString(ByteArray);
}
