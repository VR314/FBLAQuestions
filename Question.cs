namespace FBLAQuestions {
    /// <summary>
    /// The enum to represent the question type [MC/FillIn/TF/Dropdown]
    /// </summary>
    public enum QuestionType {
        MC, FillIn, TF, Dropdown
    }
 
    public class Question {
        /// <summary>
        /// The type of question
        /// </summary>
        public QuestionType Type { get; init; }

        /// <summary>
        /// The text of the question.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The answer choices to the question, in string format -- to be parsed on client side
        /// </summary>
        public string[] Answers { get; set; }

        /// <summary>
        /// The correct answer to the question
        /// </summary>
        public string CorrectAns { get; init; }

        public bool Correct { get; set; }

        public int Index { get; set; }

        public string Chosen {
            get { return chosen; }
            set {
                chosen = value;
                Correct = chosen == CorrectAns;
            }
        }

        private string chosen = "";

        public Question() {

        }

        /// <summary>
        /// The constructor for the Question class
        /// </summary>
        /// <param name="type">The type of question</param>
        /// <param name="ans">The array of answer choices... null if not applicable</param>
        /// <param name="correct">The correct answer</param>
        public Question(QuestionType type, string text, string[] ans, string correct) {
            Type = type;
            Answers = ans;
            Text = text;
            CorrectAns = correct;
        }

    }
}
