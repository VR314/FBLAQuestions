namespace FBLAQuestions {
    class DisplayQuestion : Question {
        public string AnswerDisplay { get; set; }

        public DisplayQuestion(Question q) {
            this.Text = q.Text;
            this.Answers = q.Answers;
            this.CorrectAns = q.CorrectAns;
            string ans = " - " + Answers[0];
            for (int i = 1; i < Answers.Length - 1; i++) {
                ans += "\n - " + Answers[i];
            }
            this.AnswerDisplay = ans;
        }

    }
}
 