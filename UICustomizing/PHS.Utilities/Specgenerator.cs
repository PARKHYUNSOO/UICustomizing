using Aveva.Pdms.Database;
using Aveva.Pdms.Shared;
using Aveva.PDMS.Database.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace PHS.Utilities
{
    class Specgenerator
    {
        public Specgenerator()
        {

        }
        static int []diameters= { 15, 25, 32, 40, 50, 65, 80, 100, 125, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950, 1000 };

        static List<int> get_dias(string diastr)
        {
            string[] size1 = diastr.Split(',');
            List<int> working_dia = new List<int>();
            foreach (string item in size1)
            {
                string result = item.Replace(" ", "");
                if (result == "")
                    continue;
                if (result.Contains("TO"))
                {
                    string[] result2 = result.Split(new string[] { "TO" }, StringSplitOptions.None);
                    int value1 = Convert.ToInt32(result2[0]);
                    int value2 = Convert.ToInt32(result2[1]);
                    int min = 0;
                    int max = 0;
                    if (value1 > value2)
                    {
                        min = value2;
                        max = value1;
                    }
                    else
                    {
                        max = value2;
                        min = value1;

                    }
                    var range_dia = from dia in diameters
                                    where dia >= min && dia <= max
                                    select dia;
                    working_dia.AddRange(range_dia);

                }
                else
                {
                    Console.WriteLine("사이즈:" + result);
                    working_dia.Add(Convert.ToInt32(result));
                }
            }
            return working_dia;
        }
        public void reculsive_copy_spec(DbElement owner,DbElement copy_from_element,string specname,List<int> diameters)
        {
            try {
                int ddepth = copy_from_element.GetInteger(DbAttributeInstance.DDEP);
                DbElementType copy_from_type = copy_from_element.GetElementType(DbAttributeInstance.TYPE);
                if (copy_from_type == DbElementTypeInstance.TEXT)
                {
                    return;                    
                }
                string copy_from_name = copy_from_element.GetAsString(DbAttributeInstance.NAME);
                string tanswer = copy_from_element.GetAsString(DbAttributeInstance.TANS);


                //if (copy_from_type == DbElementTypeInstance.SPCOMPONENT)
                    //return;
                //if (ddepth == 6)
                //    return;
                DbElement sel = null;
                if (ddepth == 3) //comp 타입 결정
                {
                    string type_name = "/" + specname + "." + tanswer;
                    try {
                        var selobj = owner.Members().Where(x => x.GetAsString(DbAttributeInstance.NAME) == type_name);

                        if (selobj.Count() == 0)
                        {
                            sel = owner.CreateLast(copy_from_type);
                            sel.SetAttribute(DbAttributeInstance.NAME, type_name);
                            sel.Copy(copy_from_element);
                        }
                        else
                        {
                            sel = selobj.First();
                        }
                        foreach (DbElement org in copy_from_element.Members())
                        {
                            reculsive_copy_spec(sel, org, specname, diameters);
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine("해당 Selec는 존재하기에 오류남");
                        Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} Selec는 존재하기에 오류남222|", type_name)).RunInPdms();
                    }
                }
                
                else
                {

                    if (ddepth == 4)
                    {
                        int size = Convert.ToInt32(copy_from_element.GetAsString(DbAttributeInstance.ANSW).Replace("mm", ""));
                        if(!diameters.Contains(size))
                            return;
                        //if (size != 32)
                        Console.WriteLine("11");
                    }
                    //sel = owner.CreateLast(DbElementTypeInstance.SELEC);
                    if (tanswer.Contains("JOINT-MITRE")|| tanswer.Contains("JOINT-THREAD"))
                    {
                        return;
                    }
                    if (copy_from_name == "/PPPG40S/HMDP_SPEC/PEPG409SS-25")
                        //return;
                        Console.WriteLine("11");
                    sel = owner.CreateLast(copy_from_type);
                    if (copy_from_type==DbElementTypeInstance.SPCOMPONENT)
                    {
                        //spco이름 세팅
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.NAME, "/"+specname+copy_from_element.GetAsString(DbAttributeInstance.CATR).Replace("-SCOM",""));
                        }catch(Exception ee)
                        {

                        }

                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.CMPR, copy_from_element.GetElement(DbAttributeInstance.CMPR));
                        }
                        catch (Exception ee) { }
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.CATR, copy_from_element.GetElement(DbAttributeInstance.CATR));
                        }
                        catch (Exception ee) { }
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.DETR, copy_from_element.GetElement(DbAttributeInstance.DETR));
                        }
                        catch (Aveva.Pdms.Utilities.Messaging.PdmsException ee) {
                        }
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.PRTREF, copy_from_element.GetElement(DbAttributeInstance.PRTREF));
                        }catch(Exception ee)
                        { }
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.TANS, copy_from_element.GetAsString(DbAttributeInstance.TANS));
                        }
                        catch (Exception ee) { }
                    }
                    else
                    {
                        sel.Copy(copy_from_element);

                    }                    

                    foreach (DbElement org in copy_from_element.Members())
                    {
                        reculsive_copy_spec(sel, org, specname, diameters);
                    }
                }
            }
            catch ( Exception ee)
            {
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} any is wrong|", "--")).RunInPdms();
            }

            
            //DbCopyOption op = new DbCopyOption();
            //sel.CopyHierarchy(copy_from_element, op);

            //reculsive_copy_spec()
        }

        public void run()
        {
            string connstr = "dsn=mis-1;uid=tbdb01;pwd=tbdb01";
            string dept = "MACH";
            string shipno = "2332";
            string query = "";
            if (dept=="MACH")
            {
                query = string.Format("select * from pdst_m where ship_no='{0}'", shipno);
            }
            else if(dept=="PIPE")
            {
                query = string.Format("select * from pdst_t_2435 where ship_no='{0}'", shipno);
            }
            else if(dept=="ACCOM")
            {

                query = string.Format("select * from pdst_a_2424 where ship_no='{0}'",shipno);
            }
            CreateSpecification(connstr, query,dept);
                    
        }

        private void CreateSpecification(string connstr, string query,string dept)
        {
            OdbcConnection conn = new OdbcConnection(connstr);
            OdbcCommand cmd = new OdbcCommand(query, conn);
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);
            da.Fill(dt);

            string comp_ref_query = "select * from STD_PDST_COMP_REF_AM";
            cmd.CommandText = comp_ref_query;
            DataTable comp_ref_dt = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(comp_ref_dt);




            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);
            DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SPWLD };
            TypeFilter filter = new TypeFilter(dbtype);
            AndFilter finalfilter = new AndFilter();

            AttributeStringFilter filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("Description"), FilterOperator.Equals, "HMD_PIPE_SPEC");


            finalfilter.Add(filter);
            finalfilter.Add(filter2);



            DBElementCollection spwl_collects = new DBElementCollection(Outfit_Elements, finalfilter);


            Console.WriteLine("스펙갯수:" + spwl_collects.Cast<DbElement>().Count());

            //dt.Rows.OfType<DataRow>().Where(x => x[1].ToString() == "WB").Count();
            //dt.Rows.OfType<DataRow>().GroupBy(x=>x[11].ToString())
            //var xx =spwl_collects.Cast<DbElement>().Where(x => x.GetAsString(DbAttributeInstance.NAME).Substring(0, 2) == "AA");
            int speccnt = 0;
            List<DbElement> working_dbelement = new List<DbElement>();


            var system_analysis = dt.Rows.OfType<DataRow>().GroupBy(x => x["LINE_NO"], x => x["NOM_DIA"], (lineno, nomdia) => new { lineno, nomdia }).OrderBy(x => x.lineno.ToString());
            int cnt = 0;
            foreach (var lineno in system_analysis)
            {
                cnt++;
                //if (cnt == 3)
                //    break;
                //스펙생성부분
                //DbElement spec_element = DbElement.GetElement("/PROJ_SPWL").CreateAfter(DbElementTypeInstance.SPECIFICATION);
                string specname = dept+"."+lineno.lineno.ToString().Replace(' ', '.');
                DbElement spec_element = null;

                if (DbElement.GetElement("/" + specname).IsNull)
                {

                    spec_element = DbElement.GetElement("/PROJ_SPWL").CreateLast(DbElementTypeInstance.SPECIFICATION);
                    spec_element.SetAttribute(DbAttributeInstance.NAME, "/" + specname);
                    spec_element.SetAttribute(DbAttributeInstance.QUES, "TYPE");
                    spec_element.SetAttribute(DbAttributeInstance.PURP, "PIPE");
                    spec_element.SetAttribute(DbAttributeInstance.DESC, "PROJ.SPEC");
                }

                try
                {
                    //SPEC TEXT 추가
                    DbElement spectext_element = spec_element.CreateLast(DbElementTypeInstance.TEXT);
                    spectext_element.SetAttribute(DbAttributeInstance.NAME, "/" + specname + "." + "PIPINGTEXT");
                    spectext_element.SetAttribute(DbAttributeInstance.STEX, "PIPING");
                    spectext_element.SetAttribute(DbAttributeInstance.RTEX, "PIPING");
                }
                catch (Exception ee) { }

                try
                {
                    foreach (var nomdia in lineno.nomdia)
                    {
                        //Aveva.Pdms.Utilities.Messaging.PdmsException.


                        Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |size: {0} |", nomdia.ToString())).RunInPdms();
                        DataRow[] targetrow = dt.Rows.OfType<DataRow>().Where(row => row["LINE_NO"].ToString() == lineno.lineno.ToString() && row["NOM_DIA"].ToString() == nomdia.ToString()).ToArray();

                        string press = targetrow[0]["PRESS"].ToString();
                        string temp = targetrow[0]["TEMP"].ToString();

                        string matlspec = targetrow[0]["MATL_SPEC"].ToString();
                        string connspec = targetrow[0]["PIPE_C_TYPE"].ToString() + "/" + targetrow[0]["PIPE_C_STD"].ToString() + "/" + targetrow[0]["PIPE_C_MATL"].ToString();
                        connspec = connspec.Replace("\n", "");
                        string valvespec = targetrow[0]["VALVE_C_TYPE"].ToString();

                        DataRow[] matl_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == matlspec).ToArray();
                        DataRow[] conn_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == connspec).ToArray();
                        DataRow[] valve_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == valvespec).ToArray();


                        List<int> diameters = get_dias(nomdia.ToString());

                        //Material Spec생성
                        if (matl_datarow.Count() == 0)
                        {
                            //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} material ref 없음|", matlspec)).RunInPdms();
                        }
                        else
                        {
                            //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                            DbElement material_spec = DbElement.GetElement(matl_datarow[0][1].ToString());
                            string[] allowtype = new string[] { "ELBO" };//
                            foreach (DbElement element in material_spec.Members())
                            {

                                if (element.GetElementType().ToString() != "TEXT")
                                {
                                    //if (!allowtype.Contains(element.GetAsString(DbAttributeInstance.TANS)))
                                    //    continue;
                                    //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                                    reculsive_copy_spec(spec_element, element, specname, diameters);

                                }
                            }
                        }
                        //Conn spec 생성
                        if (conn_datarow.Count() == 0)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} conn ref 없음|", connspec)).RunInPdms();
                        }
                        else
                        {
                            //Gasket 생성
                            DbElement gask_spec = null;
                            if (connspec.Contains("5K"))
                            {
                                gask_spec = DbElement.GetElement("/POGASASF05/HMDP_SPEC");
                            }
                            else if (connspec.Contains("10K"))
                            {
                                gask_spec = DbElement.GetElement("/POGASASF10/HMDP_SPEC");
                            }
                            else if (connspec.Contains("16K"))
                            {
                                gask_spec = DbElement.GetElement("/POGASASF16/HMDP_SPEC");
                            }
                            else if (connspec.Contains("20K"))
                            {
                                gask_spec = DbElement.GetElement("/POGASASF20/HMDP_SPEC");
                            }
                            else if (connspec.Contains("30K"))
                            {
                                gask_spec = DbElement.GetElement("/POGASASF30/HMDP_SPEC");
                            }
                            foreach (DbElement element in gask_spec.Members())
                            {
                                reculsive_copy_spec(spec_element, element, specname, diameters);

                            }
                            //Flange Spec
                            DbElement conn_spec = DbElement.GetElement(conn_datarow[0][1].ToString());
                            foreach (DbElement element in conn_spec.Members())
                            {
                                reculsive_copy_spec(spec_element, element, specname, diameters);

                            }
                        }
                        //Valve Spec생성
                        if (valve_datarow.Count() == 0)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} valve ref 없음|", valvespec).Replace("\n", "")).RunInPdms();
                        }
                        else
                        {
                            DbElement valve_spec = DbElement.GetElement(valve_datarow[0][1].ToString());
                            foreach (DbElement element in valve_spec.Members())
                            {
                                reculsive_copy_spec(spec_element, element, specname, diameters);
                            }

                        }

                        //DbElement conn_spec = DbElement.GetElement("/PFSF30/HMDP_SPEC");
                        //DbElement valve_spec = DbElement.GetElement("/PVVVV10/HMDP_SPEC");


                        //Select 생성


                    }
                }
                catch (Exception ee)
                {
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} 오륭", "11")).RunInPdms();
                    Console.WriteLine("오류:" + ee.Message);
                }
                //break;
            }
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |뻥뻥2|")).RunInPdms();
            //MDB.CurrentMDB.SaveWork("스펙생성");

            //CurrentElement.Element.Copy(DbElement.GetElement("/ACCOM.(DF)"));
            //DbElement de = CurrentElement.Element.Clone();

            //지정된 Element 뒤에 object생성하는 부분
            //DbElement xx= CurrentElement.Element.CreateAfter(DbElementTypeInstance.SPECIFICATION);            
            //xx.SetAttribute(DbAttributeInstance.NAME, "/바보야4");



            //새로운 Element를 만들고 다른object의 하위구조를 모두 복사하는 부분.

            //DbElement xx = CurrentElement.Element.CreateAfter(DbElementTypeInstance.SPECIFICATION);
            //DbCopyOption op=new DbCopyOption();            
            //op.ToName="vvd";

            ////DbElement yy= DbElement.GetElement("/ALDJSAFKL");
            //xx.CopyHierarchy(DbElement.GetElement("/ALDJSAFKL"),op);

            //DbCopyOption op = new DbCopyOption();


            // ALDJSAFKL을 복사하는데 /xxx 다음에 만들어서 복사한다.
            //DbElement yy= DbElement.GetElement("/ALDJSAFKL").CreateCopyHierarchyAfter(DbElement.GetElement("/xxx"), op);
            //테스트가 필요하네요

            //삭제
            //DbElement.GetElement("/xxx").Delete();

            //Rename
            //DbElement.GetElement("/보바").SetAttribute(DbAttributeInstance.NAME, "/보바1");

            //CurrentElement.Element.Copy(DbElement.GetElement("/보바1"));
            //MDB.CurrentMDB.SaveWork("여기는 테스트1");
            //



            //foreach (DbElement element in spwl_collects)
            //{
            //    //element.Delete();
            //    DbCopyOption dd= new DbCopyOption();

            //    //dd.ToName="/bbboa";
            //    //DbElement de= element.CreateCopyHierarchyAfter(CurrentElement.Element, dd);
            //    //de.InsertBefore(CurrentElement.Element);
            //    //DbElement xx=DbElement.GetElement("/ACCOM.(DF)");

            //    CurrentElement.Element.InsertAfterLast(xx);
            //    //de.InsertAfterLast(element.Owner);
            //    ////Console.WriteLine("스펙월드]" + spwl.GetAsString(DbAttributeInstance.NAME));
            //    //foreach (DbElement spec in spwl.Members())
            //    //{


            //    //    //spec.CreateLast(DbElementType.GetElementType("Sele"));


            //    //    string specname = spec.GetAsString(DbAttributeInstance.NAME);
            //    //    if (specname.Substring(1, 2) == "/*")
            //    //        continue;

            //    //    working_dbelement.Add(spec);
            //    //    speccnt++;
            //    //    //Console.WriteLine("   -->스펙]"+spec.GetAsString(DbAttributeInstance.NAME));
            //    //}
            //}
            //MDB.CurrentMDB.GetWork();
        }
    }
}
