using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Dto
{
    public class DiaryWithPageInfoDto
    {
        public List<DiaryUserDto> Diaries { get; set; }
        public PageDto PageDto { get; set; }

    }
}
