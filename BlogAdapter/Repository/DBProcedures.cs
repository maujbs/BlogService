using System.ComponentModel;

namespace BlogAdapter.Repository
{
    public static class DbProcedure
    {
        #region Posts
        //public const string GetOwnPosts = "[dbo].[GetOwnPosts]";
        public const string GetPosts = "[dbo].[GetPosts]"; //All or filter by author(Own)
        public const string CreatePost = "[dbo].[AddPost]";
        public const string EditPost = "[dbo].[UpdatePost]";
        //public const string SubmitPost = "[dbo].[SubmitPost]"; //Set Pending (Locked)
        public const string GetPostsByStatus = "[dbo].[GetPostsByStatus]"; //Get Pending
        //Approve => Approved, Reject => OnEdition , Submit => Pending (Locked)
        //When Rejected => Allow CreatePostComment
        public const string UpdatePostStatus = "[dbo].[UpdatePostStatus]";

        #endregion

        #region Comments
        public const string CreatePostComment = "[dbo].[AddPostComment]";
        public const string GetPostsComments = "[dbo].[GetPostsComments]";
        #endregion

        #region User
        //Get id, username, encoded password, role
        public const string GetUser = "[dbo].[GetUser]";
        public const string LogOut = "[dbo].[LogOut]";
        public static string AddUser = "[dbo].[AddUser]";

        public static string LogIn = "[dbo].[LogIn]";
        #endregion
    }
}