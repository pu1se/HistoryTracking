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
            params UserType[] isVisibleForUserRoles)
        {
            var expression = (MemberExpression)func.Body;
            var propertyName = expression.Member.Name;
            EntityInfo.PropertyList.Add(new TrackingPropertyInfo{ Name = propertyName, IsVisibleForUserRoles = isVisibleForUserRoles.ToList()});
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
