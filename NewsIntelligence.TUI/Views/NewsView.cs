using Terminal.Gui;

namespace NewsIntelligence.TUI.Views
{
    public class NewsView : FrameView
    {
        private readonly TextView _newsTextView;

        public NewsView() : base("Main portal for news.")
        {
            X = 0;
            Y = 0;
            Width = Dim.Fill();
            Height = Dim.Fill();

            var asciiLogo = new Label("Logo")
            {
              Text = 
"""
  d8                                            
_d88__ 888-~88e  e88~~8e  Y88b    e    /  d88~\ 
 888   888  888 d888  88b  Y88b  d8b  /  C888   
 888   888  888 8888__888   Y888/Y88b/    Y88b  
 888   888  888 Y888    ,    Y8/  Y8/      888D 
 "88_/ 888  888  "88___/      Y    Y     \_88P  
                                                 
""",
                X = Pos.Center(), 
                Y = 0
            };

            _newsTextView = new TextView()
            {
                X = 0,
                Y = Pos.Bottom(asciiLogo) + 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                Text = """
                Welcome to tnews! this is the main portal for news... WIP
                """
            };
            
            this.Add(asciiLogo, _newsTextView);
            
        }
    }
}