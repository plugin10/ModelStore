namespace ModelStore.API
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Goods
        {
            private const string Base = $"{ApiBase}/goods";

            public const string Create = Base;
        }
    }
}
