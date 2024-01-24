using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;

namespace WebApplication1.Controllers
{
    public class APIController : Controller
    {
        private readonly MyDBContext _dbContext;

        public APIController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(5000);
            //停5秒

            //return Content("<h2>Hello,Content</h2>");
            //告訴client端回傳純文字檔
            //return Content("<h2>Hello,Content</h2>", "text/HTML");
            //告訴client端回傳html檔
            //return Content("<h2>Content,您好</h2>", "text/plain", System.Text.Encoding.UTF8);
            //要使用中文的話,要告訴他用甚麼編碼,否則會出現亂碼
            return Content("<h2>Content,您好</h2>", "text/html", System.Text.Encoding.UTF8);
            //Content (string content, string contentType,                        System.Text.Encoding contentEncoding);

        }

        public IActionResult CheckAccount(string name)
        {
            var membername = _dbContext.Members.Where(x => x.Name == name).Select(x => x.Name).Any();
            return Content($"{membername}");
        }
        
        //[HttpPost]
        public IActionResult Cities()
        {
            var cities=_dbContext.Addresses.Select(x => x.City).Distinct();
            return Json(cities);
        }
        public IActionResult Districts(string city) 
        {
            if (string.IsNullOrEmpty(city)) city = string.Empty;

            var districts=_dbContext.Addresses.Where(x=>x.City == city).Select(x=>x.SiteId).Distinct();

            return Json(districts);
        }
        public IActionResult Roads(string site) 
        {
            if (string.IsNullOrEmpty(site)) site = string.Empty;

            var roads=_dbContext.Addresses.Where(x=>x.SiteId==site).Select(x=>x.Road).Distinct();

            return Json(roads);
        }
        public IActionResult Avatar(int id=1) 
        {
            Member? member =_dbContext.Members.Find(id);
            if(member != null)
            {
                byte[] img = member.FileData;
                return File(img, "image/jpeg");
                //File(string ,string contentType)
            }

            return NotFound();
        }
        public IActionResult Register(UserDto dto) 
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                dto.Name = "Guest";
            }
            return Content($"Hello!!{dto.Name},You are {dto.Age} years old.Your Email is {dto.Email}");
        }
    }
}
