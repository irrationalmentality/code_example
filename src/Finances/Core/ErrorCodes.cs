namespace Code.Example.Finances;

public static class ErrorCodes
{
    public static readonly Error InsufficientFunds = new Error(-1000, "Insufficient funds");
    public static readonly Error AccountNotFound = new Error(-1001, "Account not found");
    public static readonly Error InvalidServerRamSize = new Error(-1002, "Server RAM must be a multiple of gigabyte and greater than zero");
    public static readonly Error InvalidServerCpuCount = new Error(-1003, "Server number of CPU must be greater than zero");
}
