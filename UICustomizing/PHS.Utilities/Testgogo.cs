using Aveva.Pdms.Database;
using Aveva.Pdms.Shared;
using Aveva.PDMS.Database.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHS.Utilities
{
    class Testgogo
    {
        public Testgogo()
        {

        }
        public void run()
        {
            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
            DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.PIPE };
            TypeFilter filter = new TypeFilter(dbtype);
            AndFilter finalfilter = new AndFilter();
            AttributeStringFilter filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("Name"), FilterOperator.Equals, "/babo");
                
            finalfilter.Add(filter);
            finalfilter.Add(filter2);

                
            DBElementCollection spwl_collects = new DBElementCollection(Outfit_Elements, finalfilter);
            Console.WriteLine("스펙월드갯수:" + spwl_collects.Cast<DbElement>().Count());

            int speccnt = 0;
            List<DbElement> working_dbelement = new List<DbElement>();


            foreach (DbElement element in spwl_collects)
            {
                //element.Delete();
                DbCopyOption dd= new DbCopyOption();
                
                //dd.ToName="/bbboa";
                //DbElement de= element.CreateCopyHierarchyAfter(CurrentElement.Element, dd);
                //de.InsertBefore(CurrentElement.Element);
                DbElement xx=DbElement.GetElement("/xx1");

                CurrentElement.Element.InsertAfterLast(xx);
                //de.InsertAfterLast(element.Owner);
                ////Console.WriteLine("스펙월드]" + spwl.GetAsString(DbAttributeInstance.NAME));
                //foreach (DbElement spec in spwl.Members())
                //{


                //    //spec.CreateLast(DbElementType.GetElementType("Sele"));


                //    string specname = spec.GetAsString(DbAttributeInstance.NAME);
                //    if (specname.Substring(1, 2) == "/*")
                //        continue;

                //    working_dbelement.Add(spec);
                //    speccnt++;
                //    //Console.WriteLine("   -->스펙]"+spec.GetAsString(DbAttributeInstance.NAME));
                //}
            }
            //MDB.CurrentMDB.GetWork();
                    
        }
    }
}
