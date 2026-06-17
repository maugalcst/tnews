using Terminal.Gui;

namespace NewsIntelligence.TUI.Views
{
    public class SourcesView : FrameView
    {
        private readonly ListView _sourcesListView;

        public SourcesView() : base("Selecciona tus fuentes de noticias")
        {
            X = 0;
            Y = 0;
            Width = Dim.Fill();
            Height = Dim.Fill();

            List<string> mockSources = new()
            {
                "Wired",
                "The Verge",
                "TechCrunch",
                "Hacker News"
            };

            _sourcesListView = new ListView(mockSources)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                AllowsMarking = true
            };

            this.Add(_sourcesListView);
        }
    }
}