namespace Business.Helpers
{
    /// <summary>
    /// Pure rules for normalizing a post body and category before persistence.
    /// </summary>
    public static class PostHelper
    {
        /// <summary>Maximum body length kept as-is without truncation.</summary>
        public const int MaxBodyLengthWithoutTruncation = 20;

        /// <summary>Maximum prefix length when truncating; the suffix adds 3 characters (max stored length 100).</summary>
        public const int MaxTruncatedPrefixLength = 97;

        private const string TruncationSuffix = "...";

        public static string NormalizeBody(string body)
        {
            if (body == null) return null;
            if (body.Length <= MaxBodyLengthWithoutTruncation) return body;
            var prefixLength = body.Length >= MaxTruncatedPrefixLength ? MaxTruncatedPrefixLength : body.Length;
            var prefix = body.Substring(0, prefixLength);
            return prefix + TruncationSuffix;
        }

        public static string ResolveCategory(int type, string requested)
        {
            switch (type)
            {
                case 1:
                    return "Farándula";
                case 2:
                    return "Política";
                case 3:
                    return "Futbol";
                default:
                    return requested ?? string.Empty;
            }
        }
    }
}
