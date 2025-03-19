namespace BlazorAppWithCosmosDB.Data
{
    public class User
    {
        public Guid? id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Country { get; set; }

        public int? Age { get; set; }
    }
}