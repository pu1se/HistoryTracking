using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;
using HistoryTracking.DAL.TrackEntityChangesLogic.Base.Configuration;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackedEntityConfigBuilder<TEntity> where TEntity: class
    {
        private TrackedEntityConfig EntityConfig { get; }

        public TrackedEntityConfigBuilder(bool showOnUiAsCategory)
        {
            EntityConfig = new TrackedEntityConfig(GetEntityTableName(), typeof(TEntity), showOnUiAsCategory);
        }

        public TrackedEntityConfigBuilder<TEntity> TrackProperty<TProperty>(
            Expression<Func<TEntity, TProperty>> func,
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

            if (EntityConfig.PropertyList.Select(x => x.Name).Contains(propertyName))
            {
                throw new Exception($"The description for property {propertyName} is already exists in Track Properties Configuration.");
            }

            EntityConfig.PropertyList.Add(new TrackedPropertyConfig
            {
                Name = propertyName, 
                IsVisibleForUserRoles = isVisibleForUserRoles.ToList(),
                DisplayingPropertyFunction = displayPropertyFunc
            });
            return this;
        }

        public DisplayRelatedEntityPropertiesConfigBuilder<TEntity, TProperty> DisplayRelatedEntity<TProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> func) where TProperty : class
        {
            var expression = (MemberExpression)func.Body;
            var propertyName = expression.Member.Name;

            return new DisplayRelatedEntityPropertiesConfigBuilder<TEntity, TProperty>(this);
        }

        public TrackedEntityConfig BuildConfiguration()
        {
            return EntityConfig;
        }

        public TrackedEntityConfigBuilder<TEntity> SaveRelatedEntityId(
            Expression<Func<TEntity, Guid>> func)
        {
            var expression = (MemberExpression)func.Body;
            var saveRelatedEntityIdPropertyName = expression.Member.Name;

            EntityConfig.SaveRelatedEntityIdPropertyName = saveRelatedEntityIdPropertyName;

            return this;
        }

        private string GetEntityTableName()
        {
            var entityType = typeof(TEntity);
            return entityType.Name;
        }

        public TrackedEntityConfigBuilder<TEntity> AlsoDisplayChangesInParentEntityWithId<TProperty>(Expression<Func<TEntity, TProperty>> func)
        {
            var expression = (MemberExpression)func.Body;
            var propertyName = expression.Member.Name;

            if (EntityConfig.PropertyList.Select(x => x.Name).Contains(propertyName))
            {
                throw new Exception($"The description for property {propertyName} is already exists in Track Properties Configuration.");
            }

            EntityConfig.PropertyList.Add(new TrackedPropertyConfig
            {
                Name = propertyName, 
                IsParentEntityId = true
            });

            return this;
        }
    }
}
