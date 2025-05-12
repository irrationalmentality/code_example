using System.Globalization;

using CSharpFunctionalExtensions;

namespace Code.Example.Finances.Tariffs;

public sealed partial class RamSize : SimpleValueObject<int>
{
    private RamSize(int value)
        : base(value)
    {
    }

    public static RamSize Trusted(int value)
        => new(value);

    public static Result<RamSize, Error> TryCreate(int sizeMb)
    {
        return Result
            .SuccessIf(IsValidRamSize, new RamSize(sizeMb), ErrorCodes.InvalidServerRamSize);

        bool IsValidRamSize()
            => sizeMb > 0
               && sizeMb % 1024 == 0;
    }

    public decimal CalculateCost(decimal pricePerGb)
        => Value * pricePerGb / 1024;

    public override string ToString()
        => Value.ToString(CultureInfo.InvariantCulture);
}
