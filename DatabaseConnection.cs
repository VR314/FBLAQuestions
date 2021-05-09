using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace FBLAQuestions {
    internal class DatabaseConnection {
        private static string LoadConnectionString(string id = "Default") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        /// <summary>
        /// Loads a random selection of five questions from the question bank with the given types.
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public static Question[] LoadFiveQuestions(QuestionType[] qt) {
            using (var cnn = new SQLiteConnection(LoadConnectionString())) {
                cnn.Open();
                var command = cnn.CreateCommand();
                List<Question> questions = new();
                foreach (var q in qt) {
                    command.CommandText =
                $@"
                SELECT *
                FROM Questions
                WHERE Type = {(int)q}
                ";
                    Debug.WriteLine(command.CommandText);

                    using (var reader = command.ExecuteReader()) {

                        while (reader.Read()) {
                            var type = reader.GetInt32(1);
                            var text = reader.GetString(2);
                            var answers = reader.GetString(3);
                            string[] ansArray = answers.Split(',');
                            string correct = reader.GetString(4);

                            questions.Add(new Question { Type = (QuestionType)type, Text = text, Answers = ansArray, CorrectAns = correct });
                        }
                    }
                }

                // TODO: optimize Quiz generation
                var temp = questions.OrderBy(x => new Random().Next()).ToArray()[0..5];
                while (!ValidArrangement(qt, temp)) {
                    temp = questions.OrderBy(x => new Random().Next()).ToArray()[0..5];
                }
                for (int i = 0; i < temp.Length; i++) {
                    temp[i].Index = i;
                }
                return temp;
            }
        }

        /// <summary>
        /// Checks the validity of a quiz arrangement
        /// </summary>
        /// <param name="qt"></param>
        /// <param name="questions"></param>
        /// <returns></returns>
        private static bool ValidArrangement(QuestionType[] qt, Question[] questions) {
            bool contains = true;
            foreach (QuestionType qtt in qt) {
                if (contains)
                    contains = questions.Any(q => q.Type == qtt);
            }
            return contains;
        }


        /// <summary>
        /// Loads all questions of a given question type, used in the Viewer page
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public static BindingList<Question> LoadAllQuestions(QuestionType[] qt) {
            using (var cnn = new SQLiteConnection(LoadConnectionString())) {
                cnn.Open();
                var command = cnn.CreateCommand();
                List<Question> questions = new();
                foreach (var q in qt) {
                    command.CommandText =
                $@"
                SELECT *
                FROM Questions
                WHERE Type = {(int)q}
                ";
                    Debug.WriteLine(command.CommandText);

                    using (var reader = command.ExecuteReader()) {

                        while (reader.Read()) {
                            var type = reader.GetInt32(1);
                            var text = reader.GetString(2);
                            var answers = reader.GetString(3);
                            string[] ansArray = answers.Split(',');
                            string correct = reader.GetString(4);

                            questions.Add(new Question { Type = (QuestionType)type, Text = text, Answers = ansArray, CorrectAns = correct });
                        }
                    }
                }

                var bindingList = new BindingList<Question>(questions);
                return bindingList;
            }
        }

        /// <summary>
        /// Updates the user object's data in the SQLite database
        /// </summary>
        /// <param name="u"></param>
        public static void UpdateUserData(User u) {
            using (var cnn = new SQLiteConnection(LoadConnectionString())) {
                cnn.Open();
                var command = cnn.CreateCommand();
                command.CommandText =
                    "update Users set Correct = :correct, Incorrect = :incorrect, QuizzesTaken = :quizzes, LastQuizScore = :lastScore where Name=\"name\"";
                command.Parameters.Add("correct", DbType.String).Value = u.Correct.ToString();
                command.Parameters.Add("incorrect", DbType.String).Value = u.Incorrect.ToString();
                command.Parameters.Add("quizzes", DbType.String).Value = u.Quizzes.ToString();
                command.Parameters.Add("lastScore", DbType.String).Value = u.LastScore.ToString();
                command.Parameters.Add("name", DbType.String).Value = u.Name;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns whether or not a User exists in the database with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool UserExists(string name) {
            return null == GetUserData(name);
        }

        /// <summary>
        /// Returns a User object with the given name, found in the SQLite DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User GetUserData(string name) {
            using (var cnn = new SQLiteConnection(LoadConnectionString())) {
                cnn.Open();
                var command = cnn.CreateCommand();

                command.CommandText = $"SELECT * \n FROM Users \n WHERE Name = \"" + name + "\"";
                Debug.WriteLine(command.CommandText);

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        int correct = reader.GetInt32(1);
                        int incorrect = reader.GetInt32(2);
                        int quizzes = reader.GetInt32(3);
                        int last = reader.GetInt32(4);

                        return (new User { Name = name, Correct = correct, Incorrect = incorrect, Quizzes = quizzes, LastScore = last });
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Creates a new user with given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User AddUser(string name) {
            using (var cnn = new SQLiteConnection(LoadConnectionString())) {
                cnn.Open();
                var command = cnn.CreateCommand();
                command.CommandText = ("INSERT INTO Users(Name, Correct, Incorrect, QuizzesTaken, LastQuizScore) VALUES(\"" + name + "\",\"0\",\"0\",\"0\",\"0\")");
                /*
                command.Parameters.Add("name", DbType.String).Value = "\"" + name + "\"";
                command.Parameters.Add("correct", DbType.String).Value = "0";
                command.Parameters.Add("incorrect", DbType.String).Value = "0";
                command.Parameters.Add("quizzes", DbType.String).Value = "0";
                command.Parameters.Add("lastScore", DbType.String).Value = "0";
                */
                command.ExecuteNonQuery();
            }

            return GetUserData(name); ;
        }

    }
}
