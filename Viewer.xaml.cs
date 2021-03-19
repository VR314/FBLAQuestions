using System.ComponentModel;
using System.Windows.Controls;
 
namespace FBLAQuestions {
    /// <summary>
    /// Interaction logic for Viewer.xaml
    /// </summary>
    public partial class Viewer : Page {
        public Viewer() {
            InitializeComponent();
            var MCs = DatabaseConnection.LoadAllQuestions(new QuestionType[] { QuestionType.MC, QuestionType.Dropdown });
            BindingList<DisplayQuestion> MCbl = new BindingList<DisplayQuestion>();
            foreach (var MC in MCs) {
                DisplayQuestion dq = new(MC);
                MCbl.Add(dq);
            }
            gridMC.ItemsSource = MCbl;

            var TFs = DatabaseConnection.LoadAllQuestions(new QuestionType[] { QuestionType.TF, QuestionType.FillIn });
            BindingList<DisplayQuestion> TFbl = new BindingList<DisplayQuestion>();
            foreach (var TF in TFs) {
                DisplayQuestion dq = new(TF);
                TFbl.Add(dq);
            }
            gridFill.ItemsSource = TFbl;
        }
    }
}
