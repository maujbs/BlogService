using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using BlogAdapter.Adapters;
using BlogAdapter.Models;

namespace BlogServiceAPI.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerRequest
    {        
        [HttpPost]
        [Route("AddPostComment")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> AddPostComment(int postId, string text, int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.PUBLIC, RoleLevel.WRITER, RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            try
            {
                CommentsAdapter commentsAdapter = new CommentsAdapter();
                commentsAdapter.AddPostComment(postId, text);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        /// <summary>
        /// Get posts comments
        /// </summary>
        /// <param name="postId">Post Id to get a comments of that post</param>
        [HttpGet]
        [Route("GetCommentsByPost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<List<Comment>> GetCommentsByPost(int postId, int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.PUBLIC, RoleLevel.WRITER, RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            List<Comment> commentsCollection = new List<Comment>();
            try
            {
                CommentsAdapter databaseAdapter = new CommentsAdapter();
                commentsCollection = databaseAdapter.GetPostsComments(default, postId);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return commentsCollection;
        }

        /// <summary>
        /// Get single comment
        /// </summary>
        /// <param name="commentId">Comment Id to get a single post</param>
        [HttpGet]
        [Route("GetComment")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Comment> GetComment(int commentId, int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.PUBLIC, RoleLevel.WRITER, RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            Comment comment = new Comment();
            try
            {
                CommentsAdapter databaseAdapter = new CommentsAdapter();
                comment = databaseAdapter.GetPostsComments(commentId, default).FirstOrDefault() ?? comment;
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return comment;
        }


    }
}


