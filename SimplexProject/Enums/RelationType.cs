namespace SimplexProject.Enums
{
    internal enum RelationType
    {
        LessEqual,
        GreaterEqual,
        Equal,
    }

    internal static class RelationTypeExtensions
    {
        private static readonly Dictionary<RelationType, string> ToStringMap = new Dictionary<RelationType, string>
        {
            { RelationType.LessEqual, "<=" },
            { RelationType.GreaterEqual, ">=" },
            { RelationType.Equal, "=" }
        };

        private static readonly Dictionary<string, RelationType> FromStringMap = new Dictionary<string, RelationType>
        {
            { "<=", RelationType.LessEqual },
            { ">=", RelationType.GreaterEqual },
            { "=", RelationType.Equal },
        };

        public static string ToDescriptionString(this RelationType relationType)
        {
            return ToStringMap.TryGetValue(relationType, out string description) ? description : relationType.ToString();
        }

        public static RelationType FromDescriptionString(string description)
        {
            if (FromStringMap.TryGetValue(description, out RelationType relationType))
            {
                return relationType;
            }
            throw new ArgumentException($"No matching enum value for description '{description}'", nameof(description));
        }
    }
}
