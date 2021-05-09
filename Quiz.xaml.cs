using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for Quiz.xaml
    /// </summary>
    public partial class Quiz : Page {

        private MainWindow mainWindow;
        private Question[] questions;
        private StackPanel stackPanel;
        public Quiz() {
            FontFamily = new FontFamily("Bahnschrift SemiLight SemiCondensed");
            FontSize = 18.5;
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
            stackPanel = (StackPanel)(FindName("MainStackPanel"));
            GenerateQuestions();
            foreach (Question q in questions) {
                Render(q);
            }

            Button submit = new();
            submit.Content = "SUBMIT";
            submit.Click += Submit_Click;
            submit.Margin = new Thickness(5, 1, 5, 5);
            submit.Height = 25;
            stackPanel.Children.Add(submit);
        }

        private void Submit_Click(object sender, RoutedEventArgs e) {
            bool c = true;

            foreach (Question q in questions) {
                if (q.Chosen == "") {
                    MessageBox.Show("No answer for #" + (q.Index + 1) + "!");
                    c = false;
                }
            }

            if (c) {
                mainWindow.Questions = questions;
                int correct = questions.Count(q => q.Correct);
                mainWindow.User.Correct += correct;
                mainWindow.User.Incorrect += 5 - correct;
                mainWindow.User.LastScore = (int)((correct / 5.0) * 100);
                mainWindow.User.Quizzes++;
                DatabaseConnection.UpdateUserData(mainWindow.User);
                mainWindow.Navigate("PrintPage.xaml");
            }
        }

        private void GenerateQuestions() {
            questions = new Question[5];
            int[] temp = Enumerable.Range(0, mainWindow.Options.Length).Where(i => mainWindow.Options[i]).ToArray();
            QuestionType[] options = new QuestionType[temp.Length];
            for (int i = 0; i < temp.Length; i++) {
                options[i] = (QuestionType)(temp[i]);
            }
            questions = DatabaseConnection.LoadFiveQuestions(options);
        }

        private void Render(Question question) {
            switch (question.Type) {
                case QuestionType.MC:
                    RenderMC(question);
                    break;
                case QuestionType.FillIn:
                    RenderFill(question);
                    break;
                case QuestionType.TF:
                    RenderTF(question);
                    break;
                case QuestionType.Dropdown:
                    RenderDropdown(question);
                    break;
                default:
                    throw new InvalidOperationException("Invalid question type");
            }
        }

        private void RenderMC(Question question) {
            Label label = new();
            label.Content = $"{question.Index + 1}.  {question.Text}";
            label.Margin = new Thickness(0, 2, 2, 1);
            StackPanel qpanel = new();
            qpanel.Children.Add(label);
            qpanel.Margin = new Thickness(2, 2, 2, 5);

            // Shuffle answer choices
            Random rand = new();
            string[] shuffledList = question.Answers.OrderBy(c => rand.Next()).ToArray();
            foreach (string ans in shuffledList) {
                RadioButton rb = new();
                rb.Content = ans;
                rb.Checked += Rb_Checked;
                rb.Tag = question;
                qpanel.Children.Add(rb);
            }

            stackPanel.Children.Add(qpanel);
        }

        private void Rb_Checked(object sender, System.Windows.RoutedEventArgs e) {
            string chosen = (string)((RadioButton)sender).Content;
            Question q = (Question)((RadioButton)sender).Tag;
            q.Chosen = chosen;
            //TODO: eliminate answers array, since everything is already saved in the Question objects
        }

        private void RenderFill(Question question) {
            Label label = new();
            label.Margin = new Thickness(0, 2, 2, 1);
            label.Content = $"{question.Index + 1}.  {question.Text}";
            StackPanel qpanel = new();
            qpanel.Children.Add(label);
            qpanel.Margin = new Thickness(2, 2, 2, 5);

            TextBox tb = new();
            tb.Tag = question;
            tb.TextChanged += Tb_TextChanged;
            qpanel.Children.Add(tb);

            stackPanel.Children.Add(qpanel);
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e) {
            string chosen = (string)((TextBox)sender).Text;
            Question q = (Question)((TextBox)sender).Tag;
            q.Chosen = chosen;
        }

        private void RenderTF(Question question) {
            Label label = new();
            label.Margin = new Thickness(0, 2, 2, 1);
            label.Content = $"{question.Index + 1}.  {question.Text}";
            StackPanel qpanel = new();
            qpanel.Children.Add(label);
            qpanel.Margin = new Thickness(2, 2, 2, 5);

            string[] tf = new string[] { "True", "False" };
            foreach (string ans in tf) {
                RadioButton rb = new();
                rb.Content = ans;
                rb.Checked += Rb_Checked;
                rb.Tag = question;
                qpanel.Children.Add(rb);
            }

            stackPanel.Children.Add(qpanel);
        }

        private void RenderDropdown(Question question) {
            Label label = new();
            label.Margin = new Thickness(0, 2, 2, 1);
            label.Content = $"{question.Index + 1}.  {question.Text}";
            StackPanel qpanel = new();
            qpanel.Children.Add(label);
            qpanel.Margin = new Thickness(2, 2, 2, 5);
            ComboBox cb = new();
            //TODO: add margins to RadioButtons and ComboBoxes
            cb.Tag = question;
            cb.SelectionChanged += Cb_SelectionChanged;

            // Shuffle answer choices
            Random rand = new();
            string[] shuffledList = question.Answers.OrderBy(c => rand.Next()).ToArray();
            foreach (string ans in shuffledList) {
                cb.Items.Add(ans);
            }
            qpanel.Children.Add(cb);

            stackPanel.Children.Add(qpanel);
        }

        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = (ComboBox)sender;
            string chosen = (string)(cb.SelectedItem);
            Question q = (Question)(cb.Tag);
            q.Chosen = chosen;
        }
    }
}
