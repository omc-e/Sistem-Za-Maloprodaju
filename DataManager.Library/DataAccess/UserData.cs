using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {

            SQLDataAccess sql = new SQLDataAccess();

            var parameters = new {UserId = Id};

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "Sistem-Za-Maloprodaju-DB");

            return output;
        }
    }
}
