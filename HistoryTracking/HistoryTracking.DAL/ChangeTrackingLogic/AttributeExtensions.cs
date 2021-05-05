using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.ChangeTrackingLogic
{
    public static class AttributeExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo propertyOrClass) where TAttribute : class
        {
            var attribute = propertyOrClass.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
            return attribute;
        }

        public static bool HasTrackChangesAttribute(this PropertyInfo property)
        {
            return property.GetAttribute<TrackEntityChangesAttribute>() != null;
        }
    }
}
