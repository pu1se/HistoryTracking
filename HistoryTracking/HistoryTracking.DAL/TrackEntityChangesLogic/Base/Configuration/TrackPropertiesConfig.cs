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
    public class TrackPropertiesConfig<TEntity> where TEntity: class
    {
        public TrackingEntityInfo EntityInfo { get; }

        public TrackPropertiesConfig()
        {
            EntityInfo = new TrackingEntityInfo(GetEntityTableName(), typeof(TEntity));
        }

        public TrackPropertiesConfig<TEntity> TrackProperty<TProperty>(
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

            if (EntityInfo.PropertyList.Select(x => x.Name).Contains(propertyName))
            {
                throw new Exception($"The description for property {propertyName} is already exists in TrackPropertiesConfig");
            }

            EntityInfo.PropertyList.Add(new TrackingPropertyInfo
            {
                Name = propertyName, 
                IsVisibleForUserRoles = isVisibleForUserRoles.ToList(),
                DisplayingPropertyFunction = displayPropertyFunc
            });
            return this;
        }

        public TrackingEntityInfo BuildConfiguration()
        {
            return EntityInfo;
        }

        public TrackComplexPropertiesConfig<TEntity, TProperty> TrackComplexProperty<TProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> func) where TProperty : class
        {
            var expression = (MemberExpression)func.Body;
            var propertyName = expression.Member.Name;

            return new TrackComplexPropertiesConfig<TEntity, TProperty>(this, propertyName);
        }

        private string GetEntityTableName()
        {
            var entityType = typeof(TEntity);
            var tableAttr = entityType.GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
            var entityTableName = tableAttr != null ? tableAttr.Name : entityType.Name;

            return entityTableName;
        }
    }
}
