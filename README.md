# HistoryTracking
Track all changes of entities on save via Entity Framework. Display changes using a config. Here is UI https://history-tracking.azurewebsites.net/users
![image](https://user-images.githubusercontent.com/5277614/196773373-e3aa7867-e957-4bfd-9f8d-f99513f4ab07.png)

Here is config
```
var allUserRoles = EnumHelper.ToArray<UserType>();
            ConfigList = new List<TrackedEntityConfig>
            {
                TrackEntityChangesFor<UserEntity>(showOnUiAsCategory: true)
                    .ShowOnUiChangesInProperty(x => x.Name, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.Email, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.UserType, allUserRoles, type => type?.ToString().SplitByCaps())
                    .MergeDisplayingOfRelatedEntity(x => x.Addresses)
                        .ShowOnUiChangesInRelatedProperty(x => x.HouseAddress, allUserRoles)
                        .ShowOnUiChangesInRelatedProperty(x => x.City, allUserRoles)
                        .EndOfRelatedEntity()
                    .MergeDisplayingOfRelatedEntity(x => x.Contacts)
                        .ShowOnUiChangesInRelatedProperty(x => x.PhoneNumber, allUserRoles)
                        .ShowOnUiChangesInRelatedProperty(x => x.Email, allUserRoles)
                        .EndOfRelatedEntity()
                    .BuildConfiguration(),

                TrackEntityChangesFor<UserAddressEntity>(showOnUiAsCategory: false)
                    .SaveRelatedEntityId(x => x.UserId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<UserContactEntity>(showOnUiAsCategory: false)
                    .SaveRelatedEntityId(x => x.UserId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<SubscriptionProductEntity>(showOnUiAsCategory: true)
                    .ShowOnUiChangesInProperty(x => x.Title, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.Price, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.Currency, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.DistributorMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor})
                    .ShowOnUiChangesInProperty(x => x.ResellerMarkupAsPercent, new [] {UserType.SystemUser, UserType.Distributor, UserType.Reseller})
                    .AlsoDisplayChangesInParentEntityWithId(x => x.ParentId)
                    .BuildConfiguration(),

                TrackEntityChangesFor<OrderEntity>(showOnUiAsCategory: true)
                    .ShowOnUiChangesInProperty(x => x.Comments, allUserRoles)
                    .ShowOnUiChangesInProperty(x => x.OrderStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .ShowOnUiChangesInProperty(x => x.PaymentStatus, allUserRoles, type => type?.ToString().SplitByCaps())
                    .BuildConfiguration(),
            };
```            
