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
    public class UserData : IUserData
    {
        private ISQLDataAccess _sql;

        public UserData(ISQLDataAccess sql)
        {
            _sql = sql;
        }
        public List<UserModel> GetUserById(string Id)
        {



            var parameters = new { UserId = Id };

            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "Sistem-Za-Maloprodaju");

            return output;
        }
    }
}
