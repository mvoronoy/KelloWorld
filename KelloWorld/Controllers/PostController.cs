using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using KelloWorld.Model;
using KelloWorld.Model.CrossLayer;
using KelloWorld.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KelloWorld.Controllers
{
    /// <summary>
    /// Manages ppost subordinated to blog
    /// </summary>
    [Route("api/blog/{blogName}/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IPostService _postService;

        public PostController(
            ILogger<BlogController> logger, 
            IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }
        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatePost([FromRoute] string blogName, [FromBody] PostDto newPost)
        {
            try 
            {
                int correlationId =_postService.CreatePost(blogName, newPost);

                return CreatedAtRoute(
                    nameof(AssyncGetPostById),
                    new { postId = correlationId, blogName = blogName },
                    correlationId
                    );
            } catch (ValidationError)
            {
                return BadRequest();
            }

        }
        [HttpGet()]
        public async Task<IEnumerable<PostDto>> GetPosts([FromRoute] string blogName) 
        {
            IEnumerable<PostDto> result = await _postService.GetPosts(blogName);
            return result;
        }
        [HttpGet("{postId}", Name = nameof(AssyncGetPostById))]
        public async Task<IActionResult> AssyncGetPostById([FromRoute] int id)
        {
            try
            {
                PostDto post = await _postService.GetModeratedPost(id);
                return post == null
                    ? (IActionResult)Accepted()/*not completed yet*/
                    : (IActionResult)Ok(post);
            }
            catch (AggregateException ae) 
            {
                switch (ae.InnerException) {
                    case EntityNotFound ef:
                        return NotFound();
                    default:
                        throw ae.InnerException;
                }
            }
        }

    }

}