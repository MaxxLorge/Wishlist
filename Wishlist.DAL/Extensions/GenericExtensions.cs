using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Wishlist.DAL.Extensions;

public static class GenericExtensions
{
    public static string DisplaySeparatedByNewLines<T>(this T item)
    where T: notnull
    {
        var itemType = item.GetType();

        var properties = itemType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (x.GetCustomAttribute<DisplayAttribute>(), x.GetValue(item)))
            .Where(x => x.Item1 != null);

        var formattedProp = properties
            .Select(x => $"{x.Item1!.Name}: {x.Item2}");

        return string.Join(Environment.NewLine, formattedProp);
    }
}