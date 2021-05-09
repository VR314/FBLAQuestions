namespace FBLAQuestions {
    public class User {

        public string Name { get; set; }

        public int Correct { get; set; }

        public int Incorrect { get; set; }

        public int Quizzes { get; set; }

        public int LastScore { get; set; }

        public double Percent {
            get {
                return (int)(Correct / (double)(Correct + Incorrect) * 100);
            }
        }


        public User() {

        }
    }
}
