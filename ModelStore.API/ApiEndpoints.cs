namespace ModelStore.API
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Products
        {
            private const string Base = $"{ApiBase}/products";

            public const string Create = Base;
            public const string Get = $"{Base}/{{idOrSlug}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Users
        {
            private const string Base = $"{ApiBase}/users";

            public const string Create = Base;
            public const string Get = $"{Base}/login";
        }

        public static class Orders
        {
            private const string Base = $"{ApiBase}/orders";

            public const string GetAll = Base;
            public const string Create = Base;
        }
    }
}