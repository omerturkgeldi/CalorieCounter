namespace CalorieCounter.Core.Constants
{
    public static class Roles
    {

        // Her türlü kullanıcı oluşturma yetkisine sahiptir. Uygulama, sadece 1 SuperAdmin rolüne sahip kullancıyıla başlar.
        public const string SUPERADMIN = "SuperAdmin";
        public const string ADMIN = "Admin";
        public const string DIETITIAN = "Dietitian";
        public const string USER = "User";

        public static readonly string[] All = new string[] { SUPERADMIN , ADMIN, DIETITIAN, USER };
    }


}