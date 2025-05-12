namespace Code.Example.Vi;

public static class ErrorCodes
{
    public static readonly Error InvalidServerRamSize = new Error(-2000, "Server RAM must be a multiple of gigabyte and greater than zero");
    public static readonly Error InvalidServerCpuCount = new Error(-2001, "Server number of CPU must be greater than zero");
}
