﻿namespace BlazorAppWithCosmosDB
{
    public class CosmosDbOptions
    {
        public const string CosmosDb = "CosmosDB";

        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string ContainerName { get; set; } = string.Empty;
    }
}
