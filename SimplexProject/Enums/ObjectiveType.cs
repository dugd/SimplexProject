namespace SimplexProject.Enums
{
    internal enum ObjectiveType
    {
        Maximize,
        Minimize,
    }

    internal static class ObjectiveTypeExtensions
    {
        private static readonly Dictionary<ObjectiveType, string> ToStringMap = new Dictionary<ObjectiveType, string>
        {
            { ObjectiveType.Maximize, "max" },
            { ObjectiveType.Minimize, "min" }
        };

        private static readonly Dictionary<string, ObjectiveType> FromStringMap = new Dictionary<string, ObjectiveType>
        {
            { "max", ObjectiveType.Maximize },
            { "min", ObjectiveType.Minimize }
        };

        public static string ToDescriptionString(this ObjectiveType objectiveType)
        {
            return ToStringMap.TryGetValue(objectiveType, out string description) ? description : objectiveType.ToString();
        }

        public static ObjectiveType FromDescriptionString(string description)
        {
            if (FromStringMap.TryGetValue(description, out ObjectiveType objectiveType))
            {
                return objectiveType;
            }
            throw new ArgumentException($"No matching enum value for description '{description}'", nameof(description));
        }
    }
}
