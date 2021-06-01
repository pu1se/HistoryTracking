using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL.Enums;

namespace HistoryTracking.DAL.TrackEntityChangesLogic.PropertiesTrackingConfigurations
{
    public class TrackingPropertiesConfig<TEntity>
    {
        private TrackingEntityInfo EntityInfo { get; }

        public TrackingPropertiesConfig()
        {
            EntityInfo = new TrackingEntityInfo(GetEntityTableName(), typeof(TEntity));
        }

        public TrackingPropertiesConfig<TEntity> TrackProperty<TProperty>(
            Expression<Func<TEntity, TProperty>> func,
            UserType[] isVisibleForUserRoles, 
            Func<object, string> displayPropertyFuncExpression = null)
        {
            var expression = (MemberExpression)func.Body;
            var propertyName = expression.Member.Name;

            Func<object, string> displayPropertyFunc;
            if (displayPropertyFuncExpression == null)
            {
                var defaultDisplayingPropertyFunc = new Func<object, string>(property => property != null ? property.ToString() : string.Empty);
                displayPropertyFunc = defaultDisplayingPropertyFunc;
            }

            EntityInfo.PropertyList.Add(new TrackingPropertyInfo
            {
                Name = propertyName, 
                IsVisibleForUserRoles = isVisibleForUserRoles.ToList(),
                DisplayingPropertyFunction = displayPropertyFunc
            });
            return this;
        }

        private string GetEntityTableName()
        {
            var entityType = typeof(TEntity);
            var tableAttr = entityType.GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
            var entityTableName = tableAttr != null ? tableAttr.Name : entityType.Name;

            return entityTableName;
        }

        public TrackingEntityInfo BuildConfiguration()
        {
            return EntityInfo;
        }
    }
}
