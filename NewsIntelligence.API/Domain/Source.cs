namespace NewsIntelligente.API.Domain
{
    public class Source
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Article> Articles{ get; set; } = new();


        private Source() {}

        public Source(string name, string url, string category)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category");
            
            Id = Guid.NewGuid();
            Name = name;
            Url = url;
            Category = category;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}