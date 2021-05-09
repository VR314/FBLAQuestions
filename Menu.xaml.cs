using System.Windows;
using System.Windows.Controls;

//TODO: refactor all XAML to follow a more consistent order
//TODO: include "leaderboard" or quiz history for a user 
//          - user "insights?", suggest "you have a 40% average with --- questions, I suggest studying more from [source URL]

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page {

        /// <summary>
        /// The reference to the MainWindow and its variables
        /// </summary>
        private MainWindow mainWindow;

        /// <summary>
        /// Constructor for the Menu page
        /// </summary>
        public Menu() {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
            WriteWelcomeText();
        }

        /// <summary>
        /// Reads name from MainWindow and writes a welcome message
        /// </summary>
        private void WriteWelcomeText() {
            object findWelcomeText = FindName("WelcomeText");
            object findStatsText = FindName("StatsText");
            if (findWelcomeText is Label welcomeLabel && findStatsText is Label statsLabel) {
                User u = DatabaseConnection.GetUserData(mainWindow.UserName);
                if (u != null) {
                    mainWindow.User = u;
                    welcomeLabel.Content = $"Welcome back, {mainWindow.UserName}";
                    statsLabel.Content = $"Your last quiz score was a {u.LastScore}%, and your lifetime average is {u.Percent}% over {u.Quizzes} quizzes";
                } else {
                    welcomeLabel.Content = $"Welcome, {mainWindow.UserName}";
                    mainWindow.User = DatabaseConnection.AddUser(mainWindow.UserName);
                    statsLabel.Content = "Take your first quiz!";
                }
            } else {
                throw new System.InvalidOperationException("Error in reading Name");
            }
        }

        /// <summary>
        /// On click of the QuizButton, navigate to the QuizOptions page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuizButton_Click(object sender, RoutedEventArgs e) {
            mainWindow.Navigate("QuizOptions.xaml");
        }

        private void QViewButton_Click(object sender, RoutedEventArgs e) {
            mainWindow.Navigate("Viewer.xaml");
        }
    }
}
