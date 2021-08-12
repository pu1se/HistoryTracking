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
    public class RelatedEntityPropertiesConfigBuilder<TMainEntity, TRelatedEntity> 
        where TRelatedEntity: class 
        where TMainEntity : class
    {
        private TrackedEntityConfigBuilder<TMainEntity> MainContext { get; }

        private RelatedEntityPropertiesConfig Config { get; }

        public RelatedEntityPropertiesConfigBuilder(TrackedEntityConfigBuilder<TMainEntity> mainContext)
        {
            MainContext = mainContext;
            Config = new RelatedEntityPropertiesConfig(typeof(TRelatedEntity));
        }

        public RelatedEntityPropertiesConfigBuilder<TMainEntity, TRelatedEntity> ShowOnUiChangesInRelatedProperty<TProperty>(
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

        public TrackedEntityConfigBuilder<TMainEntity> EndOfRelatedEntity()
        {
            MainContext.BuildConfiguration().RelatedEntities.Add(Config);
            return MainContext;
        }
    }
}
