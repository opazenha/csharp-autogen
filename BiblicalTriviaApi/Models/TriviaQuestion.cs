using System;

namespace BiblicalTriviaApi.Models
{
    public class TriviaQuestion
    {
        public string? Question { get; set; }
        public string[]? Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string? Explanation { get; set; }
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public string? Reference { get; set; }
    }
}
