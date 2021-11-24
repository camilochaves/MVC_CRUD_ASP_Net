namespace ReasonSystems.DLL.SwissKnife
{
    public static partial class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}