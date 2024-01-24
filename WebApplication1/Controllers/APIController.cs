using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;
using WebApplication1.Models.Infra;

namespace WebApplication1.Controllers
{
    public class APIController : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly IWebHostEnvironment _host;

        public APIController(MyDBContext dbContext,IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
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
            //Content (string content, string contentType,System.Text.Encoding contentEncoding);

        }

        public IActionResult CheckAccount(string name)
        {
            var nameInDb = _dbContext.Members.Where(x => x.Name == name).Any();
            return Content($"{nameInDb}");
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

            string fileName = "empty.jpg";
            if(dto.Image != null)
            {
                fileName= dto.Image.FileName;
            }
            //取得檔案存放資料夾的路徑+檔案名稱
            string uploadPath = Path.Combine(_host.WebRootPath, "img", fileName);

            //上傳檔案到資料夾
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                dto.Image?.CopyTo(fileStream);
            }

            //轉成二進位的資料
            byte[]? imgByte = null;
            using(var memoryStream=new MemoryStream())
            {
                dto.Image?.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            var salt = HashUtility.GetSalt();
            if (dto.Password == null)
            {
                throw new Exception("密碼不能為空值");
            }
            var hashedPassword=HashUtility.ToSHA256(dto.Password, salt);

            var member = new Member
            {
                Name = dto.Name,
                Age = dto.Age,
                Email = dto.Email,
                FileName = fileName,
                FileData = imgByte,
                Password = hashedPassword,
                Salt = salt
            };
            _dbContext.Members.Add(member);
            _dbContext.SaveChanges();

            //todo 檔案已存在如何處理?
            //todo 限制上傳的檔案類型
            //todo 上傳檔案大小

            return Content("新增成功");

            //return Content($"{dto.Image?.FileName}~{dto.Image?.Length}~{dto.Image?.ContentType}~{uploadPath}");

            //return Content($"Hello!!{dto.Name},You are {dto.Age} years old.Your Email is {dto.Email}");
            //return Content($"{Path.Combine(uploadPath, "img", fileName)}");
        }

        [HttpPost]
        public IActionResult Spots([FromBody]SearchDto dto)
        {
            
            //根據分類編號讀取景點資料
            var spots = dto.categoryId == 0 ? _dbContext.SpotImagesSpots : _dbContext.SpotImagesSpots.Where(x=>x.CategoryId==dto.categoryId);
            //根據關鍵字搜尋
            if (!string.IsNullOrEmpty(dto.keyword))
            {
                spots=spots.Where(x=>x.SpotTitle.Contains(dto.keyword) || x.SpotDescription.Contains(dto.keyword) );
            }
            //根據哪個欄位排序  排序方式是否為asc
            switch(dto.sortBy){
                case "spotTitle":
                    spots= dto.sortType=="asc"?spots.OrderBy(x =>x.SpotTitle): spots.OrderByDescending(x => x.SpotTitle);
                    break;
                case "categoryId":
                    spots = dto.sortType == "asc" ? spots.OrderBy(x => x.CategoryId) : spots.OrderByDescending(x => x.CategoryId);
                    break;
                default:
                    spots = dto.sortType == "asc" ? spots.OrderBy(x => x.SpotId) : spots.OrderByDescending(x => x.SpotId);
                    break;
            }

            //取得分頁
            int page = dto.Page ?? 1;
            int pageSize = dto.pageSize ?? 9;

            int totalpages=(int)Math.Ceiling((double)spots.Count()/ pageSize) ;

            page = page > totalpages ? totalpages : page;

            spots=spots.Skip((page-1)*pageSize).Take(pageSize);
            SpotsPagingDto pagingDto = new SpotsPagingDto
            {
                TotalPages = totalpages,
                SpotResult = spots
            };

            return Json(pagingDto);
        }
    }
}
