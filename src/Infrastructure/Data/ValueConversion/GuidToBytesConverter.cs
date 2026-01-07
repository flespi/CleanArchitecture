using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CleanArchitecture.Infrastructure.Data.ValueConversion;

public class GuidToBytesConverter : ValueConverter<Guid, byte[]>
{
    private static readonly ConverterMappingHints DefaultHints
        = new(size: 16, valueGeneratorFactory: (_, _) => new SequentialGuidValueGenerator());

    public GuidToBytesConverter()
        : this(null)
    {
    }

    public GuidToBytesConverter(ConverterMappingHints? mappingHints)
        : base(
            v => v.ToByteArray(true),
            v => new Guid(v, true),
            DefaultHints.With(mappingHints))
    {
    }

    public static ValueConverterInfo DefaultInfo { get; }
        = new(typeof(Guid), typeof(byte[]), i => new GuidToBytesConverter(i.MappingHints), DefaultHints);
}
