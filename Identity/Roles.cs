namespace Coffee_Ecommerce.API.Identity
{
    public static class Roles
    {
        public static string Owner { get; private set; } = "business_owner";
        public static string Administrator { get; private set; } = "commercial_admin";
        public static string Establishment { get; private set; } = "commercial_place";
        public static string User { get; private set; } = "customer";
        public static string SuspendedUser { get; private set; } = "suspended_account";
        public static string BlockedAccount { get; private set; } = "blocked_account";

        public static int RoleNumber(string role)
        {
            switch (role)
            {
                case "business_owner":
                    return 0;
                case "commercial_admin":
                    return 1;
                case "commercial_place":
                    return 2;
                case "customer":
                    return 3;
                default:
                    return -1;
            }
        }
    }
}
