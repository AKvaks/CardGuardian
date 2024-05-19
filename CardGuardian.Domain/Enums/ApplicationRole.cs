namespace CardGuardian.Domain.Enums
{
    public enum ApplicationRoles
    {
        Admin,
        Moderator,
        User
    }

    public static class ApplicationRolesHelpers
    {
        public static string GetApplicationRoleName(int role)
        {
            switch (role)
            {
                case 0:
                    return "Admin";
                case 1:
                    return "Moderator";
                case 2:
                    return "User";
                default:
                    return "User";
            }
        }

        public static List<string> GetApplicationRoles()
        {
            return [.. Enum.GetNames<ApplicationRoles>()];
        }
    }
}
