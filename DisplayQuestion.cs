namespace FBLAQuestions {
    internal class DisplayQuestion : Question {
        public string AnswerDisplay { get; set; }

        public DisplayQuestion(Question q) {
            Text = q.Text;
            Answers = q.Answers;
            CorrectAns = q.CorrectAns;
            string ans = " - " + Answers[0];
            for (int i = 1; i < Answers.Length - 1; i++) {
                ans += "\n - " + Answers[i];
            }
            AnswerDisplay = ans;
        }

    }
}
