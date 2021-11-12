using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BlogServiceAPI.Controllers
{
    public class ControllerRequest : ControllerBase
    {
        
        protected BadRequestObjectResult HandleRequestException(Exception exception)
        {
            return BadRequest("Something went wrong. " + exception.Message + ":" + exception.StackTrace);
        }

        protected BadRequestObjectResult HandleValidationRequestError()
        {
            return BadRequest("User is not allowed to process this action");
        }

        protected bool ValidateRequestPermissions(int token, List<int> validRoleLevels)
        {
            //If there is no token, then the user cannot use the services
            //If roles of the token are not validRoles for the action, the user cannot use the service
            //This can be updated to get multiple roles per user
            try
            {
                BlogAdapter.Adapters.UserAdapter databaseAdapter = new BlogAdapter.Adapters.UserAdapter();
                var userRole = databaseAdapter.ValidateUserRole(token, validRoleLevels);
                return userRole != null && validRoleLevels.Contains(userRole.RoleLevel);
            }
            catch (Exception ex)
            {
                HandleRequestException(ex);
            }
            return default(bool);
        }
    }
}