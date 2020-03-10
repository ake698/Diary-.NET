using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Dto
{
    public class DiaryUserDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
        public bool IsPublic { get; set; }

        //public long UserId { get; set; }

        //public string Username { get; set; }
        public UserDto User { get; set; }

    }
}
