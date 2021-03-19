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

            if (mcChoice is CheckBox && fillChoice is CheckBox && tfChoice is CheckBox && dropChoice is CheckBox) {
                mainWindow.Options[0] = (bool)((CheckBox)mcChoice).IsChecked;
                mainWindow.Options[1] = (bool)((CheckBox)fillChoice).IsChecked;
                mainWindow.Options[2] = (bool)((CheckBox)tfChoice).IsChecked;
                mainWindow.Options[3] = (bool)((CheckBox)dropChoice).IsChecked;
            } else {
                throw new System.InvalidOperationException("CheckBoxes not retreived correctly");
            }

            mainWindow.Navigate("Quiz.xaml");
        }
    }
}
