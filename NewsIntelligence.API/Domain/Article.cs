namespace NewsIntelligente.API.Domain
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
        public Source Source { get; private set; }
        public Guid SourceId { get; private set; }
        public DateTimeOffset ScrapedDate { get; private set; }

        private Article() {}

        public Article(string title, string author, string content, string url, string category, DateTimeOffset publishedDate, Source source, Guid sourceId)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            if (string.IsNullOrEmpty(author))
                throw new ArgumentNullException("author");
            
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");
            
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category");

            Id = Guid.NewGuid();
            Title = title;
            Author = author;
            Content = content;
            Url = url;
            Category = category;
            PublishedDate = publishedDate;
            Source = source;
            SourceId = sourceId;
            ScrapedDate = DateTimeOffset.UtcNow;
        }

    }
}