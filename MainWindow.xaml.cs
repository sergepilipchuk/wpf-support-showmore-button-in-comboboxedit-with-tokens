namespace MoreTokensApp {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
