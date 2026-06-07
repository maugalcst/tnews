namespace NewsIntelligence.API.Domain
{
    public class Source
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Category { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public string XPathTitle { get; set; } = null!;
        public string XPathContent { get; set; } = null!;
        public string XPathContainer { get; set; } = null!;

        public List<Article> Articles{ get; set; } = new();


        private Source() {}

        public Source(string name, string url, string category, string xPathTitle, string xPathContent)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category");

            if (string.IsNullOrEmpty(xPathTitle))
                throw new ArgumentNullException("xPathTitle");
            
            if (string.IsNullOrEmpty(xPathContent))
                throw new ArgumentNullException("xPathContent");

            Id = Guid.NewGuid();
            Name = name;
            Url = url;
            Category = category;
            XPathTitle = xPathTitle;
            XPathContent = xPathContent;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}