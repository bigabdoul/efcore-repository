using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreRepository.Test.Entities
{
    [Table("Blogs")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
