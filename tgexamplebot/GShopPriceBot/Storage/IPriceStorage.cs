using GShopPriceBot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GShopPriceBot.Storage
{
    /// <summary>
    /// Price storage
    /// </summary>
    public interface IPriceStorage
    {
        /// <summary>
        /// Init storage (create tables, etc)
        /// </summary>
        void Init();

        /// <summary>
        /// Insert all positions
        /// </summary>
        /// <param name="positions"></param>
        void AddPositions(PricePosition[] positions);

        /// <summary>
        /// Find price positions
        /// </summary>
        /// <param name="namePart"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<PricePosition> Find(string namePart, string category, int limit);

        /// <summary>
        /// Add an user query
        /// </summary>
        /// <param name="query"></param>
        void AddUserQuery(UserQuery query);

        /// <summary>
        /// Return all unique user ids, that registered in our database
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetUniqueUserIds();

        /// <summary>
        /// Removes all price positions
        /// </summary>
        void ClearPrices();
    }
}
