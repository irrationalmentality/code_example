namespace Code.Example.Finances.Tariffs;

public static class ServerTariff // This is a stub
{
    public static decimal CalculateCost(CpuCount cpuCount, RamSize ramSize)
    {
        var result = cpuCount.CalculateCost(5);
        result += ramSize.CalculateCost(7);

        return result;
    }
}
