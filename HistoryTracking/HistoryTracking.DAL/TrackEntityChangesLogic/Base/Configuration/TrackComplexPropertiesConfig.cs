using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration
{
    public class TrackComplexPropertiesConfig<TMainEntity, TComplexProperty> 
        where TComplexProperty: class 
        where TMainEntity : class
    {
        private TrackPropertiesConfig<TMainEntity> MainContext { get; }
        private Type ComplexPropertyType { get; }
        private string ComplexPropertyName { get; }
        private List<TrackingPropertyInfo> SubPropertyList { get; } = new List<TrackingPropertyInfo>();

        public TrackComplexPropertiesConfig(TrackPropertiesConfig<TMainEntity> mainContext, string complexPropertyName)
        {
            MainContext = mainContext;
            ComplexPropertyType = typeof(TComplexProperty);
            ComplexPropertyName = complexPropertyName;
        }

        public TrackComplexPropertiesConfig<TMainEntity, TComplexProperty> TrackProperty<TProperty>(
            Expression<Func<TComplexProperty, TProperty>> func,
            UserType[] isVisibleForUserRoles, 
            Func<object, string> displayPropertyFunc = null)
        {
            var expression = (MemberExpression)func.Body;
            var propertyName = expression.Member.Name;

            if (displayPropertyFunc == null)
            {
                var defaultDisplayingPropertyFunc = new Func<object, string>(property => property != null ? property.ToString() : string.Empty);
                displayPropertyFunc = defaultDisplayingPropertyFunc;
            }

            SubPropertyList.Add(new TrackingPropertyInfo
            {
                Name = propertyName,
                IsVisibleForUserRoles = isVisibleForUserRoles.ToList(),
                DisplayingPropertyFunction = displayPropertyFunc
            });
            return this;
        }

        public TrackPropertiesConfig<TMainEntity> EndOfComplexProperty()
        {
            MainContext.EntityInfo.PropertyList.Add(new TrackingPropertyInfo
            {
                Name = ComplexPropertyName,
                SubProperties = SubPropertyList
            });
            return MainContext;
        }
    }
}
