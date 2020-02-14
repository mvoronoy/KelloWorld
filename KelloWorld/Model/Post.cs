using System;

namespace KelloWorld.Model
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        /// <summary>
        /// Date when post has been accepted for the moderation
        /// </summary>
        public DateTime ModerationDate { get; set; }
        /// <summary>
        /// Status of moderation 0 - initial, 1 - accepted, -1 rejcted
        /// </summary>
        public int ModerationStatus { get; set; }
    }
}
