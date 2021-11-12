using BlogAdapter.Repository;
using BlogAdapter.Extensions;
using BlogAdapter.Models;
using System.Collections.Generic;
using BlogsAdapter.Repository;
using System;

namespace BlogAdapter.Adapters
{
    public class PostsAdapter
    {
        public List<Post> GetPosts(int postId, int authorId)
        {
            var repository = new DatabaseRepository();
            var parameters = new Dictionary<string, object>() { { "@PostId", postId }, { "@AuthorId", authorId } };
            var dataRows = repository.GetDataTable(DbProcedure.GetPosts, repository.ConnectionString, parameters).Rows;
            
            //Convert rows to strong typed objects
            return dataRows.ToTypedCollection<Post>();
        }

        public void CreatePost(int authorId, string title, string body)
        {
            var repository = new DatabaseRepository();
            var parameters = new Dictionary<string, object>() { { "@AuthorId", authorId }, { "@Title", title }, { "@Body", body } };
            repository.Execute(DbProcedure.CreatePost, repository.ConnectionString, parameters);
        }
        public void EditPost(int postId, string title, string body)
        {
            var repository = new DatabaseRepository();
            var parameters = new Dictionary<string, object>() { { "@PostId", postId }, { "@Title", title }, { "@Body", body } };
            repository.Execute(DbProcedure.EditPost, repository.ConnectionString, parameters);
        }

        public void UpdatePostStatus(int postId, string status)
        {
            var repository = new DatabaseRepository();
            var parameters = new Dictionary<string, object>() { { "@PostId", postId }, { "@Status",status } };
            repository.Execute(DbProcedure.UpdatePostStatus, repository.ConnectionString, parameters);
        }

        public List<Post> GetPostByStatus(string status)
        {
            var repository = new DatabaseRepository();
            var parameters = new Dictionary<string, object>() { { "@Status", status } };
            var dataRows = repository.GetDataTable(DbProcedure.GetPostsByStatus, repository.ConnectionString, parameters).Rows;

            //Convert rows to strong typed objects
            return dataRows.ToTypedCollection<Post>();
        }
    }
}
