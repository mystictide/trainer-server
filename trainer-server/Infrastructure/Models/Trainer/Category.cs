using Dapper.Contrib.Extensions;

namespace trainer.server.Infrastructure.Models.Trainer
{
    [Table("categories")]
    public class Category
    {
        [Key]
        public int? ID { get; set; }
        public string? Name { get; set; }
    }
}
