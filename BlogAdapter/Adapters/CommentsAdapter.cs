using BlogAdapter.Repository;
using BlogAdapter.Extensions;
using BlogAdapter.Models;
using System.Collections.Generic;
using BlogsAdapter.Repository;
using System;

namespace BlogAdapter.Adapters
{
    public class CommentsAdapter
    {
        public void AddPostComment(int postId, string text)
        {
            var repository = new DatabaseRepository();
            //PostId of the comment
            var parameters = new Dictionary<string, object>() { { "@PostId", postId }, { "@Text", text } };
            repository.Execute(DbProcedure.CreatePostComment, repository.ConnectionString, parameters);
        }

        public List<Comment> GetPostsComments(int commentId, int postId)
        {
            var repository = new DatabaseRepository();
            var parameters = new Dictionary<string, object>() { { "@CommentId", commentId }, {"@PostId", postId } };
            var dataRows = repository.GetDataTable(DbProcedure.GetPostsComments, repository.ConnectionString, parameters).Rows;

            //Convert rows to strong typed objects
            return dataRows.ToTypedCollection<Comment>();
           
        }
    }
}
