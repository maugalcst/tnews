using NewsIntelligence.TUI.Views;
using Terminal.Gui;

namespace NewsIntelligence.TUI
{
    class Program
    {
        static void Main(string[] args)
        {

            Application.Init();
            var top = Application.Top;

            var mainWindow = new Window("TNEWS - Inteligencia de Noticias")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1 
            };

            // left panel
            var menuFrame = new FrameView("Menu")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(20),
                Height = Dim.Fill()
            };

            // main panel
            var contentFrame = new FrameView("Main")
            {
                X = Pos.Right(menuFrame),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 4
            };

            // stats panel (below main frame)
            var statsFrame = new FrameView("Stats")
            {
                X = Pos.Right(menuFrame),
                Y = Pos.Bottom(contentFrame),
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            mainWindow.Add(menuFrame, contentFrame, statsFrame);

            // bottom panel (keybindings n stuff)
            var statusBar = new StatusBar(new StatusItem[] {
                new StatusItem(Key.Q | Key.CtrlMask, "~CTRL-Q~ Salir", () => { Application.RequestStop(); }),
                new StatusItem(Key.Enter, "~ENTER~ Leer", () => {}),
                new StatusItem(Key.Space, "~ESPACIO~ Activar/Desactivar", () => {})
            });

            top.Add(mainWindow, statusBar);

            // left menu views
            var logsView = new LogsView();
            var sourcesView = new SourcesView();
            var newsView = new NewsView();

            List<string> leftMenuOptionsList = new()
            {
                "NEWS",
                "SOURCES",
                "LOGS"
            };

            ListView myListView = new ListView(leftMenuOptionsList)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            menuFrame.Add(myListView);

            myListView.SelectedItemChanged += (args) =>
            {
                contentFrame.Title = leftMenuOptionsList[args.Item];

                switch (args.Item)
                {
                    case 0: // News
                        contentFrame.RemoveAll();
                        contentFrame.Add(newsView);
                        break;
                    
                    case 1: // Sources
                        contentFrame.RemoveAll();
                        contentFrame.Add(sourcesView);
                        break;
                    
                    case 2: // Logs
                        contentFrame.RemoveAll();
                        contentFrame.Add(logsView);
                        break;
                }

                contentFrame.SetNeedsDisplay();
            };

            Application.Run();
            Application.Shutdown();
        }
    }
}