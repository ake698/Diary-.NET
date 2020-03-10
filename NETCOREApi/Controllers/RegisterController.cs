//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using NETCOREApi.Models;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace NETCOREApi.Controllers
//{
//    [Route("api/[controller]")]
//    public class RegisterController : Controller
//    {
//        private readonly TodoContext _context;
//        public RegisterController(TodoContext context)
//        {
//            _context = context;
//        }

//        // POST api/<controller>
//        [HttpPost]
//        public IActionResult Post([FromBody]User userInfo)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            User result = _context.User.SingleOrDefault(u => u.Username == userInfo.Username);
//            if (result != null)
//            {
//                return BadRequest(new
//                {
//                    msg = "该用户已注册，请重新设置用户名！"
//                });
//            }
//            _context.User.Add(userInfo);
//            _context.SaveChanges();
//            return Ok(new
//            {
//                msg = "注册成功"
//            });

            
//        }

//    }
//}
