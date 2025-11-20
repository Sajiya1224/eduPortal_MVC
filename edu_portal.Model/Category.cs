using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eduPortal.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(50)]
        public string Name { get; set; }
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
