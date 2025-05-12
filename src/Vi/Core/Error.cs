namespace Code.Example.Vi;

public record Error
{
    public int Code { get; private set; }
    public string Description { get; private set; }

    public Error(int code, string description)
    {
        Code = code;
        Description = description;
    }
}
