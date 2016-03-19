using Aveva.Pdms.Database;
using Aveva.Pdms.Shared;
using Aveva.PDMS.Database.Filters;
using PHS.Utilities.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities
{
    public partial class SpecGen_Form : Form
    {
        public SpecGen_Form()
        {
            InitializeComponent();
        }
        static int[] diameters = { 4, 5, 6, 8, 10, 12, 15, 16, 18, 20, 22, 25, 26, 30, 32, 35, 40, 50, 60, 63 ,65, 70, 75, 80, 90, 100, 110, 125, 150, 160, 200, 250, 300, 315, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950, 1000, 1100, 1200, 1300, 1400, 1500 };
        string[] ecp_system = new string[] { "EB", "EH", "EM" };
        private void btn_createspec_Click(object sender, EventArgs e)
        {
            string connstr = "dsn=mis-1;uid=tbdb01;pwd=tbdb01";
            string dept = "MACH";
            string shipno = "2332";

            dept = cb_dept.Text;
            shipno = txtshipno.Text;

            string query = "";
            if (dept == "MACH")
            {
                query = string.Format("select * from pdst_m where ship_no='{0}' UNION SELECT * FROM PDST_M WHERE SHIP_NO='*'", shipno);
            }
            else if (dept == "PIPE")
            {
                query = string.Format("select * from PDST_PIPE where ship_no='{0}' UNION SELECT * FROM PDST_PIPE WHERE SHIP_NO='*'", shipno);
            }
            else if (dept == "ACCOM")
            {

                query = string.Format("select * from PDST_ACCM where ship_no='{0}' UNION SELECT * FROM PDST_ACCM WHERE SHIP_NO='*'", shipno);
            }

            splashScreenManager1.ShowWaitForm();
            CreateSpecification(connstr, query, dept);
            splashScreenManager1.CloseWaitForm();
        }
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
                    IEnumerable<int> range_dia = from dia in diameters
                                                 where dia >= min && dia <= max
                                                 select dia;

                    working_dia.AddRange(range_dia);
                    //Non-Standard Branch의 Bore 사이즈가 10으로 해놔서 무조건 10mm를 추가하기로 한다.
                    working_dia.Add(10);
                    working_dia = working_dia.Distinct().ToList();


                }
                else
                {
                    Console.WriteLine("사이즈:" + result);
                    working_dia.Add(10);
                    working_dia.Add(Convert.ToInt32(result));
                }
            }
            return working_dia;
        }
        public void reculsive_copy_spec(DbElement owner, DbElement copy_from_element, string specname, List<int> diameters)
        {
            try
            {
                int ddepth = copy_from_element.GetInteger(DbAttributeInstance.DDEP);
                DbElementType copy_from_type = copy_from_element.GetElementType(DbAttributeInstance.TYPE);
                if (copy_from_type == DbElementTypeInstance.TEXT)
                {
                    return;
                }
                string copy_from_name = copy_from_element.GetAsString(DbAttributeInstance.NAME);
                
                string tanswer = copy_from_element.GetAsString(DbAttributeInstance.TANS).Trim();
                
                string answer = copy_from_element.GetAsString(DbAttributeInstance.ANSW);
                string question = "";
                string qualifier = "";
                if (copy_from_type == DbElementTypeInstance.SELEC)
                {
                    question = copy_from_element.GetAsString(DbAttributeInstance.QUES);
                    qualifier = copy_from_element.GetAsString(DbAttributeInstance.QUAL);
                }


                //if (copy_from_type == DbElementTypeInstance.SPCOMPONENT)
                //return;
                //if (ddepth == 6)
                //    return;
                DbElement sel = null;
                if (ddepth == 3) //comp 타입 결정
                {
                    if (tanswer == "GASK" && copy_from_name.Substring(0, 6) != "/POORG" && specname != "PIPE.SP" && specname != "MACH.SP" && specname != "ACCOM.UR" && specname != "ACCOM.PR")//가스켓은 별도의 함수에서 처리함.
                        return;
                    string type_name = "/" + specname + "." + tanswer;
                    try
                    {
                        var selobj = owner.Members().Where(x => x.GetElementType() == DbElementTypeInstance.SELEC).Where(x => x.GetAsString(DbAttributeInstance.TANS) == tanswer);

                        if (selobj.Count() == 0)
                        {
                            sel = owner.CreateLast(copy_from_type);
                            sel.SetAttribute(DbAttributeInstance.NAME, type_name);
                            sel.Copy(copy_from_element);
                        }
                        else
                        {
                            //return;
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
                        if (!diameters.Contains(size))
                            return;

                    }
                    //복사할 Flange Spec에서 첫번째가 아니면 복사하지 않는다.
                    else if(ddepth==5 && owner.Owner.GetAsString(DbAttributeInstance.TANS)=="FLAN" && copy_from_element.GetInteger(DbAttributeInstance.SEQU)!=1)
                    {
                        return;
                    }
                    if (ddepth == 6 && tanswer.Contains("HMD/SETONTEE"))
                    {//SETON TEE는 별도로 관리한다.
                        return;
                    }
                    //sel = owner.CreateLast(DbElementTypeInstance.SELEC);
                    if (tanswer.Contains("JOINT-MITRE") || tanswer.Contains("JOINT-THREAD"))
                    {
                        return;
                    }
                    IEnumerable<DbElement> selobj = null;
                    if (copy_from_type == DbElementTypeInstance.SELEC)
                    {
                        if (tanswer.Length > 4)
                        {
                            if (tanswer.Substring(0, 5) == "POORG")
                                tanswer = "ORING";
                        }
                        selobj = owner.Members().Where(x => x.GetAsString(DbAttributeInstance.TANS) == tanswer && x.GetAsString(DbAttributeInstance.ANSW) == answer && x.GetAsString(DbAttributeInstance.QUES) == question && x.GetAsString(DbAttributeInstance.QUAL) == qualifier);
                    }

                    else
                    {
                        if (owner.GetAsString(DbAttributeInstance.TANS) == tanswer && owner.GetAsString(DbAttributeInstance.ANSW) == answer)
                            selobj = owner.Members();
                        else
                        {
                            selobj = owner.Members().Where(x => x.GetAsString(DbAttributeInstance.TANS) == tanswer && x.GetAsString(DbAttributeInstance.ANSW) == answer);
                        }
                    }

                    //같은 사양의 스펙이 존재하는지 확인하는 부분.
                    if (selobj == null)
                        sel = owner.CreateLast(copy_from_type);
                    else if (selobj.Count() == 0)
                        sel = owner.CreateLast(copy_from_type);
                    else
                    {
                        sel = selobj.First();
                    }

                    if (copy_from_type == DbElementTypeInstance.SPCOMPONENT)
                    {
                        //spco이름 세팅
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.NAME, "/" + specname + copy_from_element.GetAsString(DbAttributeInstance.CATR).Replace("-SCOM", ""));
                        }
                        catch (Exception ee){}
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
                            //Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("{0} ",sel.GetAsString(DbAttributeInstance.FLNM ))).RunInPdms();
                            //Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("DETREF {0} ",copy_from_element.GetElement(DbAttributeInstance.DETR).GetAsString(DbAttributeInstance.FLNM))).RunInPdms();
                            sel.SetAttribute(DbAttributeInstance.DETR, copy_from_element.GetElement(DbAttributeInstance.DETR));
                        }
                        catch (Aveva.Pdms.Utilities.Messaging.PdmsException ee)
                        {
                        }
                        try
                        {
                            sel.SetAttribute(DbAttributeInstance.PRTREF, copy_from_element.GetElement(DbAttributeInstance.PRTREF));
                        }
                        catch (Exception ee)
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
            catch (Exception ee)
            {
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} any is wrong|", "--")).RunInPdms();
            }


            //DbCopyOption op = new DbCopyOption();
            //sel.CopyHierarchy(copy_from_element, op);

            //reculsive_copy_spec()
        }

        //ON TEE를 만들지 않을 시스템
        string[] NoAvailable_OnTEE_sys = new string[] { "UR", "PR", "PT", "PX", "DE", "DF", "RG", "RL" };
        private void CreateSpecification(string connstr, string query, string dept)
        {
            OdbcConnection conn = new OdbcConnection(connstr);
            OdbcCommand cmd = new OdbcCommand(query, conn);
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);
            da.Fill(dt);

            string specworld = "";
            if (dept == "ACCOM")
            {
                specworld = "/PROJ_SPWL.ACCOM";
            }
            else if (dept == "MACH")
            {
                specworld = "/PROJ_SPWL.MACH";
            }
            else if (dept == "PIPE")
            {
                specworld = "/PROJ_SPWL.PIPE";
            }

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
                int txtsize = txt_specname.Text.Count();
                try
                {
                    if (txt_specname.Text != "" && lineno.lineno.ToString().Substring(0, txtsize) != txt_specname.Text)
                        continue;
                }
                catch (Exception ee)
                {

                }
                //if (cnt == 6)
                //    break;
                //스펙생성부분
                //DbElement spec_element = DbElement.GetElement("/PROJ_SPWL").CreateAfter(DbElementTypeInstance.SPECIFICATION);
                string specname = dept + "." + lineno.lineno.ToString().Replace(" ,", ".");
                specname = specname.Replace(", ", ".");
                specname = specname.Replace(' ', '.');
                DbElement spec_element = null;

                //스펙이름이 없을경우 새로 만들고
                if (DbElement.GetElement("/" + specname).IsNull)
                {

                    spec_element = DbElement.GetElement(specworld).CreateLast(DbElementTypeInstance.SPECIFICATION);
                    if (specname.Length>50) //SPEC이름은 최대 50자까지 입력가능하다 이를 초과할경우그래서 47자+"..."으로 구성한다.
                    {
                        specname = specname.Substring(0, 45)+"...";
                    }
                    spec_element.SetAttribute(DbAttributeInstance.NAME, "/" + specname);
                    spec_element.SetAttribute(DbAttributeInstance.QUES, "TYPE");
                    spec_element.SetAttribute(DbAttributeInstance.PURP, "PIPE");
                    spec_element.SetAttribute(DbAttributeInstance.DESC, string.Format("{0}/PROJ.SPEC", dept));
                    spec_element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));
                    spec_element.SetAttribute(DbAttributeInstance.PDAREF, DbElement.GetElement("/HMD.PDATAB.DEF"));

                }
                else  //해당스펙 존재시에는 spec element를 리턴한다.
                {

                    try
                    {
                        //SPEC TEXT 추가
                        DbElement spectext_element = spec_element.CreateLast(DbElementTypeInstance.TEXT);
                        spectext_element.SetAttribute(DbAttributeInstance.NAME, "/" + specname + "." + "PIPINGTEXT");
                        spectext_element.SetAttribute(DbAttributeInstance.STEX, "PIPING");
                        spectext_element.SetAttribute(DbAttributeInstance.RTEX, "PIPING");
                    }
                    catch (Exception ee)
                    {
                        spec_element = DbElement.GetElement("/" + specname);
                    }
                }
                catref_available_list.Clear();
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
                        string connmatl = targetrow[0]["PIPE_C_MATL"].ToString();
                        string connstd = targetrow[0]["PIPE_C_STD"].ToString();

                        
                        
                        //스펙의 재질정보 // STEEL , SUS304 , SUS304L , SUS316L 등
                        DataRow[] materialArr = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row["DETERMINE_ARGS"].ToString() == matlspec).ToArray();
                        
                        
                        //Spec 생성시에 bending machine reference에 값을 넣는다.
                        
                        if (materialArr.Length>0)
                        {
                            spec_element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement(materialArr[0]["BEND_MACHINE"].ToString()));
                        }
                        string main_mtrl="";
                        if (materialArr.Count()>0)
                            main_mtrl = materialArr[0][2].ToString();


                        //string connspec = targetrow[0]["PIPE_C_TYPE"].ToString() + "/" + targetrow[0]["PIPE_C_STD"].ToString() + "/" + targetrow[0]["PIPE_C_MATL"].ToString();
                        string connspecstr = "*/" + connstd + "/" + connmatl;
                        connspecstr = connspecstr.Replace("\n", "");
                        string valvespecstr = targetrow[0]["VALVE_C_TYPE"].ToString();

                        DataRow[] matl_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == matlspec).ToArray();
                        DataRow[] conn_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == connspecstr).ToArray();
                        DataRow[] valve_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == valvespecstr).ToArray();

                        //Framo는 matlspec+"/"+connstd형태로 ref테이블에 구성했다.
                        DataRow[] framo_matl_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == matlspec+"/"+connstd).ToArray();
                        if (framo_matl_datarow.Length>0 )
                        {
                            if( framo_matl_datarow[0]["ETC"].ToString()=="FRAMO")
                                spec_element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement(framo_matl_datarow[0]["BEND_MACHINE"].ToString()));
                        }

                        //square flange를 위한 스펙항목 추출
                        //선급정보 추가적으로 넣어야함.
                        string sqfl_key = matlspec + "/" + connstd;
                        DataRow[] sqfl_datarow = comp_ref_dt.Rows.Cast<DataRow>().Where(row => row[0].ToString() == sqfl_key && row["CERT"].ToString() == "").ToArray();

                        DbElement material_spec = null;
                        DbElement conn_spec = null;
                        DbElement gask_spec = null;
                        DbElement gask_spec2 = null;
                        DbElement valve_spec = null;
                        DbElement framo_material_spec = null;
                        DbElement weld_spec = null;
                        DbElement commonAtta_spec = null;
                        DbElement ecp_hanger_spec = null;
                        DbElement sleeve_spec = null;
                        DbElement sqfl_spec = null;
                        DbElement oring_spec = null;
                        DbElement ontee_spec = null;
                        List<int> diameters = get_dias(nomdia.ToString());

                    #region Material Spec생성
                        try
                        {
                            if (matl_datarow.Count() == 0)
                            {
                                //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} material ref 없음|", matlspec)).RunInPdms();
                            }
                            else
                            {
                                //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                                material_spec = DbElement.GetElement(matl_datarow[0][1].ToString());

                                
                                if (!material_spec.GetElement(DbAttributeInstance.PDAREF).IsNull)
                                {
                                    DbElement.GetElement("/" + specname).SetAttribute(DbAttributeInstance.PDAREF, material_spec.GetElement(DbAttributeInstance.PDAREF));
                                }

                                string[] allowtype = new string[] { "ELBO" };//
                                foreach (DbElement element in material_spec.Members())
                                {

                                    if (element.GetElementType().ToString() != "TEXT")
                                    {                                        
                                        reculsive_copy_spec(spec_element, element, specname, diameters);
                                    }
                                }
                            }

                            
                            //SetOn TEE생성
                            
                            if (dept=="ACCOM"  &&   NoAvailable_OnTEE_sys.Contains(lineno.lineno.ToString().Substring(0, 2)))
                            {
                                Console.WriteLine("ONTEE 만들지 않음");
                            }
                                
                            else
                            {
                                ontee_spec = DbElement.GetElement("/PTONTEE/HMDP_SPEC");
                                Building_OnTee_Struct(specname, spec_element, diameters, ontee_spec);
                            }

                        }
                        catch (Exception EE)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |Material Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                    #endregion

                    #region Conn spec 생성
                        try
                        {
                            if (conn_datarow.Count() == 0)
                            {
                                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} conn ref 없음|", connspecstr)).RunInPdms();
                            }
                            else
                            {
                                //가스켓 스펙 생성부분
                                if (dept == "PIPE" || dept == "ACCOM")
                                {
                                    //가스켓생성
                                    DbElement[]  gask_specs=Create_Gasket_Pipe(specname, spec_element, connspecstr, diameters, targetrow[0]);
                                    gask_spec = gask_specs[0];
                                    gask_spec2 = gask_specs[1];
                                }
                                else
                                {
                                    gask_spec=Create_Gasket_Mach(specname, spec_element, connspecstr, diameters, targetrow[0]);
                                }
                                //return;

                                //Flange Spec
                                conn_spec = DbElement.GetElement(conn_datarow[0][1].ToString());
                                foreach (DbElement element in conn_spec.Members())
                                {
                                    reculsive_copy_spec(spec_element, element, specname, diameters);

                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |Conn Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                    #endregion

                    #region //Valve Spec생성
                        try
                        {
                            if (valve_datarow.Count() == 0)
                            {
                                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} valve ref 없음|", valvespecstr).Replace("\n", "")).RunInPdms();
                            }
                            else
                            {
                                valve_spec = DbElement.GetElement(valve_datarow[0][1].ToString());
                                foreach (DbElement element in valve_spec.Members())
                                {
                                    reculsive_copy_spec(spec_element, element, specname, diameters);
                                }

                            }

                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |Valve Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                    #endregion

                        //DbElement conn_spec = DbElement.GetElement("/PFSF30/HMDP_SPEC");
                        //DbElement valve_spec = DbElement.GetElement("/PVVVV10/HMDP_SPEC");
                    #region FRAMO스펙일경우
                        try
                        {
                            if (framo_matl_datarow.Count() == 0)
                            {
                                //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} material ref 없음|", matlspec+"/"+connstd)).RunInPdms();
                            }
                            else
                            {
                                //Aveva.Pdms.Utilities.CommandLine.Command.OutputAndClearError();
                                framo_material_spec = DbElement.GetElement(framo_matl_datarow[0][1].ToString());
                                if (!framo_material_spec.GetElement(DbAttributeInstance.PDAREF).IsNull)
                                {
                                    DbElement.GetElement("/" + specname).SetAttribute(DbAttributeInstance.PDAREF, framo_material_spec.GetElement(DbAttributeInstance.PDAREF));
                                }

                                string[] allowtype = new string[] { "ELBO" };//
                                foreach (DbElement element in framo_material_spec.Members())
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


                        }
                        catch (Exception EE)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |Material Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                    #endregion


                    #region WELD스펙  생성부분
                        try
                        {
                            //Weld스펙생성
                            weld_spec = DbElement.GetElement("/POWELD/HMDP_SPEC");
                            foreach (DbElement element in weld_spec.Members())
                            {
                                reculsive_copy_spec(spec_element, element, specname, diameters);
                            }
                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |Weld Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                        #endregion


                        #region ATTA스펙  생성부분
                        try
                        {
                            //ATTA스펙생성
                            commonAtta_spec = DbElement.GetElement("/PA00/HMDP_SPEC");
                            foreach (DbElement element in commonAtta_spec.Members())
                            {
                                reculsive_copy_spec(spec_element, element, specname, diameters);
                            }
                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |Atta Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                        #endregion

                        #region 배관ECP파이프에서 쓰는 Hanger스팩 생성부분
                        try
                        {
                            if (dept == "PIPE" && ecp_system.Contains(lineno.lineno.ToString().Substring(0, 2)))
                            {
                                // Hanger스펙생성
                                ecp_hanger_spec = DbElement.GetElement("/PIPE.ECP.HANGER.STD");
                                foreach (DbElement element in ecp_hanger_spec.Members())
                                {
                                    reculsive_copy_spec(spec_element, element, specname, diameters);
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |hanger Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                    #endregion
                        
                    #region Sleeve스펙 생성 부분
                        try
                        {
                            // sleeve스펙생성
                            sleeve_spec = DbElement.GetElement("/PS-STEEL/HMDP_SPEC");

                            if (main_mtrl == "SUS304L")
                            {
                                sleeve_spec = DbElement.GetElement("/PS-SUS304L/HMDP_SPEC");
                            }
                            else if (main_mtrl == "SUS316L")
                            {
                                sleeve_spec = DbElement.GetElement("/PS-SUS316L/HMDP_SPEC");
                            }

                            foreach (DbElement element in sleeve_spec.Members())
                            {
                                reculsive_copy_spec(spec_element, element, specname, diameters);
                            }
                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |hanger Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                    #endregion

                        #region Sqaure Falnge Spec구성스펙 생성 부분
                        try
                        {
                            if (sqfl_datarow.Count() > 0)
                            {

                                sqfl_spec = DbElement.GetElement(sqfl_datarow[0][1].ToString());
                                // sleeve스펙생성


                                foreach (DbElement element in sqfl_spec.Members())
                                {
                                    reculsive_copy_spec(spec_element, element, specname, diameters);
                                }

                                #region Oring 스펙 구성 부분
                                try
                                {
                                    oring_spec = DbElement.GetElement(sqfl_datarow[0]["ORING_SPEC"].ToString());
                                    // sleeve스펙생성

                                    foreach (DbElement element in oring_spec.Members())
                                    {
                                        reculsive_copy_spec(spec_element, element, specname, diameters);
                                    }
                                }
                                catch (Exception ee)
                                { }
                                #endregion
                            }


                        }
                        catch (Exception ee)
                        {
                            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |hanger Spec찾을수 없음|").Replace("\n", "")).RunInPdms();
                        }
                        #endregion

                        //이 메소드를 만든 이유는 빈번한 스펙변경으로 일단 스펙을 만들고 사양이 변경되면 
                        //새롭게 만들어지는 것들은 상관없으나 기존에 맞지않는 스펙을 삭제하기 위해서 
                        //한스펙생성시에 사용되어지는 Standard Spec의 catref 를 모두 가능범위안에 두고(spco_available_list 변수)
                        //스펙이 하나가 만들어지고 나서 가능범위안에 없는 애들은 삭제하는 기능을 사용하기 위해서 가능범위리스트를 만드는 메소드이다.
                        Building_Available_Spco(material_spec, conn_spec, gask_spec,gask_spec2, valve_spec, framo_material_spec, weld_spec, ecp_hanger_spec, sleeve_spec, sqfl_spec,  oring_spec,  ontee_spec,commonAtta_spec);

                    }
                }
                catch (Exception ee)
                {
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} 오륭", lineno.lineno.ToString() + "/" + lineno.nomdia.ToString())).RunInPdms();
                    Console.WriteLine("오류:" + ee.Message);
                }

                ////Stype을  Oracle Table인 AM_STYPE에 정의된 Component Group들만 바꾼다.
                STYPE_Configuration(spec_element);

                //Elbow에 대해서 각도제한 해제 무조건 1~90도로 바꿈
                AngleFree1to90(spec_element);


                ////이미 만든스펙에서 사양변경되어 스펙이 다시 만들어지는 경우 기존의 찌꺼기SPCO가 존재하게 되는데
                ////이것을 삭제하는 부분임
                ////Building_Available_Spco()에서 가능한 리스트를 만들고 여기에 없는 애들은 삭제한다.
                DeleteDiffSpec(spec_element);

                //////중복 SPCO 제거
                Delete_Duplicate_Spco(spec_element);


                ////지우고 싶은 SPCO삭제구현.
                Delete_NoUSEC_Spco(spec_element, dept, lineno.lineno.ToString().Substring(0, 2));

                //////빈껍데기 Selef를 제거함. 재귀적 호출을 통해 제거함.
                ClearEmptySele(spec_element);
                
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

        private void AngleFree1to90(DbElement spec_element)
        {
            try
            {

                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC };
                TypeFilter filter = new TypeFilter(dbtype);
                object para = (object)0;

                AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.QUES, FilterOperator.Equals, "ANGL");
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection mach_coll = new DBElementCollection(spec_element, finalfilter);
                foreach (DbElement item in mach_coll)
                {
                    foreach (DbElement subitem in item.Members())
                    {
                        subitem.SetAttribute(DbAttributeInstance.ANSW, 1.0);
                        subitem.SetAttribute(DbAttributeInstance.MAXA, 90.0);
                    }

       
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
        }

        //지우는거.
        //일단 만들고 나서 안쓰는 컴포넌트들을 여기에서 모두 처리한다.
        private void Delete_NoUSEC_Spco(DbElement spec_element,string dept,string system)
        {            

            if(dept=="ACCOM" )
            {





                //Inst VWTA type모두 삭제 // UR,PR일경우는 GASK와 FLAN도 지운다.
                AndFilter finalfilter = new AndFilter();
                TypeFilter filt = new TypeFilter(DbElementTypeInstance.SELEC);
                AttributeIntFilter intfilter = new AttributeIntFilter(DbAttributeInstance.DDEP, FilterOperator.Equals, 3);
                finalfilter.Add(filt);
                
                finalfilter.Add(intfilter);
                DBElementCollection tmpseles = new DBElementCollection(spec_element, finalfilter);
                DbElement[] resultseles = tmpseles.Cast<DbElement>().Where(x => x.GetAsString(DbAttributeInstance.TANS) == "VTWA" || x.GetAsString(DbAttributeInstance.TANS) == "INST" || x.GetAsString(DbAttributeInstance.TANS) == "GASK" || x.GetAsString(DbAttributeInstance.TANS) == "FLAN").ToArray();
                foreach (DbElement sele in resultseles)
                {
                    try
                    {
                        string tanswer = sele.GetAsString(DbAttributeInstance.TANS);
                        //선실 Duct에 gasket을 지운다.
                        if (system == "UR" || system == "PR")
                        {
                            if (tanswer == "GASK" || tanswer == "FLAN")
                            {
                                sele.Delete();
                            }
                           
                        }
                        else
                        {
                            if (tanswer == "GASK" || tanswer == "FLAN")
                                continue;
                            sele.Delete();
                        }
                    }
                    catch (Exception ee)
                    { }
                }
                //ONTEE삭제
                if(NoAvailable_OnTEE_sys.Contains(system))
                {
                    TypeFilter typefilt = new TypeFilter(DbElementTypeInstance.SELEC);
                    AttributeStringFilter filt2=new AttributeStringFilter(DbAttributeInstance.TANS,FilterOperator.StartsWith,"HMD/SETONTEE");
                    AndFilter andfilter=new AndFilter();
                    andfilter.Add(typefilt);
                    andfilter.Add(filt2);
                    DBElementCollection seles = new DBElementCollection(spec_element, andfilter);
                    try
                    {
                        foreach (DbElement element in seles)
                        {
                            element.Delete();
                        }
                    }catch(Exception ee)
                    {
                        Console.WriteLine("11");
                    }
                    
                }
                else
                {
                    //복사할 Flange Spec에서 첫번째 SELE만 두고 나머지는 삭제
                    finalfilter = new AndFilter();
                    filt = new TypeFilter(DbElementTypeInstance.SELEC);
                    intfilter = new AttributeIntFilter(DbAttributeInstance.DDEP, FilterOperator.Equals, 5);
                    
                    finalfilter.Add(filt);
                    finalfilter.Add(intfilter);

                    DBElementCollection deleting_flanges = new DBElementCollection(spec_element, finalfilter);
                    DbElement[] target_element = deleting_flanges.Cast<DbElement>().Where(x => x.Owner.Owner.GetAsString(DbAttributeInstance.TANS) == "FLAN" && x.GetInteger(DbAttributeInstance.SEQU) != 1).ToArray();


                    foreach (DbElement flange in target_element)
                    {
                        try {
                            flange.Delete();
                        }catch(Exception ee)
                        {

                        }
                        
                    }
                }
                
            }


            if(dept=="ACCOM" || dept=="MACH")
            {
                //8m 10m 12m 파이프는 배관만 쓴다.
                //복사할 tube bend Spec에서 첫번째 SELE만 두고 나머지는 삭제
                AndFilter finalfilter = new AndFilter();
                TypeFilter filt = new TypeFilter(DbElementTypeInstance.SELEC);
                AttributeIntFilter intfilter = new AttributeIntFilter(DbAttributeInstance.DDEP, FilterOperator.Equals, 5);

                finalfilter.Add(filt);
                finalfilter.Add(intfilter);

                DBElementCollection deleting_flanges = new DBElementCollection(spec_element, finalfilter);
                DbElement[] target_element = deleting_flanges.Cast<DbElement>().Where(x => (x.Owner.Owner.GetAsString(DbAttributeInstance.TANS) == "TUBE" || x.Owner.Owner.GetAsString(DbAttributeInstance.TANS) == "BEND") && x.GetInteger(DbAttributeInstance.SEQU) != 1).ToArray();


                foreach (DbElement tube_bend in target_element)
                {
                    try
                    {
                        tube_bend.Delete();
                    }
                    catch (Exception ee)
                    {

                    }

                }
            }
        }
        //중복 spco를 지우는데 이름이 있는걸 최대한 살리고 이름이 모두없으면 그중 아무거나 1개만 남긴다. 
        private void Delete_Duplicate_Spco(DbElement spec_element)
        {
            TypeFilter filt = new TypeFilter(DbElementTypeInstance.SPCOMPONENT);
            DBElementCollection spcos = new DBElementCollection(spec_element, filt);

            var spcos_group=spcos.Cast<DbElement>().GroupBy(r=>r.GetElement(DbAttributeInstance.CATR));
            foreach (var group in spcos_group)
            {
                DbElement[] dbelements = group.ToArray();
                List<string> elementnames= new List<string>();
                foreach (DbElement item in dbelements)
	            {
                    elementnames.Add(item.GetAsString(DbAttributeInstance.NAME));		 
	            }
                if (elementnames.Count == 1)
                    continue;
                int deletecnt=0;
                foreach (string elementName in elementnames)
                {
                    DbElement element = DbElement.GetElement(elementName);
                    bool isnamed = element.GetBool(DbAttributeInstance.ISNAME);

                    if (!isnamed)
                    {
                        if ((elementnames.Count - deletecnt) == 1)
                            break;

                        element.Delete();
                        deletecnt++;

                    }
                }
            }
        }

        private void DeleteDiffSpec(DbElement spec_element)
        {
            TypeFilter filt = new TypeFilter(DbElementTypeInstance.SPCOMPONENT);
            DBElementCollection spcos_within_curspec = new DBElementCollection(spec_element, filt);
            List<string> spcos_str_list= spcos_within_curspec.Cast<DbElement>().Select(r => r.GetAsString(DbAttributeInstance.NAME)).ToList();
            foreach (string item in spcos_str_list)
            {
                DbElement itemref = DbElement.GetElement(item);
                DbElement catr = itemref.GetElement(DbAttributeInstance.CATR);
                if (!catref_available_list.Contains(catr))
                {
                    itemref.Delete();
                }
                    
            }
        }
        List<DbElement> catref_available_list = new List<DbElement>();

        private void Building_Available_Spco(DbElement material_spec, DbElement conn_spec, DbElement gask_spec, DbElement gask_spec2, DbElement valve_spec, DbElement framo_material_spec, DbElement weld_spec, DbElement ecp_hanger_spec, DbElement sleeve_spec, DbElement sqfl_spec, DbElement oring_spec, DbElement ontee_spec,DbElement commonAtta_sepc)
        {

            TypeFilter spcotype_filter = new TypeFilter(DbElementTypeInstance.SPCOMPONENT);
            
            if (material_spec!=null)
            {
                DBElementCollection matlspco_coll = new DBElementCollection(material_spec, spcotype_filter);
                catref_available_list.AddRange(matlspco_coll.Cast<DbElement>().Select(r=>r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (conn_spec != null)
            {
                DBElementCollection connspco_coll = new DBElementCollection(conn_spec, spcotype_filter);
                catref_available_list.AddRange(connspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (gask_spec!=null)
            {
                DBElementCollection gaskspco_coll = new DBElementCollection(gask_spec, spcotype_filter);
                catref_available_list.AddRange(gaskspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (gask_spec2!=null)
            {
                DBElementCollection gask2spco_coll = new DBElementCollection(gask_spec2, spcotype_filter);
                catref_available_list.AddRange(gask2spco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (valve_spec != null)
            {
                DBElementCollection valvspco_coll = new DBElementCollection(valve_spec, spcotype_filter);
                catref_available_list.AddRange(valvspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (framo_material_spec != null)
            {
                DBElementCollection framospco_coll = new DBElementCollection(framo_material_spec, spcotype_filter);
                catref_available_list.AddRange(framospco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (weld_spec != null)
            {
                DBElementCollection weldspco_coll = new DBElementCollection(weld_spec, spcotype_filter);
                catref_available_list.AddRange(weldspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (ecp_hanger_spec != null)
            {
                DBElementCollection ecpspco_coll = new DBElementCollection(ecp_hanger_spec, spcotype_filter);
                catref_available_list.AddRange(ecpspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (sleeve_spec != null)
            {
                DBElementCollection sleevespco_coll = new DBElementCollection(sleeve_spec, spcotype_filter);
                catref_available_list.AddRange(sleevespco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (sqfl_spec != null)
            {
                DBElementCollection sqflspco_coll = new DBElementCollection(sqfl_spec, spcotype_filter);
                catref_available_list.AddRange(sqflspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (oring_spec != null)
            {
                DBElementCollection oringspco_coll = new DBElementCollection(oring_spec, spcotype_filter);
                catref_available_list.AddRange(oringspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (ontee_spec != null)
            {
                DBElementCollection onteespco_coll = new DBElementCollection(ontee_spec, spcotype_filter);
                catref_available_list.AddRange(onteespco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            if (commonAtta_sepc != null)
            {
                DBElementCollection commonAttaspco_coll = new DBElementCollection(commonAtta_sepc, spcotype_filter);
                catref_available_list.AddRange(commonAttaspco_coll.Cast<DbElement>().Select(r => r.GetElement(DbAttributeInstance.CATR)).ToList());
            }
            
            
                       
        }

        string[] STEAM_CONDENSATE_SYSTEM = new string[] { "TC", "TD", "TH", "TL", "TR", "TS", "TU" };
        string[] FRESH_WATER = new string[] { "DA", "DP", "DX", "WG", "BG", "BA", "BB", "BC", "BD", "BH", "BP", "BS", "BT", "PB", "PD", "PE", "PG", "PO", "PR", "PS", "PT", "PW", "PX", "WA", "WB", "WC", "WD", "WE", "WG" };
        string[] BALLAST_LINE = new string[] { "GR" };
        string[] FIREWASH_FOAM = new string[] { "BF", "QF" };
        string[] ECP = new string[] { "EB", "EH", "EM" };
        string[] COMPRESSED_AIR = new string[] { "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AK", "AR" };
        string[] AIR_VENT_SOUNDING = new string[] { "VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VN", "VS", "VT", "VM" };
        string[] FUELOIL_DO = new string[] { "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW" };
        string[] LO = new string[] { "LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI", "LJ", "LK" };
        string[] FRESHWATER = new string[] { "DH" };
        string[] CARGO_HANDLING = new string[] { "CO", "CS" };
        string[] CARG_ODME = new string[] { "GS" };
        string[] CARGO_DROP = new string[] { "CD" };
        string[] CARGO_TANK = new string[] { "CT" };
        string[] INERT_GAS = new string[] { "IG", "NG", "IO", "IV" };
        string[] PENETRATION_COTANK = new string[] { "GH", "CH" };
        string[] CO2 = new string[] { "QB", "QC" };
        string[] DRY_POWDER = new string[] { "QD", "QL", "QS" };
        string[] HYDRAULIC_OIL = new string[] { "HA", "HB", "HD", "HF", "HH", "HK", "HO", "HR", "HT", "HV" };
        string[] GAUGING = new string[] { "GA", "GC", "GD", "GF", "GH", "GL", "GP" };
        string[] SEAWATER = new string[] { "SA", "SC", "SD", "SE", "SG", "SM" };
        string[] UNINSULATED_SPIRAL_DUCT = new string[] { "UR" };
        string[] EXHAUST = new string[] { "XA", "XD", "XL", "XO", "XS" };

        /// <summary>
        ///   가스켓스펙을 만드는 부분  
        /// </summary>
        /// <param name="specname"></param>
        /// <param name="spec_element"></param>
        /// <param name="connspec"></param>
        /// <param name="diameters"></param>
        /// <param name="targetrow"></param>
        private void Create_Gasket(string specname, DbElement spec_element, string connspec, List<int> diameters, DataRow targetrow)
        {
            DbElement gask_spec = null;
            DbElement sel = null;
            string system = targetrow["LINE_NO"].ToString().Substring(0, 2);
            if (connspec.Contains("5K"))
            {
                gask_spec = DbElement.GetElement("/POGASASF05RF/HMDP_SPEC");
            }
            else if (connspec.Contains("10K"))
            {
                gask_spec = DbElement.GetElement("/POGASASF10RF/HMDP_SPEC");
            }
            else if (connspec.Contains("16K"))
            {
                gask_spec = DbElement.GetElement("/POGASASF16RF/HMDP_SPEC");
            }
            else if (connspec.Contains("20K"))
            {
                gask_spec = DbElement.GetElement("/POGASASF20RF/HMDP_SPECC");
            }
            else if (connspec.Contains("30K"))
            {
                gask_spec = DbElement.GetElement("/POGASASF30SER/HMDP_SPEC");
            }
            foreach (DbElement element in gask_spec.Members())
            {
                reculsive_copy_spec(spec_element, element, specname, diameters);

            }
        }

        //가스켓로직
        private DbElement [] Create_Gasket_Pipe(string specname, DbElement spec_element, string connspec, List<int> diameters, DataRow targetrow)
        {
            //Gasket 생성
            DbElement gask_spec = null;
            DbElement gask_spec2 = null;
            string system = targetrow["LINE_NO"].ToString().Substring(0, 2);
            int minsize = 0;
            int maxsize = 1000;
            if (STEAM_CONDENSATE_SYSTEM.Contains(system))
            {

                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASASF05RFWIR/HMDP_SPEC");
                else if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASSPW10RFS4/HMDP_SPEC");
                else if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASSPW16RFS4/HMDP_SPEC");
                else if (connspec.Contains("20K"))
                    gask_spec = DbElement.GetElement("/POGASSPW20RFS4/HMDP_SPEC");
                else if (connspec.Contains("30K"))
                    gask_spec = DbElement.GetElement("/POGASSPW30RFS4/HMDP_SPEC");


            }
            else if (BALLAST_LINE.Contains(system))
            {
                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASNBR05FF/HMDP_SPEC");
                if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASNBR10FF/HMDP_SPEC");
                if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASNBR16FF/HMDP_SPEC");
            }
            else if (FUELOIL_DO.Contains(system))
            {
                
                if (connspec.Contains("5K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF05FF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF05RF/HMDP_SPEC");
                }
                if (connspec.Contains("10K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF10FF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF10RF/HMDP_SPEC");
                }
                if (connspec.Contains("16K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF16FF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF16RF/HMDP_SPEC");
                }
                if (connspec.Contains("20K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF20FF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF20RF/HMDP_SPEC");
                }
                if (connspec.Contains("30K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF30FF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF30RF/HMDP_SPEC");
                }
                Building_Gasket_Struct(specname, spec_element, diameters, gask_spec2);

            }
            else if (LO.Contains(system))
            {

                if (connspec.Contains("5K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF05FFF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF05RFF/HMDP_SPEC");
                }
                if (connspec.Contains("10K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF10FFF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF10RFF/HMDP_SPEC");
                }
                if (connspec.Contains("16K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF16FFF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF16RFF/HMDP_SPEC");
                }
                if (connspec.Contains("20K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF20FFF/HMDP_SPEC");
                    gask_spec2 = DbElement.GetElement("/POGASASF20RFF/HMDP_SPEC");
                }

                Building_Gasket_Struct(specname, spec_element, diameters, gask_spec2);

            }
            else if (CARGO_HANDLING.Contains(system) || CARG_ODME.Contains(system) || CARGO_TANK.Contains(system) || INERT_GAS.Contains(system))
            {
                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASTEF05RFSER/HMDP_SPEC");
                if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASTEF10RFSER/HMDP_SPEC");
                if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASTEF16RFSER/HMDP_SPEC");
                if (connspec.Contains("20K"))
                    gask_spec = DbElement.GetElement("/POGASTEF20RFSER/HMDP_SPEC");
                if (connspec.Contains("30K"))
                    gask_spec = DbElement.GetElement("/POGASTEF30RFSER/HMDP_SPEC");
            }
            else if (CARGO_DROP.Contains(system))
            {
                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASTEF05FFSER/HMDP_SPEC");
                if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASTEF10FFSER/HMDP_SPEC");
                if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASTEF16FFSER/HMDP_SPEC");

            }
            else
            {
                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASASF05RF/HMDP_SPEC");
                if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASASF10RF/HMDP_SPEC");
                if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASASF16RF/HMDP_SPEC");
                if (connspec.Contains("20K"))
                    gask_spec = DbElement.GetElement("/POGASASF20RF/HMDP_SPEC");
                if (connspec.Contains("30K"))
                    gask_spec = DbElement.GetElement("/POGASASF30RF/HMDP_SPEC");
            }


            Building_Gasket_Struct(specname, spec_element, diameters, gask_spec);
            return new DbElement[] {gask_spec,gask_spec2};

        }
        //가스켓로직
        private DbElement Create_Gasket_Mach(string specname, DbElement spec_element, string connspec, List<int> diameters, DataRow targetrow)
        {
            string[] AsbestosFree = new string[] { "B", "S", "W", "P", "A", "F", "I", "L", "V", "D", "Q" };

            //Gasket 생성
            DbElement gask_spec = null;
            string system = targetrow["LINE_NO"].ToString().Substring(0, 2);
            int minsize = 0;
            int maxsize = 1000;
            string sys_init = system.Substring(0, 1);
            if (sys_init == "T")
            {

                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASASF05RFWIR/HMDP_SPEC");
                else if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASSPW10SSGR/HMDP_SPEC");
                else if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASSPW16SSGR/HMDP_SPEC");
                else if (connspec.Contains("20K"))
                    gask_spec = DbElement.GetElement("/POGASSPW20SSGR/HMDP_SPEC");
                else if (connspec.Contains("30K"))
                    gask_spec = DbElement.GetElement("/POGASSPW30SSGR/HMDP_SPEC");

            }
            else if (AsbestosFree.Contains(sys_init))
            {
                if (connspec.Contains("5K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF05RF/HMDP_SPEC");
                }
                if (connspec.Contains("10K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF10RF/HMDP_SPEC");
                }
                if (connspec.Contains("16K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF16RF/HMDP_SPEC");
                }
                if (connspec.Contains("20K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF20RF/HMDP_SPEC");
                }
                if (connspec.Contains("30K"))
                {
                    gask_spec = DbElement.GetElement("/POGASASF30RF/HMDP_SPEC");
                }
            }

            else
            {
                if (connspec.Contains("5K"))
                    gask_spec = DbElement.GetElement("/POGASASF05RF/HMDP_SPEC");
                if (connspec.Contains("10K"))
                    gask_spec = DbElement.GetElement("/POGASASF10RF/HMDP_SPEC");
                if (connspec.Contains("16K"))
                    gask_spec = DbElement.GetElement("/POGASASF16RF/HMDP_SPEC");
                if (connspec.Contains("20K"))
                    gask_spec = DbElement.GetElement("/POGASASF20RF/HMDP_SPEC");
                if (connspec.Contains("30K"))
                    gask_spec = DbElement.GetElement("/POGASASF30RF/HMDP_SPEC");
                if (connspec.Contains("JISF7805"))
                    gask_spec = DbElement.GetElement("/POGASASF01FF/HMDP_SPEC");

            }


            Building_Gasket_Struct(specname, spec_element, diameters, gask_spec);
            return gask_spec;
        }

        //특정스펙내에 비어있는 셀레 지우기
        private void ClearEmptySele(DbElement spec)
        {
            //DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);
            
            DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC };
            TypeFilter filter = new TypeFilter(dbtype);
            object para = (object)0;

            AttributeIntFilter filter2 = new AttributeIntFilter(DbAttributeInstance.MCOU, FilterOperator.Equals, 0);
            AndFilter finalfilter = new AndFilter();
            finalfilter.Add(filter);
            finalfilter.Add(filter2);
            DBElementCollection sele_coll = new DBElementCollection(spec, finalfilter);
            IEnumerable<string> selelist = sele_coll.Cast<DbElement>().Select(x => x.GetAsString(DbAttributeInstance.REF)).ToArray();

            if (selelist.Count() == 0)
                return;


            int totalcount = 0;
            try
            {
                foreach (string item in selelist)
                {
                    try
                    {
                        string[] refnos = item.Replace("=", "").Split('/');
                        DbElement dbitem = DbElement.GetElement(new int[] { Convert.ToInt32(refnos[0]), Convert.ToInt32(refnos[1]) });

                        dbitem.Delete();

                        totalcount++;
                    }
                    catch (Exception ee)
                    {

                    }
                }
               
            }
            catch (Exception ee)
            {

            }
            ClearEmptySele(spec);
            this.lbl_emptysele_cnt.Text = " 지운 빈 셀레갯수 : " + totalcount.ToString();


        }

        //전체스펙에서 빈셀레 지우기
        private void ClearEmptySele()
        {
            //DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);
            DbElement base_accom = DbElement.GetElement("/PROJ_SPWL.ACCOM");
            DbElement base_mach = DbElement.GetElement("/PROJ_SPWL.MACH");
            DbElement base_pipe = DbElement.GetElement("/PROJ_SPWL.PIPE");
            DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC };
            TypeFilter filter = new TypeFilter(dbtype);
            object para = (object)0;

            AttributeIntFilter filter2 = new AttributeIntFilter(DbAttributeInstance.MCOU, FilterOperator.Equals, 0);
            AndFilter finalfilter = new AndFilter();
            finalfilter.Add(filter);
            finalfilter.Add(filter2);
            DBElementCollection accom_coll = new DBElementCollection(base_accom, finalfilter);
            DBElementCollection mach_coll = new DBElementCollection(base_mach, finalfilter);
            DBElementCollection pipe_coll = new DBElementCollection(base_pipe, finalfilter);
            IEnumerable<string> accomseles = accom_coll.Cast<DbElement>().Select(x => x.GetAsString(DbAttributeInstance.REF)).ToArray();
            IEnumerable<string> machseles = mach_coll.Cast<DbElement>().Select(x => x.GetAsString(DbAttributeInstance.REF)).ToArray();
            IEnumerable<string> pipeseles = pipe_coll.Cast<DbElement>().Select(x => x.GetAsString(DbAttributeInstance.REF)).ToArray();



            int totalcount = 0;
            try
            {
                foreach (string item in accomseles)
                {
                    try
                    {
                        string[] refnos = item.Replace("=", "").Split('/');
                        DbElement dbitem = DbElement.GetElement(new int[] { Convert.ToInt32(refnos[0]), Convert.ToInt32(refnos[1]) });

                        dbitem.Delete();

                        totalcount++;
                    }
                    catch (Exception ee)
                    {

                    }
                }
                foreach (string item in machseles)
                {
                    string[] refnos = item.Replace("=", "").Split('/');
                    DbElement dbitem = DbElement.GetElement(new int[] { Convert.ToInt32(refnos[0]), Convert.ToInt32(refnos[1]) });

                    dbitem.Delete();
                    totalcount++;
                }
                foreach (string item in pipeseles)
                {
                    string[] refnos = item.Replace("=", "").Split('/');
                    DbElement dbitem = DbElement.GetElement(new int[] { Convert.ToInt32(refnos[0]), Convert.ToInt32(refnos[1]) });

                    dbitem.Delete();
                    totalcount++;
                }

            }
            catch (Exception ee)
            {

            }
            this.lbl_emptysele_cnt.Text = " 지운 빈 셀레갯수 : " + totalcount.ToString();


        }

        private static void Building_Gasket_Struct(string specname, DbElement spec_element, List<int> diameters, DbElement gask_spec)
        {
            DbElement sele = null;
            DbElement copy_element = null;
            //첫번째 셀레
            string type_name = "/" + specname + "." + "GASK";
            try
            {
                var selobj = spec_element.Members().Where(x => x.GetAsString(DbAttributeInstance.NAME) == type_name);
                var copyobj = gask_spec.Members().Where(x => x.GetAsString(DbAttributeInstance.TYPE) == "SELE").Where(x => x.GetAsString(DbAttributeInstance.TANS) == "GASK");
                if (selobj.Count() == 0)
                {
                    foreach (DbElement item in copyobj)
                    {
                        sele = spec_element.CreateLast(DbElementTypeInstance.SELEC);
                        sele.SetAttribute(DbAttributeInstance.NAME, type_name);
                        sele.Copy(item);
                        copy_element = item;
                    }
                }
                else
                {
                    sele = selobj.First();
                    copy_element = copyobj.First();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine("해당 Selec는 존재하기에 오류남");
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} Selec는 존재하기에 오류남222|", type_name)).RunInPdms();
            }
            //두번째 셀레-Bore Size
            foreach (int diameter in diameters)
            {
                try
                {

                    foreach (DbElement boreitem in copy_element.Members())
                    {
                        if (boreitem.GetAsString(DbAttributeInstance.ANSW).Replace("mm", "") == diameter.ToString())
                        {
                            DbElement sele2 = null;
                            type_name = "/" + specname + "." + "GASK/" + diameter.ToString() + "mm";
                            var boreobj = sele.Members().Where(x => x.GetAsString(DbAttributeInstance.ANSW).Replace("mm", "") == diameter.ToString());
                            if (boreobj.Count() == 0)
                            {
                                sele2 = sele.CreateLast(DbElementTypeInstance.SELEC);
                                sele2.Copy(boreitem);
                            }
                            else
                                sele2 = boreobj.First();
                            sele2.SetAttribute(DbAttributeInstance.NAME, type_name);

                            //세번째 셀레-Stype
                            foreach (DbElement stypeitem in boreitem.Members())
                            {
                                DbElement sele3 = null;
                                string stype = stypeitem.GetAsString(DbAttributeInstance.TANS);
                                type_name = "/" + specname + "/" + diameter.ToString() + "/" + stype;
                                var stypeobj = sele2.Members().Where(x => x.GetAsString(DbAttributeInstance.TANS) == stype);
                                if (stypeobj.Count() == 0)
                                {
                                    sele3 = sele2.CreateFirst(DbElementTypeInstance.SELEC);
                                    sele3.Copy(stypeitem);
                                }
                                else
                                    sele3 = stypeobj.First();
                                sele3.InsertBeforeFirst(sele2);
                                sele3.SetAttribute(DbAttributeInstance.NAME, type_name);

                                //네번째 셀레-SPCO
                                foreach (DbElement spco in stypeitem.Members())
                                {
                                    DbElement sele4 = null;

                                    string compname = spco.GetAsString(DbAttributeInstance.CATR).Replace("-SCOM", "");
                                    type_name = "/" + specname + compname;
                                    if (sele3.Members().Count() == 0)
                                        sele4 = sele3.CreateLast(DbElementTypeInstance.SPCOMPONENT);
                                    else
                                    {
                                        sele4 = sele3.Members().First();
                                    }
                                    sele4.Copy(spco);
                                    sele4.SetAttribute(DbAttributeInstance.NAME, type_name);


                                }
                            }
                        }
                    }


                }
                catch (Exception ee)
                {

                }

            }
        }
        /// <summary>
        /// Seton TEE의 스펙을 자동생성하는 메소드임.
        /// </summary>
        /// <param name="specname">pipe spec name ex)PIPE.AC.101-200</param>
        /// <param name="spec_element">DbElement타입의 spec element /PIPE.AC.101-200</param>
        /// <param name="diameters">생성할 diameter size</param>
        /// <param name="tee_spec">참조할 Tee 공통스펙</param>
        private static void Building_OnTee_Struct(string specname, DbElement spec_element, List<int> diameters, DbElement tee_spec)
        {
            DbElement sele = null;
            DbElement copy_element = null;
            //첫번째 셀레
            string type_name = "/" + specname + "." + "TEE";
            try
            {
                //해당하는 Tee가 존재하는지 판단하는 부분임. 이름으로 판단하는것이 아니라 속성으로 검색하여 판단한다.
                var selobj = spec_element.Members().Where(x => x.GetAsString(DbAttributeInstance.NAME) == type_name);
                var copyobj = tee_spec.Members().Where(x => x.GetAsString(DbAttributeInstance.TYPE) == "SELE").Where(x => x.GetAsString(DbAttributeInstance.TANS) == "TEE");
                if (selobj.Count() == 0)
                {
                    foreach (DbElement item in copyobj)
                    {
                        sele = spec_element.CreateLast(DbElementTypeInstance.SELEC);
                        sele.SetAttribute(DbAttributeInstance.NAME, type_name);
                        sele.Copy(item);
                        copy_element = item;
                    }
                }
                else
                {
                    sele = selobj.First();
                    copy_element = copyobj.First();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine("해당 Selec는 존재하기에 오류남");
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p |{0} Selec는 존재하기에 오류남222|", type_name)).RunInPdms();
            }
            //두번째 셀레-Bore Size
            foreach (int diameter in diameters)
            {
                try
                {

                    foreach (DbElement boreitem in copy_element.Members())
                    {
                        if (boreitem.GetAsString(DbAttributeInstance.ANSW).Replace("mm", "") == diameter.ToString())
                        {
                            DbElement sele2 = null;
                            type_name = "/" + specname + "." + "TEE/" + diameter.ToString() + "mm";
                            var boreobj = sele.Members().Where(x => x.GetAsString(DbAttributeInstance.ANSW).Replace("mm", "") == diameter.ToString());
                            if (boreobj.Count() == 0)
                            {
                                sele2 = sele.CreateLast(DbElementTypeInstance.SELEC);
                                sele2.Copy(boreitem);
                            }
                            else
                                sele2 = boreobj.First();
                            sele2.SetAttribute(DbAttributeInstance.NAME, type_name);
                            //Sort 정렬 부분 
                            var boreitem_sorted = boreitem.Members().OrderByDescending(bore => bore.GetDouble(DbAttributeInstance.ANSW));

                            //3번째 셀레-Bore Size 2
                            foreach (DbElement boreitem2 in boreitem_sorted)
                            {
                                DbElement sele3 = null;
                                string bore2 = boreitem2.GetAsString(DbAttributeInstance.ANSW);
                                type_name = "/" + specname + "/" + diameter.ToString() + "/" + bore2;
                                
                                var boreobj2 = sele2.Members().Where(x => x.GetAsString(DbAttributeInstance.ANSW) == bore2);
                                if (boreobj2.Count() == 0)
                                {
                                    sele3 = sele2.CreateFirst(DbElementTypeInstance.SELEC);
                                    sele3.Copy(boreitem2);
                                }
                                else
                                    sele3 = boreobj2.First();
                                sele3.InsertBeforeFirst(sele2);
                                try
                                {
                                    sele3.SetAttribute(DbAttributeInstance.NAME, type_name);
                                }
                                catch (Exception ee) { }

                                //4번째 셀레-Stype
                                foreach (DbElement stypeitem in boreitem2.Members())
                                {
                                    DbElement sele4 = null;
                                    string stype = stypeitem.GetAsString(DbAttributeInstance.TANS);
                                    type_name = "/" + specname + "/" + diameter.ToString() + "/" + bore2 + "/" + stype;
                                    var stypeobj = sele3.Members().Where(x => x.GetAsString(DbAttributeInstance.TANS) == stype);
                                    if (stypeobj.Count() == 0)
                                    {
                                        sele4 = sele3.CreateFirst(DbElementTypeInstance.SELEC);
                                        sele4.Copy(stypeitem);
                                    }
                                    else
                                        sele4 = stypeobj.First();
                                    sele4.InsertBeforeFirst(sele3);
                                    try
                                    {
                                        sele4.SetAttribute(DbAttributeInstance.NAME, type_name);
                                    }catch(Exception ee)
                                    {

                                    }

                                    //5번째 셀레-SPCO
                                    foreach (DbElement spco in stypeitem.Members())
                                    {
                                        DbElement sele5 = null;

                                        string compname = spco.GetAsString(DbAttributeInstance.CATR).Replace("-SCOM", "");
                                        type_name = "/" + specname + compname;
                                        if (sele4.Members().Count() == 0)
                                            sele5 = sele4.CreateLast(DbElementTypeInstance.SPCOMPONENT);
                                        else
                                        {
                                            sele5 = sele4.Members().First();
                                        }
                                        sele5.Copy(spco);
                                        try
                                        {
                                            sele5.SetAttribute(DbAttributeInstance.NAME, type_name);
                                        }catch(Exception ee)
                                        { }


                                    }
                                }
                            }
                        }
                    }


                }
                catch (Exception ee)
                {

                }

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ClearEmptySele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);




        }
        DataTable valvetype_dt = new DataTable();
        private void btn_change_vv_type_Click(object sender, EventArgs e)
        {

            STYPE_Configuration(CurrentElement.Element);

        }
        //Valve subtype타입을 AM_STYPE에 정의된 대로 바꾸고 테이블에 정의안된 valve는 삭제하는 부분임.
        private void STYPE_Configuration(DbElement BaseElement)
        {
            try
            {
                List<String> deletelist = new List<String>();
                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SPCOMPONENT };
                TypeFilter filter = new TypeFilter(dbtype);
                object para = (object)0;

                AttributeIntFilter filter2 = new AttributeIntFilter(DbAttributeInstance.DDEP, FilterOperator.Equals, 6);
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection valve_collection = new DBElementCollection(BaseElement, finalfilter);


                foreach (DbElement element in valve_collection)
                {
                    //string tanswer = element.GetAsString(DbAttributeInstance.TANS);
                    string catref = element.GetAsString(DbAttributeInstance.CATR);
                    string compgroup = catref.Split('-')[0].Substring(1);
                    DataRow[] rows = valvetype_dt.Rows.Cast<DataRow>().Where(row => row["COMP_GROUP"].ToString() == compgroup).ToArray();


                    //pv로 시작하는 애들중 spco가 unname되어있는 애들을 지우기 위한 리스트 만듬.
                    if (!element.GetBool(DbAttributeInstance.ISNAME) && catref.Substring(1, 2) == "PV")
                    {
                        if (!deletelist.Contains(element.Owner.GetAsString(DbAttributeInstance.REF)))
                            deletelist.Add(element.Owner.GetAsString(DbAttributeInstance.REF));
                    }

                    try
                    {

                        //테이블에 정의한 stype이 있는 경우 tanswer값을 그대로 지정함.
                        if (rows.Count() > 0 && rows[0]["STYPE"].ToString() != "")
                        {
                            element.Owner.SetAttribute(DbAttributeInstance.TANS, rows[0]["STYPE"].ToString());
                            //element.SetAttribute(DbAttributeInstance.TANS, rows[0]["STYPE"].ToString());
                        }
                        else
                        {
                            //테이블에 없는 밸브는 삭제함.
                            if (catref.Substring(1, 2) == "PV")
                            {
                                if (!deletelist.Contains(element.Owner.GetAsString(DbAttributeInstance.REF)))
                                    deletelist.Add(element.Owner.GetAsString(DbAttributeInstance.REF));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("11");
                    }

                }

                //정의되지 않은 안쓰는 VAlve 지우기 
                foreach (string item in deletelist)
                {
                    DbElement.GetElement(item).Delete();
                    //item.Delete();
                }
            }
            catch (Exception ee)
            { }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            DbElement base_mach = CurrentElement.Element;
            DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SPCOMPONENT };
            TypeFilter filter = new TypeFilter(dbtype);
            object para = (object)0;

            AttributeIntFilter filter2 = new AttributeIntFilter(DbAttributeInstance.DDEP, FilterOperator.Equals, 6);
            AndFilter finalfilter = new AndFilter();
            finalfilter.Add(filter);
            finalfilter.Add(filter2);
            DBElementCollection mach_coll = new DBElementCollection(base_mach, finalfilter);


            foreach (DbElement element in mach_coll)
            {
                string catref = element.GetAsString(DbAttributeInstance.CATR).Substring(1);
                if (catref.Substring(0, 2) == "PV")
                {
                    string compgroup = catref.Split(new char[] { '-' })[0];
                    element.Owner.SetAttribute(DbAttributeInstance.TANS, compgroup);
                }

            }
        }

        private void SpecGen_Form_Load(object sender, EventArgs e)
        {
            OdbcConnection conn = new OdbcConnection("dsn=mis-1;uid=tbdb01;pwd=tbdb01");
            string query = "select * from AM_STYPE";
            OdbcCommand cmd = new OdbcCommand(query, conn);
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);

            da.Fill(valvetype_dt);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SortbySize(CurrentElement.Element, DbAttributeInstance.ANSW);
        }

        private void SortbyFieldForCE(DbElement targetElement, DbAttribute att)
        {
            try
            {
                DbElement baseelement = CurrentElement.Element;
                DbElement[] sortedmember = baseelement.Members().OrderBy(x => x.GetDouble(DbAttributeInstance.ANSW)).ToArray();

                for (int i = 0; i < sortedmember.Length; i++)
                {
                    sortedmember[i].InsertAfterLast(baseelement);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
        }


        //특정 element아래에 잇는 모든애들중에 QUESTION이 PBORE 인 애들을 모두 정렬하는 기능.
        //Size를 기준으로 정렬
        private void SortbySize(DbElement targetElement, DbAttribute att)
        {
            try
            {


                DbElement baseelement = targetElement;
                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC };
                TypeFilter filter = new TypeFilter(dbtype);
                object para = (object)0;

                AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.QUES, FilterOperator.Equals, "PBOR");
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection mach_coll = new DBElementCollection(baseelement, finalfilter);
                foreach (DbElement item in mach_coll)
                {
                    DbElement[] sortedmember = item.Members().OrderBy(x => x.GetDouble(att)).ToArray();

                    for (int i = 0; i < sortedmember.Length; i++)
                    {
                        sortedmember[i].InsertAfterLast(item);
                    }
                }

                
            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {


                DbElement baseelement = CurrentElement.Element;
                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC };
                TypeFilter filter = new TypeFilter(dbtype);
                object para = (object)0;

                AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.QUES, FilterOperator.Equals, "ANGL");
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection mach_coll = new DBElementCollection(baseelement, finalfilter);
                foreach (DbElement item in mach_coll)
                {
                    foreach (DbElement subitem in item.Members())
                    {
                        subitem.SetAttribute(DbAttributeInstance.ANSW,1.0);
                        subitem.SetAttribute(DbAttributeInstance.MAXA, 90.0);
                    }

                    //DbElement[] sortedmember = item.Members().OrderBy(x => x.GetDouble(att)).ToArray();

                    //for (int i = 0; i < sortedmember.Length; i++)
                    //{
                    //    sortedmember[i].InsertAfterLast(item);
                    //}
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {


                DbElement baseelement = CurrentElement.Element;

                return;

                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC};
                TypeFilter filter = new TypeFilter(dbtype);
                
                object para = (object)0;

                AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.TANS, FilterOperator.Equals, "ATT");
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection mach_coll = new DBElementCollection(baseelement, finalfilter);
                DbElement[] elementlist = mach_coll.Cast<DbElement>().ToArray();



                foreach (DbElement item in elementlist)
                {
                    DbElement spco =item.FirstMember();
                    spco.SetAttribute(DbAttributeInstance.DETR, "/PA00-SDTE");
                    //item.SetAttribute(DbAttributeInstance.TANS, "POWELD");

                    //DbElement[] sortedmember = item.Members().OrderBy(x => x.GetDouble(att)).ToArray();

                    //for (int i = 0; i < sortedmember.Length; i++)
                    //{
                    //    sortedmember[i].InsertAfterLast(item);
                    //}
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
            //DateTime xx=CurrentElement.Element.GetDateTime(DbAttributeInstance.LASTM);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {


                DbElement baseelement = CurrentElement.Element;
                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.SELEC };
                TypeFilter filter = new TypeFilter(dbtype);
                object para = (object)0;

                AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.TANS, FilterOperator.Equals, "WELD");
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection mach_coll = new DBElementCollection(baseelement, finalfilter);
                DbElement[] elementlist = mach_coll.Cast<DbElement>().ToArray();



                foreach (DbElement item in elementlist)
                {

                    dbtype = new DbElementType[] { DbElementTypeInstance.SPCOMPONENT };
                    filter = new TypeFilter(dbtype);                    

                    
                    finalfilter.Add(filter);
                    DBElementCollection zcoll = new DBElementCollection(item, filter);

                    foreach (DbElement spco in zcoll)
                    {
                        
                        spco.SetAttribute(DbAttributeInstance.TANS,"FALS");
                        
                    }

                    //DbElement[] sortedmember = item.Members().OrderBy(x => x.GetDouble(att)).ToArray();

                    //for (int i = 0; i < sortedmember.Length; i++)
                    //{
                    //    sortedmember[i].InsertAfterLast(item);
                    //}
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
        }

        private DbSaveWork mSDbSaveWork;
        private void button8_Click(object sender, EventArgs e)
        {
            mSDbSaveWork = new DbSaveWork();
            mSDbSaveWork.StartTracking();
            mSDbSaveWork.Saveworked += SDbSaveWorkSaveworked;
        }
        void SDbSaveWorkSaveworked()
        {
            MessageBox.Show("Db에 변경이 있고 Savework이 되었습니다.!!");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DbElement ce = CurrentElement.Element;
            try
            {
                ce.SetAttribute(DbAttributeInstance.DETR, DbElement.GetElement("/PPCO-15-SDTE"));
            }catch(Exception ee)
            {
                Console.WriteLine("ㅇㄹ");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {



                DbElement baseelement = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);
                DbElementType[] dbtype = new DbElementType[] { DbElementTypeInstance.COCO };
                TypeFilter filter = new TypeFilter(dbtype);



                AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.CTYP, FilterOperator.Contains, txtconn.Text);
                AndFilter finalfilter = new AndFilter();
                finalfilter.Add(filter);
                finalfilter.Add(filter2);
                DBElementCollection mach_coll = new DBElementCollection(baseelement, finalfilter);
                DbElement[] elementlist = mach_coll.Cast<DbElement>().ToArray();



                foreach (DbElement item in elementlist)
                {
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p "+ item.ToString()).RunInPdms();
                    
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }
        }
        
    }
}
