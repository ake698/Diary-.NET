using NETCOREApi.Dto;
using NETCOREApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Utils
{
    public class ModelToDtoUtil
    {
        public static DiaryUserDto GetDiaryUserDto(Diary diary,User user)
        {
            DiaryUserDto dto = new DiaryUserDto();
            dto.Id = diary.Id;
            dto.Title = diary.Title;
            dto.Content = diary.Content;
            dto.CreateTime = diary.CreateTime;
            dto.IsPublic = diary.IsPublic;
            dto.User = GetUserDto(user);
            
            //dto.UserId = user.Id;
            //dto.Username = user.Username;
            return dto;
        }

        public static UserDto GetUserDto(User user)
        {
            UserDto userDto = new UserDto();
            userDto.Username = user.Username;
            userDto.Id = user.Id;
            userDto.Avatar = user.Avatar;
            return userDto;
        }

        public static DiaryWithPageInfoDto GetDiaryWithPageInfoDto(List<DiaryUserDto> diaryUserDtos, PageDto pageDto)
        {
            DiaryWithPageInfoDto dto = new DiaryWithPageInfoDto();
            dto.Diaries = diaryUserDtos;
            dto.PageDto = pageDto;
            return dto;
        }
    }
}
