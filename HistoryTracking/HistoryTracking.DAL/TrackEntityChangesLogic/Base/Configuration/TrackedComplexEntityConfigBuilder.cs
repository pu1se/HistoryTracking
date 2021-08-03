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
    public class TrackedComplexEntityConfigBuilder<TMainEntity, TComplexProperty> 
        where TComplexProperty: class 
        where TMainEntity : class
    {
        private TrackedEntityConfigBuilder<TMainEntity> MainContext { get; }
        private string ComplexPropertyName { get; }
        private List<TrackedPropertyConfig> SubPropertyList { get; } = new List<TrackedPropertyConfig>();

        public TrackedComplexEntityConfigBuilder(TrackedEntityConfigBuilder<TMainEntity> mainContext, string complexPropertyName)
        {
            MainContext = mainContext;
            ComplexPropertyName = complexPropertyName;
        }

        public TrackedComplexEntityConfigBuilder<TMainEntity, TComplexProperty> TrackProperty<TProperty>(
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

            SubPropertyList.Add(new TrackedPropertyConfig
            {
                Name = propertyName,
                IsVisibleForUserRoles = isVisibleForUserRoles.ToList(),
                DisplayingPropertyFunction = displayPropertyFunc
            });
            return this;
        }

        public TrackedEntityConfigBuilder<TMainEntity> EndOfComplexProperty()
        {
            MainContext.BuildConfiguration().PropertyList.Add(new TrackedPropertyConfig
            {
                Name = ComplexPropertyName,
                SubProperties = SubPropertyList
            });
            return MainContext;
        }
    }
}
