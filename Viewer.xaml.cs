using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for Viewer.xaml
    /// </summary>
    public partial class Viewer : Page {
        /// <summary>
        /// The reference to the MainWindow and its variables
        /// </summary>
        private MainWindow mainWindow;
        public Viewer() {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
            // TODO: use threading? tiny but of lag when this is running OR make this global and run this in the background on startup and load this directly from global: partially solved by virtualization?
            // Dispatch expensive calls either within the UI thread with a lower DispatcherPriority by calling Dispatcher.BeginInvoke() or to a background thread by using a BackgroundWorker to keep the UI responsive. (https://www.wpftutorial.net/10PerformanceTips.html)
            var MCs = DatabaseConnection.LoadAllQuestions(new QuestionType[] { QuestionType.MC, QuestionType.Dropdown });
            BindingList<DisplayQuestion> MCbl = new();
            foreach (var MC in MCs) {
                DisplayQuestion dq = new(MC);
                MCbl.Add(dq);
            }
            gridMC.ItemsSource = MCbl;

            var TFs = DatabaseConnection.LoadAllQuestions(new QuestionType[] { QuestionType.TF, QuestionType.FillIn });
            BindingList<DisplayQuestion> TFbl = new();
            foreach (var TF in TFs) {
                DisplayQuestion dq = new(TF);
                TFbl.Add(dq);
            }
            gridFill.ItemsSource = TFbl;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) {
            mainWindow.Navigate("Menu.xaml");
        }
    }
}
