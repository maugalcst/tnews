namespace NewsIntelligente.API.Domain
{
    public class ScrapingLog
    {
        public Guid Id { get; set; }
        public DateTimeOffset ExecutedAt { get; set; } = DateTimeOffset.UtcNow;
        public string Status { get; set; }
        public int DurationMs { get; set; }
        public int ArticlesFound { get; set; }
        public Guid SourceId { get; set; }

        private ScrapingLog() {}

        public ScrapingLog(string status, int durationMs, int articlesFound, Guid sourceId)
        {
            if (string.IsNullOrEmpty(status))
                throw new ArgumentNullException(nameof(status));
            
            if (durationMs < 0)
                throw new ArgumentOutOfRangeException("durationMs can't be a negative value.");
            
            if (articlesFound < 0)
                throw new ArgumentOutOfRangeException("ArticlesFound can't be a negative value.");
            
            Id = Guid.NewGuid();
            Status = status;
            DurationMs = durationMs;
            ArticlesFound = articlesFound;
            SourceId = sourceId;
        } 



    }
}