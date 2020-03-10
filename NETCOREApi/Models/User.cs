using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "账号长度必须在4-20位之间!！")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20位之间!")]
        public string Password { get; set; }

        public string Avatar { get; set; }
        //public List<Diary> Diaries { get; set; }  //加上会导致 死循环

    }

}
