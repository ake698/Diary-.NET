using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCOREApi.Dto;
using NETCOREApi.Models;
using NETCOREApi.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NETCOREApi.Controllers
{
    [EnableCors("any")]  //设置跨域处理代理
    [ApiController]
    [Authorize]
    [Route("api/diary/")] //question  加上/api无法访问
    public class DairyController : Controller
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;
        public DairyController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/<controller>
        [HttpGet]
        public ActionResult<DiaryWithPageInfoDto> Get([FromQuery] PageDto pageInfo)
        {
            var diaries = _context.Diary.Include(q => q.User);
            pageInfo.Total = diaries.Count();
            var result = diaries
                .Where(p => p.IsPublic == true)
                .OrderByDescending(t => t.CreateTime)
                .Skip(pageInfo.PageSize * (pageInfo.PageIndex -1))
                .Take(pageInfo.PageSize)
                .ToList();
            List<DiaryUserDto> dto = new List<DiaryUserDto>();
            //foreach(var d in diaries)
            //{
            //    Console.WriteLine(d.User.Username);
            //    dto.Add(ModelToDtoUtil.GetDiaryUserDto(d, d.User));//apend  无法实现
            //}
            result.ForEach(d => dto.Add(ModelToDtoUtil.GetDiaryUserDto(d, d.User)));
            //_context.Diary.Include(q => q.User).ToList()
            return ModelToDtoUtil.GetDiaryWithPageInfoDto(dto, pageInfo);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<DiaryUserDto> Get(long id)
        {

            //var diary =  _context.Diary.Find(id);
            var diary = _context.Diary.Include(q => q.User).SingleOrDefault(o => o.Id == id);
            if (diary == null)
            {
                return NotFound();
            }
            User user = GetCurrentUser();
            if(diary.IsPublic == false && diary.User != user)
            {
                return BadRequest(new { 
                    msg = "没有权限查看!"
                });
            }
            Console.WriteLine(diary.User.Username);
            return ModelToDtoUtil.GetDiaryUserDto(diary, diary.User);
            //return diary;
        }

        [HttpGet("/api/diary/user")]
        public ActionResult<DiaryWithPageInfoDto> GetDiaryByUser([FromQuery] PageDto pageInfo)
        {
            User user = GetCurrentUser();
            //Console.WriteLine(user.Id);
            var diaries = _context.Diary.Include(q => q.User)
                .Where(i => i.UserId == user.Id);
            pageInfo.Total = diaries.Count();
            var result = diaries
                .OrderByDescending(t => t.CreateTime)
                .Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1))
                .Take(pageInfo.PageSize)
                .ToList();
            List<DiaryUserDto> dto = new List<DiaryUserDto>();
            result.ForEach(d => dto.Add(ModelToDtoUtil.GetDiaryUserDto(d, d.User)));
            //_context.Diary.Include(q => q.User).ToList()
            return ModelToDtoUtil.GetDiaryWithPageInfoDto(dto, pageInfo);
        }

        // POST api/<controller>
        [HttpPost]
        public  ActionResult<DiaryUserDto> Post([FromBody]Diary diary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = GetCurrentUser();
            diary.UserId = user.Id;
            diary.CreateTime = DateTime.Now;
            _context.Diary.Add(diary);
            
             _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = diary.Id }, ModelToDtoUtil.GetDiaryUserDto(diary, user));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult<DiaryUserDto> PutStudent(long id, Diary diary)
        {
            User user = GetCurrentUser();
            var result = _context.Diary.Include(q => q.User).SingleOrDefault(o => o.Id == id);
            if(result == null || result.User != user)
            {
                return BadRequest(new
                {
                    msg = "没有权限操作!"
                });
            }
            result.Title = diary.Title;
            result.Content = diary.Content;
            result.IsPublic = diary.IsPublic;
            _context.Update(result);
            _context.SaveChanges();
            //diary.Id = result.Id;
            //_context.Entry(diary).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //_context.Entry(diary).Property(x => x.CreateTime).IsModified = false;
            //_context.Entry(diary).Property(x => x.UserId).IsModified = false;
            //try
            //{
            //    _context.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    return NotFound();
            //}

            return CreatedAtAction(nameof(Get), new { id = diary.Id }, ModelToDtoUtil.GetDiaryUserDto(result, user));
        }

        /// <summary>
        /// Deletes a specific Diary.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public ActionResult<DiaryUserDto> Delete(long id)
        {
            User user = GetCurrentUser();
            var result = _context.Diary.Include(q => q.User).SingleOrDefault(o => o.Id == id);
            if (result == null || result.User != user)
            {
                return BadRequest(new
                {
                    msg = "没有权限操作!"
                });
            }
            var temp = _context.Diary.Find(id);
            _context.Diary.Remove(temp);
            _context.SaveChanges();
            return ModelToDtoUtil.GetDiaryUserDto(result, user);
        }

        [HttpPost("/api/current")]
        public User GetCurrentUser()
        {
            string token = Request.Headers["Authorization"];
            JwtTokenUtil jwtUtil = new JwtTokenUtil(_configuration);
            long userid = jwtUtil.DecodeToken(token);
            User user = _context.User.Find(userid);
            if(user == null)
            {
                throw new Exception("不存在用户!");
            }
            return user;
        }


    }
}
