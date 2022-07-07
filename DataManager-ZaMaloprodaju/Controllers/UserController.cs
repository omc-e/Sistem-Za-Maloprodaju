using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace DataManager_ZaMaloprodaju.Controllers
{
    [Authorize]
    
    public class UserController : ApiController
    {
        [HttpGet]
        // GET: User/Details/5
        //Security: Only logged user information can be given back
        public UserModel GetById()
        {
            string userID = RequestContext.Principal.Identity.GetUserId();

            //Using library model, not api model
            //Because api model is for display
            UserData data = new UserData();

            return data.GetUserById(userID).First();
            
        }
      }
    }


