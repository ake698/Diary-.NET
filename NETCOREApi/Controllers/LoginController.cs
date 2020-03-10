//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using NETCOREApi.Models;
//using NETCOREApi.Utils;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace NETCOREApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : Controller
//    {
//        private readonly TodoContext _context;
//        private readonly IConfiguration _configuration;
//        public LoginController(TodoContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }
//        // GET: api/<controller>
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "hello", "world" };
//        }


//        //获取token呢
//        // POST api/<controller>
//        [HttpPost]
//        public IActionResult Post([FromBody]User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            User result = _context.User.SingleOrDefault(u => u.Username == user.Username);
//            if (result == null || result.Password != user.Password)
//            {
//                return BadRequest(new
//                {
//                    msg = "用户名或者密码错误！请稍后再试！"
//                });
//            }
//            JwtTokenUtil jwtUtil = new JwtTokenUtil(_configuration);
//            string token = jwtUtil.GetToken(result);

//            return Ok(new
//            {
//                token = token,
//                msg = "登陆成功"
//            });

//        }
//    }

//}

