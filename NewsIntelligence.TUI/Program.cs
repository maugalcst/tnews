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

            // ascii logo
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
                Y = Pos.Center()
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
            contentFrame.Add(asciiLogo);

            // bottom panel (keybindings n stuff)
            var statusBar = new StatusBar(new StatusItem[] {
                new StatusItem(Key.Q | Key.CtrlMask, "~CTRL-Q~ Salir", () => { Application.RequestStop(); }),
                new StatusItem(Key.Enter, "~ENTER~ Leer", () => {}),
                new StatusItem(Key.Space, "~ESPACIO~ Activar/Desactivar", () => {})
            });

            top.Add(mainWindow, statusBar);

            Application.Run();
            Application.Shutdown();
        }
    }
}