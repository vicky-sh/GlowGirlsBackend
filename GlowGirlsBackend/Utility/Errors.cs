namespace GlowGirlsBackend.Utility;

public static class Errors
{
    public static Error Validation(string message)
    {
        return new Error(1000, message);
    }

    public static Error NotFound(string message)
    {
        return new Error(2000, message);
    }

    public static Error Unauthorized(string message)
    {
        return new Error(3000, message);
    }
}