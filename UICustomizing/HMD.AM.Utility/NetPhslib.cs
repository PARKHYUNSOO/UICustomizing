using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aveva.PDMS.PMLNet;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using Aveva.PDMS.Database.Filters;
using Aveva.Pdms.Shared;
using Aveva.Pdms.Database;
using Aveva.Marine.Drafting;
using Aveva.Marine.UI;
using Aveva.Marine.Utility;
using Aveva.Pdms.Utilities.CommandLine;
using Aveva.ApplicationFramework;
using Aveva.Pdms.Geometry;
/* 작성일자 :2014.5.30
 * 작성자   :박현수
 * 참조     : 
 * 목  적 : AM에서 사용할 각종 개발용 편의 Library를 만들고자함. 
 * 메소드 :
 * 1) GetDataFromOrale(query,connstr) : 오라클에서 데이터가져오는 메소드
 *          
 * 
 * 
*/
namespace Hmd.Am.Utility
{
    [PMLNetCallable()]
    public class NetPhslib
    {
        MarDrafting Kcs_draft = new MarDrafting();
        MarUi Kcs_ui = new MarUi();
        MarUtil Kcs_util = new MarUtil();
        

        DataTable result_dt = new DataTable();


        [PMLNetCallable()]
        public NetPhslib()
        {
            




            Console.WriteLine("constructor");
        }

        [PMLNetCallable()]
        public void Assign(NetPhslib obj)
        {
        }
        [PMLNetCallable()]
        public int xxxx()
        {
            return 0;
        }

        #region 1) 오라클에 쿼리실행하는 구문           
        /// <summary>
        /// AM PML단에서 SQL을 실행시키고 그 결과를 리턴시키는 메소드
        /// </summary>
        /// <param name="query">실행할 쿼리문</param>
        /// <param name="connstr">연결정보 (uid=tbdb01;pwd=tbdb01;</param>
        /// <returns></returns>
        [PMLNetCallable()]
        public double ExecuteSQL(string query, string connstr)
        {
            OdbcConnection conn = new OdbcConnection(connstr);
            
            double resultrow=0.0;
            try
            {
                conn.Open();
                OdbcCommand cmd=new OdbcCommand(query,conn);
                resultrow=cmd.ExecuteNonQuery();
                
                
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("쿼리문을 수행할수 없습니다. 쿼리검증하세요");
            }
            finally
            {
                conn.Close();
            }
            return resultrow;
        }


        #endregion




        #region 2) GetDataFromOrale(query,connstr) : 오라클에서 데이터가져오는 메소드
        /* 설명     : AM에서 Oracle의 데이터를 가져오기위해 Dataset으로 값을 담은것을
         *            Hashtable로 변경해서 Return해주는 함수임.
         * Argument :
         *            @query   : select문을 수행할 query문을 던진다.
         *            @connstr : 연결DB의 Connection String.
         * return   : Hashtable형태의 select 결과.
         * 예외처리 : 없음.
         */

        [PMLNetCallable()]
        public Hashtable GetDataFromOracle(string query,string connstr)
        {
            OdbcConnection conn=new OdbcConnection(connstr);
            //conn.Open();
            DataSet ds = new DataSet();
            try{
                

                //string query="select * from lims_cf_main where rownum<100";
                OdbcCommand cmd=new OdbcCommand(query,conn);
                OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                da.Fill(ds);

            }catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            finally{
                Console.WriteLine("문제없음 데이터 가져오는것까진");
                //conn.Close();
            }
            Hashtable hs = new Hashtable();
            try
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    Hashtable hsrow = new Hashtable();
                    for (int j = 0; j < row.ItemArray.Count(); j++)
                    {
                        hsrow.Add(j + 1, row[j].ToString());
                    }
                    hs.Add(i + 1, hsrow);

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("데이터 변환도 문제있다.옹");
            }
            return hs;
        }

    #endregion



        [PMLNetCallable()]
        public double ExecuteCmd()
        {
            try
            {
                
                //System.Diagnostics.Debugger.Launch();
                DateTime date=CurrentElement.Element.GetDateTime(DbAttributeInstance.LASTM);
                System.Windows.Forms.MessageBox.Show(date.ToShortTimeString());
                Command cmd = Command.CreateCommand("");
                cmd.CommandString = "$p parkmaker2";
                cmd.RunInPdms();
                //Collection문
                DBElementCollection collection = new DBElementCollection(CurrentElement.Element);
                collection.IncludeRoot = true;
                Kcs_ui.MessageNoConfirm("멍멍1");
                Kcs_ui.MessageNoConfirm(CurrentElement.Element.Owner.GetAsString(DbAttributeInstance.NAME));
                //collection.Filter = new TypeFilter(DbElementTypeInstance.DEPT);
                foreach (DbElement entry in collection)
                {
                    
                    Kcs_ui.MessageNoConfirm(entry.GetAsString(DbAttributeInstance.NAME));


                }

                //PML Command실행.
                
            }
            catch (Exception ee)
            {
                Console.WriteLine("오류발생");
            }
            return 10.0;
        }

        [PMLNetCallable()]
        public void dwgopen()
        {
            try
            {
                Kcs_ui.MessageNoConfirm(CurrentElement.Element.GetAsString(DbAttributeInstance.NAME));

                if (Kcs_draft.DwgCurrent())
                {
                    string title = "도면저장";
                    string titleMsg = "현재 도면을 저장하시겠습니까?";
                    if (Kcs_ui.AnswerReq(title, titleMsg) == Kcs_util.Yes())
                    {
                        try
                        {
                            Kcs_util.CleanWorkspace();
                            Kcs_draft.DwgPurge();
                            Kcs_draft.DwgPack();
                            Kcs_draft.DwgSave();
                        }
                        catch (MarVitesseException e)
                        {
                            Kcs_ui.MessageNoConfirm(e.ToString());
                        }
                    }
                }


                Kcs_draft.DwgClose();
                //사실 DWGOPEN은 SHEE를 여는것이다.
                Kcs_draft.DwgOpen(CurrentElement.Element.GetAsString(DbAttributeInstance.NAME));
                System.GC.Collect();
                System.GC.WaitForFullGCComplete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// AM에서 전달된 텍스트를 Clipboard에 저장하는 함수
        /// </summary>
        /// <param name="text">클립보드에 저장될 텍스트</param>
        [PMLNetCallable()]
        public void SetClipboard(string text)
        {
            System.Windows.Forms.Clipboard.SetText(text);
        }

        /// <summary>
        ///  2중 Array형태로 입력값을 받으면 그중에서 특정 index의 값을 기준으로 정렬하는 메소드
        /// </summary>
        /// <param name="sourcehash">PML의 Array[][] </param>
        /// <param name="columnidx"> 정렬할 level2의 Column 인덱스</param>
        /// <param name="mode">true일 경우 오름차순, false일 경우 내림차순</param>
        /// <returns></returns>
        [PMLNetCallable()]
        public Hashtable ArraySort2by2(Hashtable sourcehash, double columnidx, Boolean mode)
        {
            //Hash Table의 크기에 맞춰 컬럼생성
            
            DataTable dt = new DataTable();

            Hashtable samplerow= ((Hashtable)sourcehash[1.0]);
            int columnsize = samplerow.Count;
            
            for (int i = 0; i < columnsize; i++)
            {
                Type columntype=samplerow[Convert.ToDouble(i+1)].GetType();
                dt.Columns.Add("col" + (i + 1).ToString(),columntype);
                
            }
            
            //데이터 테이블로 넣는다.
            foreach (var key in sourcehash.Keys)
            {

                Hashtable hs2 = (Hashtable)sourcehash[key];
                ArrayList ar = new ArrayList();


                for (double i=1.0; i <= hs2.Keys.Count;i=i+1 )
                {
                    //ar[columnkey]=hs2[columnkey].ToString();
                    ar.Add(hs2[i]);
                }
                    //foreach (var columnkey in hs2.Keys)
                    //{
                    //    //ar[columnkey]=hs2[columnkey].ToString();
                    //    ar.Add(hs2[columnkey]);
                    //}

                    dt.Rows.Add(ar.ToArray());
                //Hashtable hhs2=(Hashtable)hs[h1];
                //foreach(int h2 in hhs2.Keys)
                //{
                Console.WriteLine(dt);
                //}
            }
            

            int idx = Convert.ToInt32(columnidx);
            dt.DefaultView.Sort = "col"+idx.ToString() + " ASC";
            dt = dt.DefaultView.ToTable(false);

            
            Hashtable resultHs = new Hashtable();
            //Datatable -> HashTable
            for (int i = 1; i < dt.Rows.Count + 1; i++)
            {
                Hashtable resultsubHs = new Hashtable();
                for (int j = 1; j < dt.Columns.Count + 1; j++)
                {
                    resultsubHs.Add((double)j, dt.Rows[i-1][j - 1]);
                }
                resultHs.Add((double)i, resultsubHs);
            }
            Console.WriteLine(resultHs);
            return resultHs;

        }
        /// <summary>
        /// 2중배열로 데이터를 전달하면 특정 컬럼의 값을 기준으로 그룹핑하여 3중배열로 리턴하는 메소드
        /// </summary>
        /// <param name="sourcehash">서브그루핑의 원천데이터</param>
        /// <param name="keyColIdx">서브그루핑할 열의 인덱스번호</param>
        /// <returns></returns>
        [PMLNetCallable()]
        public Hashtable SubGroupingbyColumn(Hashtable sourcehash, double keyColIdx)
        {
            //Hash Table의 크기에 맞춰 컬럼생성

            DataTable dt = new DataTable();

            Hashtable samplerow = ((Hashtable)sourcehash[1.0]);
            int columnsize = samplerow.Count;

            for (int i = 0; i < columnsize; i++)
            {
                Type columntype = samplerow[Convert.ToDouble(i + 1)].GetType();
                dt.Columns.Add("col" + (i + 1).ToString(), columntype);
            }

            //데이터 테이블로 넣는다.
            foreach (double key in sourcehash.Keys.Cast<double>().OrderBy(T=>T))
            {

                Hashtable hs2 = (Hashtable)sourcehash[key];
                ArrayList ar = new ArrayList();


                foreach (var columnkey in hs2.Keys.Cast<double>().OrderBy(T => T))
                {
                    //ar[columnkey]=hs2[columnkey].ToString();
                    ar.Add(hs2[columnkey]);
                }

                dt.Rows.Add(ar.ToArray());
                
                Console.WriteLine(dt);                
            }
            //int idx = Convert.ToInt32(columnidx);
            //dt.DefaultView.Sort = "col" + idx.ToString() + " ASC";
            dt = dt.DefaultView.ToTable(false);



            var groupset=dt.AsEnumerable().GroupBy(r => r[Convert.ToInt32(keyColIdx)]).Select(g => g);

            Hashtable resultHs = new Hashtable();
            //Datatable -> HashTable
            var resultlist=groupset.ToList();
            for (int i = 1; i <= resultlist.Count ; i++)
            {
                Hashtable resultsubHs = new Hashtable();
                resultsubHs.Add(1.0,resultlist[i-1].Key);
                var resultlist2 = resultlist[i - 1].ToArray();
                for (int j =1; j <= resultlist2.Count() ; j++)
                {
                    
                    Hashtable resultsub2Hs = new Hashtable();
                    for (int k = 1; k < resultlist2[j-1].ItemArray.Count()+1; k++)
                    {
                        resultsub2Hs.Add((double)k, resultlist2[j-1][k-1]);
                    }

                    resultsubHs.Add((double)j+1, resultsub2Hs);
                }
                resultHs.Add((double)i, resultsubHs);
            }
            Console.WriteLine(resultHs);
            return resultHs;

        }

        [PMLNetCallable()]
        public string Rotate3D(string direction1, string direction2,double angle)
        {
            Direction dir = Direction.Create(direction1);
            Direction basedir = Direction.Create(direction2);
            dir.RotateAbout(basedir, angle);
           
            return dir.ToString();
        }

        [PMLNetCallable()]
        public Hashtable GetVolumeWithinElement(string elementname, Hashtable dbtypeArray, Boolean completelycover)
        {
            
            //DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.PSPOOL };
            string samplerow = ((string)dbtypeArray[1.0]);

            List<DbElementType> dbtypeslist = new List<DbElementType>();
            foreach (var item in dbtypeArray.Keys)
	        {
		        dbtypeslist.Add(DbElementType.GetElementType(dbtypeArray[item].ToString()));
	        }
            DbElementType[] dbtypes = dbtypeslist.ToArray();
            
                
            

            
            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);                        
            DBElementCollection result_collection = null;
            TypeFilter filter = new TypeFilter(dbtypes);
            AndFilter finalfilter = new AndFilter();

            InVolumeFilter volfilter = new InVolumeFilter(CurrentElement.Element,dbtypes, true);
            
            
            finalfilter.Add(filter);
            finalfilter.Add(volfilter);
            result_collection = new DBElementCollection(Outfit_Elements, finalfilter);
            
            Hashtable resultHs = new Hashtable();

            double idx = 1.0;
            foreach (DbElement item in result_collection)
            {
                resultHs.Add(idx, item.GetAsString(DbAttributeInstance.NAME));
                idx = idx + 1.0;
            }
          
          
            return resultHs;
        }
        [PMLNetCallable()]
        public Hashtable ExtractClashItems(Hashtable ClashItems)
        {
            Dictionary<string, List<string>> piecelist = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> strulist = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> equiplist = new Dictionary<string, List<string>>();

            List<Hashtable> ClashItemList = new List<Hashtable>();
            Hashtable clashHash = new Hashtable();
            
            //var xx = ClashItems.OfType<Hashtable>().GroupBy(x => x[1.0]).Select(x=>x);
            foreach (var idx in ClashItems.Keys)
            {
                //clashHash.Add()
                Hashtable tmphash = (Hashtable)ClashItems[idx];
                string key=tmphash[1.0].ToString();
                List<string> tmplist=tmphash.Values.OfType<string>().ToList();
                if( clashHash.ContainsKey(key))
                {
                    ((List<List<string>>)clashHash[key]).Add(tmplist);
                }
                else{
                    List<List<string>> values = new List<List<string>>();
                    values.Add(tmplist);
                    clashHash.Add(key,values);
                }
                
                //List<string> values = new List<string>();

                //ClashItemList.Add((Hashtable)ClashItems[idx]);
            }


            foreach (string key in clashHash.Keys)
            {
                foreach (var item in ((List<List<string>>)clashHash[key]))
                {
                    try
                    {
                        if (DbElement.GetElement(item[0]).Owner.GetAsString(DbAttributeInstance.TYPE) == "BRAN")
                        {
                            string piecename = DbElement.GetElement(item[0]).GetAsString(DbAttributeInstance.PCRFA);

                            if (piecelist.Keys.Contains(piecename))
                            {

                                if (!piecelist[piecename].Contains(item[4]))
                                    piecelist[piecename].Add(item[4]);
                            }
                            else
                            {
                                List<string> values = new List<string>();
                                values.Add(item[4]);
                                piecelist.Add(piecename, values);

                            }
                        }
                        else if (DbElement.GetElement(item[0]).Owner.GetAsString(DbAttributeInstance.TYPE) == "FRMW")
                        {
                            string struname=DbElement.GetElement(item[0]).Owner.Owner.GetAsString(DbAttributeInstance.NAME);
                            if (strulist.Keys.Contains(struname))
                            {

                                if (!strulist[struname].Contains(item[4]))
                                    strulist[struname].Add(item[4]);
                            }
                            else
                            {
                                List<string> values = new List<string>();
                                values.Add(item[4]);
                                strulist.Add(struname, values);

                            }
                        }
                        //else if(DbElement.GetElement(item[0]).Owner.GetAsString(DbAttributeInstance.TYPE) == "EQUI")
                        //{
                        //    string equipname = DbElement.GetElement(item[0]).Owner.GetAsString(DbAttributeInstance.NAME);
                        //    if (equiplist.Keys.Contains(equipname))
                        //    {

                        //        if (!equiplist[equipname].Contains(item[4]))
                        //            equiplist[equipname].Add(item[4]);
                        //    }
                        //    else
                        //    {
                        //        List<string> values = new List<string>();
                        //        values.Add(item[4]);
                        //        equiplist.Add(equipname, values);

                        //    }

                        //}
                    }catch(Exception ee)
                    {

                    }
                }
            }
            

            Hashtable resultHs = new Hashtable();
            //Datatable -> HashTable
            double i=1.0;
            foreach (string piecename in piecelist.Keys)
            {
                
                Hashtable resultsubHs = new Hashtable();
                resultsubHs.Add(1.0, piecename);
                for (int j = 1; j <= piecelist[piecename].Count; j++)
                {
                    resultsubHs.Add((double)j+1, piecelist[piecename][j - 1]);
                }
                resultHs.Add((double)i, resultsubHs);
                i= i+1;
            }
            
            foreach (string struname in strulist.Keys)
            {

                Hashtable resultsubHs = new Hashtable();
                resultsubHs.Add(1.0, struname);
                for (int j = 1; j <= strulist[struname].Count; j++)
                {
                    resultsubHs.Add((double)j + 1, strulist[struname][j - 1]);
                }
                resultHs.Add((double)i, resultsubHs);
                i = i + 1;
            }
            
            //foreach (string equiname in equiplist.Keys)
            //{

            //    Hashtable resultsubHs = new Hashtable();
            //    resultsubHs.Add(1.0, equiname);
            //    for (int j = 1; j <= equiplist[equiname].Count; j++)
            //    {
            //        resultsubHs.Add((double)j + 1, equiplist[equiname][j - 1]);
            //    }
            //    resultHs.Add((double)i, resultsubHs);
            //    i = i + 1;
            //}

            Console.WriteLine(resultHs);
            return resultHs;
        }

        //기장설계의 경우에는 후처리코드, 페인트코드 , 인슐레이션정보를 테이블이 아님 이 메소드내에서 처리해서 결과를 리턴시킴.
        [PMLNetCallable()]
        public Hashtable GetTreatInfos(string module,string system,double bore,string treatment, string matl)
        {
            Hashtable resultHs=new Hashtable();
            string [] treatinfo=new string[]{};
            if (treatment=="-")
                treatment="";
            treatment=treatment.Trim();

            ///1단계적용
            if (treatment == "GAL")
            {
                treatinfo = new string[] { "AG","AG", "---","---", "0" };
                
            }

            if (treatment == "PE")
            {
                treatinfo = new string[] { "BC","BP", "---","6KP", "0" };
            }
            if (treatment == "ALUMINIZED")
            {
                treatinfo = new string[] { "---","---", "---","---", "0" };
            }
            if (matl == "SUS")
            {
                treatinfo = new string[] { "PA","PA", "---","---", "0" };
            }
            if (treatment == "COPPER")
            {
                if (treatment == "INS")
                {
                    treatinfo = new string[] { "NO","NO", "---","---", "1" };
                }
                else
                {
                    treatinfo = new string[] { "NO","NO", "---","---", "0" };
                }
            }

            if(treatinfo.Length!=0 )
            {
                if(treatinfo[3]=="6KP")
                {
                    string munit= module.Substring(0,2);
                    if(munit=="M1")
                    {
                        treatinfo=new string[]{treatinfo[0],treatinfo[1].Substring(0,4)+"6KH",treatinfo[2]};
                    }else if(munit=="M2" || munit=="M3" || munit=="MA" || munit=="2E" ) {
                        treatinfo=new string[]{treatinfo[0],treatinfo[1].Substring(0,4)+"6KF",treatinfo[2]};
                    }
                }
                if(module.Substring(0,1)=="E" && module.Substring(module.Length-1,1)=="6")
                {
                    if(bore>=125)
                        treatinfo = new string[] { "BP","BP", "6KS","6KH", "0" };
                    else
                        treatinfo = new string[] { "AG","AG", "---","---", "0" };
                }

                //결과 List를 Hashtable로 변환해서 리턴시킴                
                for (int i = 1; i <= treatinfo.Length; i++)
                {
                    resultHs.Add((double)i, treatinfo[i-1]);
                }

                    return resultHs;
             }

            ///2 단계적용
            string [] oilsys=new string []{"FA","FC","FG","FH","FI","FN","FS","FV","LK"};
            string [] watersys = new string[] { "TA", "TB", "TE", "TH", "TR", "WE", "TU", "BP" };
            string [] exhgassys = new string[] { "XS", "XL" };
            if (oilsys.Contains(system))
            {
                if (treatment == "INS & TRAC")
                {
                    treatinfo = new string[] { "AA","AP", "---","6JY", "1" };
                }
                if (treatment == "INS / INST & ELEC.TRAC." && system == "LK")
                {
                    treatinfo = new string[] { "AA","AP", "---","6JY", "1" };
                }

            }
            if (watersys.Contains(system))
            {
                if (treatment == "INS"  )                
                    treatinfo = new string[] { "AH","AP", "---","6JY", "1" };                

                if (system == "TU" && treatment == "")
                    treatinfo = new string[] { "AH","AP", "---","6JY", "0" };

                if (system=="BP" && treatment=="INS. & TRAC. GAL")                
                    treatinfo = new string[] { "AG","GP", "---","6KV", "1" };                
            }
            if(exhgassys.Contains(system) && treatment=="INS")            
            {
                treatinfo = new string[] { "AH","BP", "---","6JB", "1" };                
            }
            if(treatinfo.Length!=0 )
            {
                if(treatinfo[3]=="6KP")
                {
                    string munit= module.Substring(0,2);
                    if(munit=="M1")
                    {
                        treatinfo=new string[]{treatinfo[0],treatinfo[1].Substring(0,4)+"6KH",treatinfo[2]};
                    }else if(munit=="M2" || munit=="M3" || munit=="MA" || munit=="2E" ) {
                        treatinfo=new string[]{treatinfo[0],treatinfo[1].Substring(0,4)+"6KF",treatinfo[2]};
                    }
                }
                if(module.Substring(0,1)=="E" && module.Substring(module.Length-1,1)=="6")
                {
                    if(bore>=125)
                        treatinfo = new string[] { "BP","BP", "6KS","6KH", "0" };
                    else
                        treatinfo = new string[] { "AG","AG", "---","---", "0" };
                }

                //결과 List를 Hashtable로 변환해서 리턴시킴                
                for (int i = 1; i <= treatinfo.Length; i++)
                {
                    resultHs.Add((double)i, treatinfo[i-1]);
                }

                    return resultHs;
             }




            /// 3단계적용
            string initsys=system.Substring(0,1);
            if(initsys=="L" || initsys=="F" || initsys=="H")
                treatinfo = new string[] { "AA","AP", "---","6KP", "0" };
            if (initsys == "T" || initsys == "W" || initsys == "D" || initsys == "B")
                treatinfo = new string[] { "AH","AP", "---","6KP", "0" };
            if(initsys=="V" || initsys=="A" || initsys=="I" || initsys=="X")
                treatinfo = new string[] { "AH","AP", "---","6KP", "0" };
                        if(treatinfo.Length!=0 )
            {
                if(treatinfo[3]=="6KP")
                {
                    string munit= module.Substring(0,2);
                    if(munit=="M1")
                    {
                        treatinfo=new string[]{treatinfo[0],treatinfo[1],treatinfo[2],"6KH",treatinfo[4]};
                    }else if(munit=="M2" || munit=="M3" || munit=="MA" || munit=="2E" ) {
                        treatinfo = new string[] { treatinfo[0], treatinfo[1], treatinfo[2], "6KF", treatinfo[4] };
                    }
                }
                if(module.Substring(0,1)=="E" && module.Substring(module.Length-1,1)=="6")
                {
                    if(bore>=125)
                        treatinfo = new string[] { "BP","BP", "6KS","6KH", "0" };
                    else
                        treatinfo = new string[] { "AG","AG", "---","---", "0" };
                }

                //결과 List를 Hashtable로 변환해서 리턴시킴                
                for (int i = 1; i <= treatinfo.Length; i++)
                {
                    resultHs.Add((double)i, treatinfo[i-1]);
                }

                    return resultHs;
             }
        
            return resultHs;
        }


        //선번과 Pipe이름으로 Line No를 체크해서 PDST상의 유일한 행을 배열로 반환시킴
        [PMLNetCallable()]
        public Hashtable GetPDSTRowFromPipeName(string spoolname,string shipno)
        {
            string mdb = MDB.CurrentMDB.Name;
            string connstr = "dsn=mis-1;uid=tbdb01;pwd=tbdb01";
            OdbcConnection conn = new OdbcConnection(connstr);
            OdbcDataAdapter da = new OdbcDataAdapter();
            string query="";
            if (mdb.Substring(0,1)=="M")
                query = string.Format(@"select A.*,B.MATERIAL 
                                        from pdst_M A , STD_PDST_COMP_REF_AM B 
                                        where A.ship_no='{0}' AND A.MATL_SPEC=B.determine_args(+)
                                        ", shipno);
            else if(mdb.Substring(0,1)=="T")
                query = string.Format(@"select A.*,B.MATERIAL 
                                        from PDST_PIPE A , STD_PDST_COMP_REF_AM B 
                                        where A.ship_no='{0}' AND A.MATL_SPEC=B.determine_args(+)
                                        ", shipno);
            else if (mdb.Substring(0, 1) == "A")
                query = string.Format(@"select A.*,B.MATERIAL 
                                        from PDST_ACCM A , STD_PDST_COMP_REF_AM B 
                                        where A.ship_no='{0}' AND A.MATL_SPEC=B.determine_args(+)
                                        ", shipno);


            OdbcCommand cmd = new OdbcCommand(query, conn);
            da.SelectCommand = cmd;
            result_dt = new DataTable();
            da.Fill(result_dt);

            string[] namesplit = spoolname.Split('-');
            string module = namesplit[0];
            string line = namesplit[1];
            string spoolno = namesplit[2];
            string system = line.Substring(0, 2);
            int lineno = Convert.ToInt32(line.Substring(2));

            List<DataRow> resultlist = new List<DataRow>();
            Hashtable resultHs = new Hashtable();


            foreach (DataRow row in result_dt.Rows)
            {
                try
                {
                    string linestr = row["LINE_NO"].ToString();
                    linestr = linestr.Trim();
                    string[] splitarr = linestr.Split(new char[] { ' ' }, 2);
                    string pdst_sys = splitarr[0];
                    string[] pdst_linenos = new string[] { };
                    //라인넘버 존재하지 않고 그냥 EB 이렇게 정의되는 경우 1-9999로 정의함.
                    if (splitarr.Length == 1)
                    {
                        pdst_linenos = new string[] { "1-99999" };
                    }
                    else
                    {
                        pdst_linenos = splitarr[1].Split(new char[] { ',' });
                    }
                    
                    if (pdst_sys == system)
                    {
                        int start_no = 0;
                        int end_no = 0;
                        bool includedLineNo = false;
                        foreach (string pdst_lineno in pdst_linenos)
                        {
                            string[] pdst_linenoarr = pdst_lineno.Split(new string[] { "-" }, StringSplitOptions.None);
                            if (pdst_linenoarr.Length == 1)
                            {
                                start_no = Convert.ToInt32(pdst_linenoarr[0]);
                                end_no = Convert.ToInt32(pdst_linenoarr[0]);
                            }
                            else
                            {
                                start_no = Convert.ToInt32(pdst_linenoarr[0]);
                                end_no = Convert.ToInt32(pdst_linenoarr[1]);
                            }
                            Console.WriteLine("22");

                            if (lineno >= start_no && lineno <= end_no)
                            {
                                includedLineNo = true;
                                break;
                            }
                        }
                        if (includedLineNo)
                        {
                            resultlist.Add(row);

                        }
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine("1");

                }

            }

            //결과 List를 Hashtable로 변환해서 리턴시킴

            for (int i = 1; i <= resultlist.Count; i++)
            {

                Hashtable resultsubHs = new Hashtable();
                for (int j = 1; j <= result_dt.Columns.Count; j++)
                {
                    resultsubHs.Add((double)j, resultlist[i - 1][j - 1].ToString());
                }
                resultHs.Add((double)i, resultsubHs);
            }
            return resultHs;
            

        }



        //아직 미구현... 사이즈체크부분 안함. 선번과 pipe이름으로 Line No, Size를 체크해서 PDST상의 유일한 행을 배열로 반환시킴
        [PMLNetCallable()]
        public Hashtable GetPDSTRowFromPipeNameCheckSize(string spoolname, string shipno)
        {
            string connstr = "dsn=mis-1;uid=tbdb01;pwd=tbdb01";
            OdbcConnection conn = new OdbcConnection(connstr);
            OdbcDataAdapter da = new OdbcDataAdapter();
            string query = string.Format(@"select A.*,B.MATERIAL 
                                        from PDST_PIPE A , STD_PDST_COMP_REF_AM B 
                                        where A.ship_no='{0}' AND A.MATL_SPEC=B.determine_args(+)
                                        ", shipno);
            OdbcCommand cmd = new OdbcCommand(query, conn);
            da.SelectCommand = cmd;
            result_dt = new DataTable();
            da.Fill(result_dt);

            string[] namesplit = spoolname.Split('-');
            string module = namesplit[0];
            string line = namesplit[1];
            string spoolno = namesplit[2];
            string system = line.Substring(0, 2);
            int lineno = Convert.ToInt32(line.Substring(2));

            List<DataRow> resultlist = new List<DataRow>();
            Hashtable resultHs = new Hashtable();


            foreach (DataRow row in result_dt.Rows)
            {
                try
                {
                    string linestr = row["LINE_NO"].ToString();
                    linestr = linestr.Trim();
                    string[] splitarr = linestr.Split(new char[] { ' ' }, 2);
                    string pdst_sys = splitarr[0];
                    string[] pdst_linenos = new string[] { };
                    //라인넘버 존재하지 않고 그냥 EB 이렇게 정의되는 경우 1-9999로 정의함.
                    if (splitarr.Length == 1)
                    {
                        pdst_linenos = new string[] { "1-99999" };
                    }
                    else
                    {
                        pdst_linenos = splitarr[1].Split(new char[] { ',' });
                    }

                    if (pdst_sys == system)
                    {
                        int start_no = 0;
                        int end_no = 0;
                        bool includedLineNo = false;
                        foreach (string pdst_lineno in pdst_linenos)
                        {
                            string[] pdst_linenoarr = pdst_lineno.Split(new string[] { "-" }, StringSplitOptions.None);
                            if (pdst_linenoarr.Length == 1)
                            {
                                start_no = Convert.ToInt32(pdst_linenoarr[0]);
                                end_no = Convert.ToInt32(pdst_linenoarr[0]);
                            }
                            else
                            {
                                start_no = Convert.ToInt32(pdst_linenoarr[0]);
                                end_no = Convert.ToInt32(pdst_linenoarr[1]);
                            }
                            Console.WriteLine("22");

                            if (lineno >= start_no && lineno <= end_no)
                            {
                                includedLineNo = true;
                                break;
                            }
                        }
                        if (includedLineNo)
                        {
                            resultlist.Add(row);

                        }
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine("1");

                }

            }

            //결과 List를 Hashtable로 변환해서 리턴시킴

            for (int i = 1; i <= resultlist.Count; i++)
            {

                Hashtable resultsubHs = new Hashtable();
                for (int j = 1; j <= result_dt.Columns.Count; j++)
                {
                    resultsubHs.Add((double)j, resultlist[i - 1][j - 1].ToString());
                }
                resultHs.Add((double)i, resultsubHs);
            }
            return resultHs;


        }
        //해당스풀에서 가장 긴 튜브의 refno, bore를 가져온다.
        [PMLNetCallable()]
        public Hashtable GetPipeSpoolLongestBore(string spoolname)
        {
            Hashtable hs = new Hashtable();
            DbElement element = DbElement.GetElement("/" + spoolname);
            DbElement [] partlist=element.GetElementArray(DbAttributeInstance.BELRFA);
            var maxlength = partlist.Cast<DbElement>().Where(data => data.GetAsString(DbAttributeInstance.TYPE) == "TUBI").Max(b => b.GetDouble(DbAttributeInstance.LTLE, 1));
            var longesttub = partlist.Cast<DbElement>().
                Where(data => data.GetAsString(DbAttributeInstance.TYPE) == "TUBI" && data.GetDouble(DbAttributeInstance.LTLE, 2) == maxlength);
                    //data.GetDouble(DbAttributeInstance.LTLE) == partlist.Max(b => b.GetDouble(DbAttributeInstance.LTLE, 0)));
            hs.Add(1.0, longesttub.First().ToString());
            hs.Add(2.0, maxlength);
            return hs;
        }


        /// <summary>
        /// 전달받은 스풀에서 사용된 Bore List를 반환하는 메소드임
        /// </summary>
        /// <param name="spoolname"></param>
        /// <returns>사용된 보어리스트</returns>
        [PMLNetCallable()]
        public Hashtable GetPipeSpoolBores(string spoolname)
        {
            Hashtable hs = new Hashtable();
            DbElement element = DbElement.GetElement("/" + spoolname);
            DbElement[] partlist = element.GetElementArray(DbAttributeInstance.BELRFA);

            var borelist = partlist.Cast<DbElement>().
                Where(data => data.GetAsString(DbAttributeInstance.TYPE) == "TUBI").Select(x => x.GetDouble(DbAttributeInstance.LBOR)).Distinct();
            //data.GetDouble(DbAttributeInstance.LTLE) == partlist.Max(b => b.GetDouble(DbAttributeInstance.LTLE, 0)));


            double idx=1.0;
            foreach (var bore in borelist)
            {
                hs.Add(idx, bore);
                idx += 1;
            }

            
            return hs;
        }

        [PMLNetCallable()]
        public  Hashtable GetPaintCode(string shipno,string itemtype, string area,string system,string lineno,string matl)
        {
            Hashtable hs = new Hashtable();
            result_dt = new DataTable();
            string connstr = "dsn=mis-1;uid=tbdb01;pwd=tbdb01";
            OdbcConnection conn = new OdbcConnection(connstr);
            OdbcDataAdapter da = new OdbcDataAdapter();
            string query = string.Format(@"select * from AM_PAINT_REF WHERE SHIP_NO='{0}'", shipno);
            OdbcCommand cmd = new OdbcCommand(query, conn);
            da.SelectCommand = cmd;
            da.Fill(result_dt);

            DataRow resultrow = null;

            if (itemtype == "PIPE")
            {

                var matchresult = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["MATERIAL"].ToString() == matl && r["SYSTEM"].ToString() == system).ToArray();
                var matchAreaMatl = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["MATERIAL"].ToString() == matl).ToArray();

                if (matchresult.Count() > 0)    //구역,재질, 시스템이 일치하는 경우
                {
                    bool haslineno = false;
                    foreach (DataRow row in matchresult)
                    {
                        string paint_lineno = row["LINE_NO"].ToString();
                        if (paint_lineno != "-")
                        {

                            string[] splitLineNo = paint_lineno.Split(new char[] { ',' });

                            foreach (string eachlineno in splitLineNo)
                            {
                                int startno = 0;
                                int endno = 0;

                                string[] eachlinesnos = eachlineno.Trim().Split(new char[] { '-' });
                                if (eachlinesnos.Length == 1)
                                {
                                    startno = Convert.ToInt32(eachlinesnos[0]);
                                    endno = Convert.ToInt32(eachlinesnos[0]);
                                }
                                else
                                {
                                    startno = Convert.ToInt32(eachlinesnos[0]);
                                    endno = Convert.ToInt32(eachlinesnos[1]);

                                }
                                if (Convert.ToInt32(lineno) >= startno && Convert.ToInt32(lineno) <= endno)
                                {
                                    haslineno = true;
                                    resultrow = row;

                                    break;
                                }

                            }
                            if (haslineno)
                                break;
                        }
                    }
                    if (haslineno == false)
                    {
                        var matchresult2 = matchresult.Where(r => r["LINE_NO"].ToString() == "-");
                        if (matchresult2.Count() > 0)
                            resultrow = matchresult2.First();
                        else
                            return hs;
                    }

                }
                else if (matchresult.Length == 0 && matchAreaMatl.Length > 0)//구역,재질만 일치하는 경우
                {
                    try
                    {
                        resultrow = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["MATERIAL"].ToString() == matl && r["SYSTEM"].ToString() == "ALL").First();
                    }
                    catch (Exception ee)
                    {
                        return hs;
                    }
                }
                else
                {
                    try
                    {
                        resultrow = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["MATERIAL"].ToString() == matl).First();
                    }
                    catch (Exception ee)
                    {
                        return hs;
                    }
                }
            }else if(itemtype=="SUPPORT")
            {
                var matchresult = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["ITEM_TYPE"].ToString()==itemtype &&  r["SYSTEM"].ToString() == system).ToArray();
                if (matchresult.Length > 0)
                    resultrow = matchresult[0];
                else
                    return hs;

            }else if(itemtype=="OUTFITTING")
            {
                var matchresult = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["ITEM_TYPE"].ToString() == itemtype &&
                     r["SYSTEM"].ToString() == system).ToArray();
                if (matchresult.Length > 0)
                    resultrow = matchresult[0];
                else
                {
                    var matchresult2 = result_dt.Rows.Cast<DataRow>().Where(r => r["AREA"].ToString() == area && r["ITEM_TYPE"].ToString() == itemtype &&
                     r["SYSTEM"].ToString() == "ALL").ToArray();
                    if (matchresult2.Length > 0)
                        resultrow = matchresult[0];
                    else
                        return hs;
                }
            }
            
            //결과 List를 Hashtable로 변환해서 리턴시킴
            Console.WriteLine("1");
            for (int i = 1; i <= resultrow.ItemArray.Length; i++)
            {

                hs.Add((double)i, resultrow[i - 1].ToString());
            }
            

            return hs;
        }

        //$s aa=$m /q:env\pmllib\outfitting\test\clashtest.mac
        //add /PARK
        //ADD PIPE 1 of ZONE /TEST
    }
}

