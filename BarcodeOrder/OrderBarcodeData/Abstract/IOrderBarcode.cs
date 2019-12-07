using System.Collections.Generic;
using OrderBarcodeData.Entities;
namespace OrderBarcodeData.Abstract
{
    public interface IOrderBarcode
    {
        IEnumerable<tOrderBarcode> BarcodeActualList { get; }
        IEnumerable<tOrderBarcode> BarcodeFullList { get; }
        void AddBarcode(tOrderBarcode ob);
        void MarkBarcode(tOrderBarcode ob);
        bool SaveCollection(List<tOrderBarcode> list);
        bool DeleteOrder(tOrderBarcode el);
        bool IsListChanged(List<tOrderBarcode> list);
    }
}
