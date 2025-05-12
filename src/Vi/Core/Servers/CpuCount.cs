using System.Globalization;
using CSharpFunctionalExtensions;

namespace Code.Example.Vi.Servers;

public sealed partial class CpuCount : SimpleValueObject<int>
{
    private CpuCount(int value)
        : base(value)
    {
    }

    public static CpuCount Trusted(int value)
        => new(value);

    public static Result<CpuCount, Error> TryCreate(int value)
    {
        return Result
            .SuccessIf(value > 0, new CpuCount(value), ErrorCodes.InvalidServerCpuCount);
    }

    public override string ToString()
        => Value.ToString(CultureInfo.InvariantCulture);
}
