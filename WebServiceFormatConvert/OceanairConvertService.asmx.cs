using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServiceFormatConvert
{
    /// <summary>
    /// OceanairConvertService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class OceanairConvertService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string ConvertToOceanair()
        {
            var fPath = Server.MapPath("~/SourceDoc/") + "Message_COHCFSTRN_8685c08d-bb35-41c8-99c5-4a103699e69d.xml";
                if (File.Exists(fPath))
                {
                    var XmlOcebos = ConvertHelper.CargowiseToOCEBOS(fPath);
                    var swbNum = XmlOcebos.Root.Element("HouseBillNumber").Value;
                    var ftpSiteDescr = "OCEBOS";
                    var path =   Server.MapPath("~/Output/" )+ ftpSiteDescr;
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    var upfilename = swbNum.Replace("@", "_").Replace("~", "_").Replace(@"/", "_").Replace("<", "_").Replace("*", "_").Replace(">", "_")
                            + "_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xml";
                    XmlOcebos.Save(path + "\\" + upfilename);
                    return path + "\\" + upfilename;
                }
                else
                    return "no this file";
        }
    }
}
