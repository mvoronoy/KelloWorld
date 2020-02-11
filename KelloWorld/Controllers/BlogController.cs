using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KelloWorld.Model;

namespace KelloWorld.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BlogController : ControllerBase
    {

        private readonly ILogger<BlogController> _logger;
        private readonly Model.BloggingContext _db;
        public BlogController(ILogger<BlogController> logger, Model.BloggingContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Model.Blog> Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            return _db.Blogs.ToArray();
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var b = await _db.Blogs.Where(b => b.BlogId == id)
                .Select(dao => new BlogDto
                {
                    BlogId = dao.BlogId,
                    Url = dao.Url
                }).SingleOrDefaultAsync();
            if (b == null)
            {
                return NotFound();
            }
            return Ok(b);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDto blog)
        {
            //validate
            if (!(blog?.Url?.All(char.IsLetterOrDigit) ?? false))
            {//blog url either absent or not a valid literal
                return BadRequest();
            }
            try
            {
                var newBlog = new Blog
                {
                    Url = blog.Url
                };
                _db.Blogs.Add(newBlog);
                await _db.SaveChangesAsync();

                return CreatedAtRoute(
                    "GetById",
                    new { id = newBlog.BlogId },
                    new BlogDto
                    {
                        BlogId = newBlog.BlogId,
                        Url = newBlog.Url
                    });
            }
            catch (DbUpdateException)
            {//Blog conflicts with existing records (duplicate) 
                return Conflict();
            }

        }
    }
}
