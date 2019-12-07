using System;
using System.Collections.Generic;
using System.Linq;
using OrderBarcodeData.Entities;
using OrderBarcodeData.Abstract;

namespace OrderBarcodeData.Concrete
{
    public class OrderBarcodeRepository: IOrderBarcode
    {
        OrderBarcodeModel context = new OrderBarcodeModel();
        public IEnumerable<tOrderBarcode> BarcodeActualList
        {
            get { return context.tOrderBarcode.Where(p=>p.IsActive==true).OrderBy(t=>t.Code); }
        }
        public IEnumerable<tOrderBarcode> BarcodeFullList
        {
            get { return context.tOrderBarcode.OrderBy(t=>t.Code); }
        }
        public void AddBarcode(tOrderBarcode ob)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    tOrderBarcode dbEntry = context.tOrderBarcode.Where(t => t.Barcode == ob.Barcode && t.Code == ob.Code).FirstOrDefault();
                    context.tOrderBarcode.Remove(dbEntry);
                    context.tOrderBarcode.Add(ob);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch { trans.Rollback(); }
            }
        }
        public void MarkBarcode(tOrderBarcode ob)
        { }
        public bool IsChanged(tOrderBarcode origOb, tOrderBarcode newOb)
        {
            if (origOb.Code == newOb.Code && origOb.Barcode == newOb.Barcode && origOb.IsActive == newOb.IsActive)
                return false;
            return true;
        }
        public bool SaveCollection(List<tOrderBarcode> list)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    tOrderBarcode dbEntry;
                    foreach (var el in list)
                    {
                        dbEntry = context.tOrderBarcode.Where(t => t.Code == el.Code).FirstOrDefault();
                        if (el.Code == "182012")
                            el.Code = "182012";
                        if (dbEntry != null)
                        {
                            if (IsChanged(dbEntry, el))
                            {
                                dbEntry.Barcode = el.Barcode;
                                dbEntry.IsActive = el.IsActive;
                            }                                
                        }
                        else
                            context.tOrderBarcode.Add(el);
                    }
                    context.SaveChanges();
                    trans.Commit();
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }
        public bool DeleteOrder(tOrderBarcode el)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    tOrderBarcode dbEntry = context.tOrderBarcode.Where(t => t.Code == el.Code).FirstOrDefault();
                    if (dbEntry != null)
                    {
                        context.tOrderBarcode.Remove(dbEntry);
                        context.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        trans.Dispose();
                    }
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }
        public bool IsListChanged(List<tOrderBarcode> list)
        {
            foreach (tOrderBarcode el in list)
            { 
                tOrderBarcode dbEntry = context.tOrderBarcode.Where(t => t.Code == el.Code).FirstOrDefault();
                if (dbEntry != null)
                {
                    if (IsChanged(dbEntry, el))
                        return true;
                }
                else
                    return true;
            }
            return false;
        }
    }
}
