namespace ModelStore.API
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Goods
        {
            private const string Base = $"{ApiBase}/goods";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id}}";
        }
    }
}
