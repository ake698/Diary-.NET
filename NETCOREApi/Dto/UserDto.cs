using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "账号长度必须在4-20位之间!！")]
        public string Username { get; set; }
        public string Avatar { get; set; }

    }
}
