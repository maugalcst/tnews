using Terminal.Gui;

namespace NewsIntelligence.TUI.Views
{
    public class LogsView : FrameView
    {
        private readonly TextView _logsTextView;

        public LogsView() : base("System logs history.")
        {
            X = 0;
            Y = 0;
            Width = Dim.Fill();
            Height = Dim.Fill();

            _logsTextView = new TextView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                Text = """
                [13:00:00] 📡 Sistema TNEWS Inicializado.
                [13:00:02] 🔌 Conectando con API en puerto 5134...
                [13:00:03] ✅ Conexión exitosa. Base de datos lista.
                [13:00:05] 🤖 Llama 3 en espera de comandos.
                """
            };
            
            this.Add(_logsTextView);
        }
    }
}