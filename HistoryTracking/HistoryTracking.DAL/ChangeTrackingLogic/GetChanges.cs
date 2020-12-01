﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking.DAL.ChangeTrackingLogic
{
    public static class GetChanges
    {
        public static List<ChangeItem> For<T>(T oldEntity, T newEntity) where T: class
        {
            var changeList = new List<ChangeItem>();
            if (oldEntity == null || newEntity == null)
            {
                return changeList;
            }

            var propertyList = oldEntity.GetType().GetProperties();
            foreach (var property in propertyList)
            {
                if (!property.HasTrackChangesAttribute())
                {
                    continue;
                }

                if (property.GetValue(oldEntity) == property.GetValue(newEntity))
                {
                    continue;
                }

                if (property.GetValue(oldEntity)?.ToString() == property.GetValue(newEntity)?.ToString())
                {
                    continue;
                }

                var change = new ChangeItem
                {
                    OldValue = property.GetValue(oldEntity)?.ToString(),
                    NewValue = property.GetValue(newEntity)?.ToString(),
                };
                change.OldValue = change.OldValue ?? string.Empty;
                change.NewValue = change.NewValue ?? string.Empty;
                changeList.Add(change);
            }

            return changeList;
        }
    }
}