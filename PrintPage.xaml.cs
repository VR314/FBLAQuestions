using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for PrintPage.xaml
    /// </summary>
    public partial class PrintPage : Page {
        private MainWindow mainWindow;
        public PrintPage() {
            InitializeComponent();

            mainWindow = (MainWindow)Application.Current.MainWindow;
            var title = new Paragraph();
            double correct = mainWindow.Questions.Count(q => q.Correct);
            title.Inlines.Add(new Run("Congrats, " + mainWindow.UserName + $", you scored {correct} / {5} = {(correct / 5) * 100}%!"));
            title.FontWeight = FontWeights.Bold;
            title.FontSize = 25;
            fd.Blocks.Add(title);
            Paragraph line = new();
            for (int i = 0; i < 5; i++) {
                line = new();
                line.FontSize = 5;
                fd.Blocks.Add(line);
                line = new();
                line.Foreground = Brushes.Black;
                Question q = mainWindow.Questions[i];

                if (q.Type == QuestionType.TF) {
                    if (q.Correct) {
                        // TODO: make ✓ and ✗ diff colors, general layout improvements (font?)
                        line.Inlines.Add("✓  " + q.Text);
                        fd.Blocks.Add(line);
                        foreach (string ans in new string[] { "True", "False" }) {
                            line = new();
                            line.Foreground = ans == q.Chosen ? Brushes.Green : Brushes.Black;
                            line.Inlines.Add(" -  " + ans);
                            fd.Blocks.Add(line);
                        }
                    } else {
                        line.Inlines.Add("✗  " + q.Text);
                        fd.Blocks.Add(line);
                        foreach (string ans in new string[] { "True", "False" }) {
                            line = new();
                            line.Foreground = ans == q.Chosen ? Brushes.Red : ((ans == q.CorrectAns) ? Brushes.Green : Brushes.Black);
                            line.Inlines.Add(" -  " + ans);
                            fd.Blocks.Add(line);
                        }
                    }
                } else if (q.Type == QuestionType.FillIn) {
                    if (q.Correct) {
                        line.Inlines.Add("✓  " + q.Text);
                        fd.Blocks.Add(line);
                        string ans = q.Chosen;
                        line = new();
                        line.Foreground = Brushes.Green;
                        line.Inlines.Add(" -  " + ans);
                        fd.Blocks.Add(line);
                    } else {
                        line.Inlines.Add("✗  " + q.Text);
                        fd.Blocks.Add(line);
                        string ans = q.Chosen;
                        line = new();
                        line.Foreground = Brushes.Red;
                        line.Inlines.Add(" -  " + ans);
                        fd.Blocks.Add(line);
                        ans = q.CorrectAns;
                        line = new();
                        line.Foreground = Brushes.Green;
                        line.Inlines.Add(" -  " + ans);
                        fd.Blocks.Add(line);
                    }

                } else if (q.Correct) {
                    line.Inlines.Add("✓  " + q.Text);
                    fd.Blocks.Add(line);
                    foreach (string ans in q.Answers) {
                        line = new();
                        line.Foreground = ans == q.Chosen ? Brushes.Green : Brushes.Black;
                        line.Inlines.Add(" -  " + ans);
                        fd.Blocks.Add(line);
                    }
                } else {
                    line.Inlines.Add("✗  " + q.Text);
                    fd.Blocks.Add(line);
                    foreach (string ans in q.Answers) {
                        line = new();
                        line.Foreground = ans == q.Chosen ? Brushes.Red : ((ans == q.CorrectAns) ? Brushes.Green : Brushes.Black);
                        line.Inlines.Add(" -  " + ans);
                        fd.Blocks.Add(line);
                    }
                }

            }
        }

        private void Print_Click(object sender, RoutedEventArgs e) {
            PrintDialog pd = new();
            if (pd.ShowDialog() != true) return;

            fd.PageHeight = pd.PrintableAreaHeight;
            fd.PageWidth = pd.PrintableAreaWidth;

            IDocumentPaginatorSource idocument = fd as IDocumentPaginatorSource;

            pd.PrintDocument(idocument.DocumentPaginator, "Printing Document...");
        }

        // TODO: change this to "Back to Main Menu" instead of being stuck back to QuizOptions OR add another button to go to main menu
        private void TakeAnother_Click(object sender, RoutedEventArgs e) {
            mainWindow.Navigate("QuizOptions.xaml");
        }
    }
}
