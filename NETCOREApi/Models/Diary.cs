using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Models
{
    public class Diary
    {
        public long Id { get; set; }
        //public string Author { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须在1-50字符之间！")]
        public string Title { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 4, ErrorMessage = "长度必须在4-2000字符之间！")]
        public string Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; }
        [Required]
        public bool IsPublic { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }
    }
}
