using Terminal.Gui;

namespace NewsIntelligence.TUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var top = Application.Top;

            var mainWindow = new Window("TNEWS - News Intelligence")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(mainWindow);

            Application.Run();
            Application.Shutdown();
        }
    }
}