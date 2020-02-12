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
using KelloWorld.Model.CrossLayer;

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
            return _db.Blogs.ToArray();
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var b = await _db.Blogs.Where(b => b.BlogId == id)
                .Select(dao => dao.Cast<BlogDto>()).SingleOrDefaultAsync();
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
                var newBlog = blog.Cast<Blog>();
                _db.Blogs.Add(newBlog);
                await _db.SaveChangesAsync();

                return CreatedAtRoute(
                    "GetById",
                    new { id = newBlog.BlogId },
                    newBlog.Cast< BlogDto>());
            }
            catch (DbUpdateException)
            {//Blog conflicts with existing records (duplicate) 
                return Conflict();
            }

        }
        [HttpPost("{blogName}/post")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePost([FromRoute] string blogName, [FromBody] PostDto newPost)
        {
            var dbPost = newPost.Cast<Post>();
            var dbBlog = await
                _db.Blogs.Where(b => b.Url == blogName).SingleOrDefaultAsync();
            if (dbBlog == null)
            {
                return BadRequest();
            }
            dbPost.BlogId = dbBlog.BlogId;
            _db.Posts.Add(dbPost);
            await _db.SaveChangesAsync();
            return CreatedAtRoute(
                    "api/Post",
                    new { id = dbPost.PostId },
                    dbPost.Cast<PostDto>()); 
        }
    }
}
