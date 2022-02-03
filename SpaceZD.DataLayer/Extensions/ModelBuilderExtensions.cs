using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpaceZD.DataLayer.Extensions;

public static class ModelBuilderExtensions
{
    public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            entity.SetTableName(entity.DisplayName());
    }

    public static void MaxLengthOfAllStringsInTables(this ModelBuilder modelBuilder, int length)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            foreach (IMutableProperty property in entity.GetProperties())
                if (property.PropertyInfo?.PropertyType == typeof(string))
                    property.SetMaxLength(length);
    }

    public static void IsDeletedPropertyFalse(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            foreach (IMutableProperty property in entity.GetProperties())
                if (property.PropertyInfo?.Name == "IsDeleted")
                    property.SetDefaultValue(false);
    }

    public static void DisableCascadeDelete(this ModelBuilder modelBuilder)
    {
        foreach (IMutableForeignKey foreignKey in modelBuilder.Model
                                                              .GetEntityTypes()
                                                              .SelectMany(t => t.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
        }

    }

}