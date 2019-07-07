using System;
using System.Linq;

namespace MarsRover
{
    public static class Helper
    {
        // A generic method for getting custom attributes of enum values
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
    where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
    }
}