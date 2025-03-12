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
    }
    
}
