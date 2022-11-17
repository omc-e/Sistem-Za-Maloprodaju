using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class UserData
    {
        private IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }
        public List<UserModel> GetUserById(string Id)
        {

            SQLDataAccess sql = new SQLDataAccess(_config);

            var parameters = new {UserId = Id};

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "Sistem-Za-Maloprodaju");

            return output;
        }
    }
}
