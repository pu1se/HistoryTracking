using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration
{
    public class DisplayRelatedEntityPropertiesConfigBuilder<TMainEntity, TRelatedEntity> 
        where TRelatedEntity: class 
        where TMainEntity : class
    {
        private TrackedEntityConfigBuilder<TMainEntity> MainContext { get; }

        private DisplayRelatedEntityPropertiesConfig Config { get; }

        public DisplayRelatedEntityPropertiesConfigBuilder(TrackedEntityConfigBuilder<TMainEntity> mainContext)
        {
            MainContext = mainContext;
            Config = new DisplayRelatedEntityPropertiesConfig(typeof(TRelatedEntity));
        }

        public DisplayRelatedEntityPropertiesConfigBuilder<TMainEntity, TRelatedEntity> TrackRelatedProperty<TProperty>(
            Expression<Func<TRelatedEntity, TProperty>> func,
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

            Config.PropertyList.Add(new TrackedPropertyConfig
            {
                Name = propertyName,
                IsVisibleForUserRoles = isVisibleForUserRoles.ToList(),
                DisplayingPropertyFunction = displayPropertyFunc
            });
            return this;
        }

        public TrackedEntityConfigBuilder<TMainEntity> EndOfComplexProperty()
        {
            MainContext.BuildConfiguration().RelatedEntities.Add(Config);
            return MainContext;
        }
    }
}
