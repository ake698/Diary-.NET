using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NETCOREApi.Dto;
using NETCOREApi.Models;
using NETCOREApi.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NETCOREApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("any")]  //设置跨域处理代理
    public class UserController : Controller
    {

        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;
        //private IWebHostEnvironment hostingEnvironment; //路径不对
        public UserController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //this.hostingEnvironment = env;
        }

        [HttpGet]
        public void Get()
        {
        }

        // POST api/<controller>
        [HttpPost("/api/login")]
        public IActionResult UserLogin([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User result = _context.User.SingleOrDefault(u => u.Username == user.Username);
            if (result == null || result.Password != user.Password)
            {
                return BadRequest(new
                {
                    msg = "用户名或者密码错误！请稍后再试！"
                });
            }
            JwtTokenUtil jwtUtil = new JwtTokenUtil(_configuration);
            string token = jwtUtil.GetToken(result);

            return Ok(new
            {
                token = token,
                id = result.Id,
                avatar = result.Avatar,
                username = result.Username,
                msg = "登陆成功"
            });

        }

        [HttpPost("/api/register")]
        public IActionResult UserRegister([FromBody]User userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User result = _context.User.SingleOrDefault(u => u.Username == userInfo.Username);
            if (result != null)
            {
                return BadRequest(new
                {
                    msg = "该用户已注册，请重新设置用户名！"
                });
            }
            userInfo.Avatar = "/static/default.jpg";
            _context.User.Add(userInfo);
            _context.SaveChanges();
            return Ok(new
            {
                msg = "注册成功"
            });
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody]PasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("密码长度必须在6-20位之间!");
            }
            
            User user = GetCurrentUser();
            if (dto.NewPassword == dto.OldPassword || dto.NewPassword == user.Password)
            {
                return BadRequest(new
                {
                    msg = "新密码不能和旧密码相同！"
                });
            }

            if (user.Password != dto.OldPassword)
            {
                return BadRequest(new
                {
                    msg = "原密码错误！"
                });
            }
            user.Password = dto.NewPassword;
            _context.Update(user);
            _context.SaveChanges();
            return Ok();

        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("/api/avatar")]
        [Authorize]
        public IActionResult OnPostUploadAsync(IFormFile avatar)
        {
            
            if (avatar == null || avatar.Length > 1024 * 1024 * 5) return BadRequest(new { msg = "上传错误！" });
            
            //获取文件名
            string fileName = ContentDispositionHeaderValue.Parse(avatar.ContentDisposition).FileName;
            //获取文件类型
            string extName = fileName.Substring(fileName.LastIndexOf(".")).Replace("\"", "");
            Console.WriteLine(extName);
            string[] types = { ".jpg", ".png" };
            if (!types.Contains(extName)) return BadRequest(new { msg = "格式错误!" });
            //生成随机命名
            string localName = $"{Guid.NewGuid()}{extName}";
            //获取文件绝对路径
            fileName = StaticPath.DirName + localName;
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                avatar.CopyTo(fs);
                fs.Flush();
            }
            User user = GetCurrentUser();
            user.Avatar = "/static/" + localName;
            _context.Update(user);
            _context.SaveChanges();
            



            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = user.Avatar });
        }


        [HttpPost("/api/current2")]
        public User GetCurrentUser()
        {
            string token = Request.Headers["Authorization"];
            JwtTokenUtil jwtUtil = new JwtTokenUtil(_configuration);
            long userid = jwtUtil.DecodeToken(token);
            User user = _context.User.Find(userid);
            if (user == null)
            {
                throw new Exception("不存在用户!");
            }
            return user;
        }
    }
}
