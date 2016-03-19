using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aveva.PDMS.PMLNet;
using Aveva.Pdms.Database;
using Aveva.Pdms.Shared;

namespace PHS.CustomizingExplorer
{
    [PMLNetCallable]
    class AddminMDB
    {
        [PMLNetCallable]
        public AddminMDB()
        {
            
        }


        [PMLNetCallable]
        public void TestGetNeigh()
        {
            var elementsInElementBox = Spatial.Instance.ElementsInElementBox(CurrentElement.Element, new[] {DbElementTypeInstance.BLOCK}, false);
            foreach (var element in elementsInElementBox)
            {
                Console.WriteLine(element);
            }
        }

        [PMLNetCallable]
        public void CreateMDB()
        {
            var dbElement = Project.CurrentProject.SystemDB.WorldMembers().First(x => x.GetActualType() == DbElementTypeInstance.MDBW);
            for (int i = 0; i < 5000; i++)
            {
                dbElement.CreateLast(DbElementTypeInstance.MDB); 
            }
        }
    }
}
