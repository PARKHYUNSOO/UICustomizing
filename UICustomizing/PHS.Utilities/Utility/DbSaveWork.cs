using Aveva.Pdms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHS.Utilities.Utility
{
    public delegate void Saveworkhandler();
    public class DbSaveWork
    {
        /// <summary>
        /// Savework event..
        /// </summary>
        public event Saveworkhandler Saveworked;

        public void StartTracking()
        {
            //start savework event..
            DbEvents.AddDBFileChangedEventHandler(OnSavework);
        }

        private void OnSavework(DbRawChanges changes, DbEvents.operation op)
        {
            if (op != DbEvents.operation.op_savework)
                return;


            //새로 생성된 element
            while (true)
            {
                DbElement element = changes.NextCreation();
                if (element == DbElement.GetElement())
                    break;
            }
            //삭제된 Element
            while (true)
            {
                DbElement element = changes.NextDeletion();
                if (element == DbElement.GetElement())
                    break;
            }
            //변경된 Element
            while (true)
            {
                DbElement element = changes.NextModification();
                if (element == DbElement.GetElement())
                    break;
            }




            if (Saveworked == null) return;
            var sSavework = Saveworked;
            sSavework();

        }
    }
}
