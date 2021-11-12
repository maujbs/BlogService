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
    public class PostsController : ControllerRequest
    {        
        /// <summary>
        /// Get all the posts
        /// </summary>
        [HttpGet]
        [Route("GetAllPosts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<List<Post>> GetAllPosts(int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.PUBLIC, RoleLevel.WRITER, RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            List<Post> postsCollection = new List<Post>();
            try
            {
                PostsAdapter databaseAdapter = new PostsAdapter();
                postsCollection = databaseAdapter.GetPosts(default, default);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return postsCollection;
        }

        /// <summary>
        /// Get single post
        /// </summary>
        /// <param name="postId">Post id</param>
        [HttpGet]
        [Route("GetPost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Post> GetPost(int postId, int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.WRITER }))
            { return HandleValidationRequestError(); }

            Post post = new Post();
            try
            {
                PostsAdapter databaseAdapter = new PostsAdapter();
                post = databaseAdapter.GetPosts(postId, default).FirstOrDefault() ?? post;
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return post;
        }


        /// <summary>
        /// Get own posts
        /// </summary>
        /// <param name="authorId">Author id of the post</param>
        [HttpGet]
        [Route("GetOwnPosts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult <List<Post>> GetOwnPosts(int authorId, int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.WRITER }))
            { return HandleValidationRequestError(); }

            List<Post> postsCollection = new List<Post>();
            try
            {
                PostsAdapter databaseAdapter = new PostsAdapter();
                postsCollection = databaseAdapter.GetPosts(default, authorId);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return postsCollection;
        }

        [HttpPost]
        [Route("CreatePost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> CreatePost(int authorId, string title, string body, int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.WRITER }))
            { return HandleValidationRequestError(); }

            try
            {
                PostsAdapter databaseAdapter = new PostsAdapter();
                databaseAdapter.CreatePost(authorId, title, body);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        [HttpPost]
        [Route("EditPost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> EditPost(int token, Post post)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.WRITER }))
            { return HandleValidationRequestError(); }

            try
            {
                PostsAdapter databaseAdapter = new PostsAdapter();
                databaseAdapter.EditPost(post.Id, post.Title, post.Body);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        [HttpPost]
        [Route("SubmitPost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> SubmitPost(int token, int postId)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.WRITER }))
            { return HandleValidationRequestError(); }

            try
            {
                UpdatePostStatus(postId, PostStatus.PENDING);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        [HttpGet]
        [Route("GetPendingPosts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<List<Post>> GetPendingPosts(int token)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            List<Post> posts = new List<Post>();
            try
            {
                PostsAdapter databaseAdapter = new PostsAdapter();
                posts = databaseAdapter.GetPostByStatus(PostStatus.PENDING);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return posts;
        }

        [HttpPost]
        [Route("ApprovePost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> ApprovePost(int token, int postId)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            try
            {
                UpdatePostStatus(postId, PostStatus.APPROVED);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        [HttpPost]
        [Route("RejectPost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<object> RejectPost(int token, int postId)
        {
            if (token == 0 || !ValidateRequestPermissions(token, new List<int> { RoleLevel.EDITOR }))
            { return HandleValidationRequestError(); }

            try
            {
                UpdatePostStatus(postId, PostStatus.EDITABLE);
            }
            catch (Exception ex)
            {
                return HandleRequestException(ex);
            }
            return true;
        }

        private void UpdatePostStatus(int postId, string status)
        {
                PostsAdapter databaseAdapter = new PostsAdapter();
                databaseAdapter.UpdatePostStatus(postId, status);
        }
    }
}


