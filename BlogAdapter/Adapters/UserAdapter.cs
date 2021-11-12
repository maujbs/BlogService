using BlogAdapter.Repository;
using BlogAdapter.Extensions;
using BlogAdapter.Models;
using System.Collections.Generic;
using BlogsAdapter.Repository;
using System.Linq;

namespace BlogAdapter.Adapters
{
    public class UserAdapter
    {
        public void AddUser(string username, string password, int roleLevel)
        {
            var repository = new DatabaseRepository();
            
            var parameters = new Dictionary<string, object>() { { "@Username", username }, { "@Password", password }, { "@RoleLevel",roleLevel }  };
            repository.Execute(DbProcedure.AddUser, repository.ConnectionString, parameters);
            
        }

        //Required to create a login token
        public User LogIn(string username, string password)
        {
            var repository = new DatabaseRepository();
            var token = CreateUserToken(username, password);

            var parameters = new Dictionary<string, object>() { { "@Username", username }, { "@Password", password }, { "@Token", token } };
            var dataTable = repository.GetDataTable(DbProcedure.LogIn, repository.ConnectionString, parameters);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0].ToTypedObject<User>();
            }
            return default(User);
        }

        public void LogOut(string username, int token)
        {
            var repository = new DatabaseRepository();
            
            var parameters = new Dictionary<string, object>() { { "@Username", username }, { "@Token", token } };
            repository.Execute(DbProcedure.LogOut, repository.ConnectionString, parameters);
        }

        //Get user roles using the assigned login token
        public User ValidateUserRole(int token, List<int> validRoleLevels)
        {
            var repository = new DatabaseRepository();
            
            var parameters = new Dictionary<string, object>() { { "@Token", token } };
            var dataTable = repository.GetDataTable(DbProcedure.GetUser, repository.ConnectionString, parameters);
            //If there is no token, then the user cannot use the services
            //If roles of the token are not validRoles for the action, the user cannot use the service
            //This can be updated to get multiple roles per user
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0].ToTypedObject<User>();
            }
            return default(User);
        }


        /////////////////////////////////////////////////////////////////

        private int CreateUserToken(string username, string password)
        {
            User user = new User { Username = username, Password = password };
            int token = user.GetHashCode();
            return token;
        }



    }
}
