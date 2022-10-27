namespace MetroBus.Extensions;

public static class Check
{
    public static void IsNullOrEmpty(string value, string variableName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(variableName);
        }
    }
    
    public static void IsNull(object value, string variableName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(variableName);
        }
    }
}