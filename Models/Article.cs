using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS54.Models
{
    public class Article
    {
        [Key]
        public int ID { get; set; }
        [StringLength(255, MinimumLength = 5, ErrorMessage ="{0} phải dài từ {2} tới {1}")]
        [Required(ErrorMessage = "{0} phải nhập")]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }


        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Ngày tạo")]

        public DateTime Created { get; set; }
        [Column(TypeName = "ntext")]
        [Display(Name = "Nội dung")]


        public string Content { set; get; }

    }
}
