using System;
using System.Windows;

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {
        public string UserName { get; set; }

        public User User { get; set; }

        /// <summary>
        /// Array of options as determined in QuizOptions: 
        /// [0] = MC
        /// [1] = Fill-In
        /// [2] = T/F
        /// [3] = Dropdown
        /// </summary>
        public bool[] Options { get; set; }
        //TODO: turn this into a Dictionary to more clearly access elements -- causes problems in GenerateQuestions() method

        public Question[] Questions { get; set; }

        public MainWindow() {
            InitializeComponent();
            Navigate("EnterName.xaml");
            Options = new bool[4];
            Questions = new Question[5];
        }

        /// <summary>
        /// Navigate to the XAML path given.
        /// </summary>
        /// <param name="path"> The XAML file name to navigate to</param>
        public void Navigate(string path) {
            MainFrame.Navigate(new Uri(path, UriKind.Relative));
        }
    }
}
