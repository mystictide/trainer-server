using Dapper.Contrib.Extensions;

namespace trainer.server.Infrastructure.Models.Trainer
{
    [Table("exercises")]
    public class Exercise
    {
        [Key]
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? URL { get; set; }

        [Write(false)]
        public IEnumerable<Category> Categories { get; set; }
    }
}
