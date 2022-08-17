using System.Collections.Generic;

namespace CalorieCounter.Core.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
        }

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Organizasyonlar
        {
            public const string View = "Permissions.Organizasyonlar.View";
            public const string Create = "Permissions.Organizasyonlar.Create";
            public const string Edit = "Permissions.Organizasyonlar.Edit";
            public const string Delete = "Permissions.Organizasyonlar.Delete";
        }



    }
}