using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for QuizOptions.xaml
    /// </summary>
    public partial class QuizOptions : Page {
        /// <summary>
        /// The reference to the MainWindow and its variables
        /// </summary>
        private MainWindow mainWindow;

        public QuizOptions() {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e) {
            object mcChoice = FindName("MCBox");
            object fillChoice = FindName("FillInBox");
            object tfChoice = FindName("TFBox");
            object dropChoice = FindName("DropdownBox");

            if (mcChoice is CheckBox mcBox && fillChoice is CheckBox fillBox && tfChoice is CheckBox tfBox && dropChoice is CheckBox dropBox) {
                mainWindow.Options[0] = (bool)mcBox.IsChecked;
                mainWindow.Options[1] = (bool)fillBox.IsChecked;
                mainWindow.Options[2] = (bool)tfBox.IsChecked;
                mainWindow.Options[3] = (bool)dropBox.IsChecked;
            } else {
                throw new System.InvalidOperationException("CheckBoxes not retreived correctly");
            }

            if (mainWindow.Options.Contains(true)) {
                mainWindow.Navigate("Quiz.xaml");
            } else {
                MessageBox.Show("Choose at least one type of question!");
            }
        }
    }
}
