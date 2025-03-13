namespace GoodMoodPerfumeBot.UserRoles
{
    public static class SharedData
    {
        public static class UserRoles
        {
            public const string Creator = "Creator";
            public const string Owner = "Owner";
            public const string Administrator = "Administrator";
            public const string Member = "Member";
            public const string Restricted = "Restricted";
            public const string Banned = "Banned";

            public static IReadOnlyList<string> AllRoles
            {
                get => new List<string>()
            {
                Owner, Administrator, Member, Restricted, Banned
            };
            }
        }

        public static class PayStatus
        {
            public const string Payed = "Payed";
            public const string NotPayed = "NotPayed";
            public static IReadOnlyList<string> AllStatuses
            {
                get => new List<string>()
                {
                    Payed, NotPayed
                };
            }
        }

        public static class OrderStatus
        {
            public const string NotShipped = "NotShipped";
            public const string Shipped = "Shipped";

            public static IReadOnlyList<string> AllStatuses
            {
                get => new List<string>()
                {
                    Shipped, NotShipped
                };
            }

        }
    }
    
}
