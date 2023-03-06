using Dapper.Contrib.Extensions;

namespace trainer.server.Infrastructure.Models.Trainer
{
    [Table("exercises")]
    public class Exercise
    {
        public Exercise()
        {
            Categories = new List<Category>();
        }
        [Key]
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? PreviewURL { get; set; }
        public string? VideoURL { get; set; }

        [Write(false)]
        public List<Category> Categories { get; set; }
    }
}
