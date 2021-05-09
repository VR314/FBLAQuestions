using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for EnterName.xaml
    /// </summary>
    public partial class EnterName : Page {
        /// <summary>
        /// The reference to the MainWindow and its variables
        /// </summary>
        private MainWindow mainWindow;

        public EnterName() {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        /// <summary>
        /// On click of the NameEnter button, save the username entered to the MainWindow and navigate to the Menu page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameEnter_Click(object sender, RoutedEventArgs e) {
            object nameBox = FindName("NameBox");

            string name;

            if (nameBox is TextBox box) {
                name = box.Text;

            } else {
                throw new System.InvalidOperationException("Error in reading text from NameBox");
            }

            mainWindow.UserName = name;
            if (name == "Name") {
                MessageBox.Show("Name cannot be empty!");
            } else {
                mainWindow.Navigate("Menu.xaml");
            }
        }

        /// <summary>
        /// On beginning of text entry, change color from Gray to Black
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            if (sender is TextBox box) {
                if (box.Foreground == Brushes.Gray) {
                    //If nothing has been entered yet.
                    box.Text = "";
                    box.Foreground = Brushes.Black;
                }
            } else {
                throw new System.InvalidOperationException("Cannot read NameBox correctly");
            }
        }
    }
}
