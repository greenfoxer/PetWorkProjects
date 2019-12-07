using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace GShopPriceBot.Models
{
    /// <summary>
    /// User query model. We use it to remember ALL user queries (hehe, then we will send the data, LOL)
    /// </summary>
    public class UserQuery
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Query { get; set; }

        public UserQuery(User user, string query)
        {
            UserId = user.Id;
            UserName = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Query = query;
        }
    }
}
