namespace ReadNest.Shared.Common
{
    using System.Text;

    public class UserAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class BookSuggestion
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Reason { get; set; }
        public string Image { get; set; }
        public string InfoLink { get; set; }
    }

    public class BookSearchResult
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Thumbnail { get; set; }
        public string InfoLink { get; set; }
    }

    public class PromptBuilder
    {
        private readonly List<UserAnswer> _answers = new();

        public PromptBuilder AddAnswer(string question, string answer)
        {
            _answers.Add(new UserAnswer { Question = question, Answer = answer });
            return this;
        }

        public PromptBuilder AddAnswerList(IEnumerable<UserAnswer> answers)
        {
            _answers.AddRange(answers);
            return this;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là chuyên gia gợi ý sách.");
            sb.AppendLine("Nhiệm vụ: Đề xuất 5 cuốn sách phù hợp với người dùng dựa trên thông tin sau:");
            sb.AppendLine();

            foreach (var ans in _answers)
            {
                sb.AppendLine($"- {ans.Question}: {ans.Answer}");
            }

            sb.AppendLine();
            sb.AppendLine("Yêu cầu:");
            sb.AppendLine("1. Đưa ra danh sách 5 sách phù hợp nhất.");
            sb.AppendLine("2. Chỉ trả về kết quả JSON thuần, không kèm giải thích.");
            sb.AppendLine("3. JSON có dạng mảng gồm 5 object với các trường: title, author, genre, reason (lý do viết bằng tiếng Việt, image, infoLink .");
            sb.AppendLine("4. Nếu không có thông tin, để giá trị là 'Unknown'.");

            return sb.ToString();
        }
    }

}
