namespace Spotly.Core.Constants;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Editor = "Editor";
    public const string Scanner = "Scanner";
    public const string Finance = "Finance";
    
    public static readonly string[] All = [Admin, Manager, Editor, Scanner, Finance];
}
