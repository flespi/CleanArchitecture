using CleanArchitecture.Domain.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CleanArchitecture.Infrastructure.Data.ValueConversion;

public class HexToBytesConverter : ValueConverter<Hex, byte[]>
{
    private static readonly ConverterMappingHints DefaultHints
        = new(size: 16, valueGeneratorFactory: (_, _) => new SequentialGuidValueGenerator());

    public HexToBytesConverter()
        : this(null)
    {
    }

    public HexToBytesConverter(ConverterMappingHints? mappingHints)
        : base(
            v => v,
            v => v,
            DefaultHints.With(mappingHints))
    {
    }

    public static ValueConverterInfo DefaultInfo { get; }
        = new(typeof(Guid), typeof(byte[]), i => new HexToBytesConverter(i.MappingHints), DefaultHints);
}
