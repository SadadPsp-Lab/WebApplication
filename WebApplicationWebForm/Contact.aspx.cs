using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace WebApplicationWebForm
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string MID = "000000140235961";
                string MerchantId = "fFnSgtgWppg17ntADfoepx9uL6Dl16Bx";
                string TerminalId = "24000906";




                string Amount = "1000";
                string OrderId = new Random().Next(1,1000000).ToString();
                //string LocalDateTime = Now.ToShortDateString;
                string ReturnUrl = string.Format("{0}://{1}/About.aspx", Request.Url.Scheme, Request.Url.Authority);// "http://localhost/About.aspx";
                string SignData = "";
                dynamic dataBytes = Encoding.UTF8.GetBytes(string.Format("{0};{1};{2}", TerminalId, OrderId, Amount));
                dynamic symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;
                dynamic encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(MerchantId), new byte[8]);
                SignData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));
                string URLAuth = "https://sadad.shaparak.ir/VPG/api/v0/Request/PaymentRequest";
                //string postString = string.Format("MerchantId={0}&TerminalId={1}&Amount={2}&OrderId={3}&LocalDateTime={4}&ReturnUrl={5}&SignData={6}&AdditionalData=", MID, TerminalId, Amount, OrderId, LocalDateTime, ReturnUrl, SignData);
                var postString = new
                {
                    MerchantId = MID,
                    TerminalId = TerminalId,
                    Amount = Amount,
                    OrderId = OrderId,
                    ReturnUrl = ReturnUrl,
                    SignData = SignData
                };

                var content = JsonConvert.SerializeObject(postString);

                const string contentType = "application/json";
                System.Net.ServicePointManager.Expect100Continue = false;


                HttpWebRequest webRequest__1 = WebRequest.Create(URLAuth) as HttpWebRequest;
                webRequest__1.Method = "POST";
                webRequest__1.ContentType = contentType;
                webRequest__1.ContentLength = content.Length;
                StreamWriter requestWriter = new StreamWriter(webRequest__1.GetRequestStream());
                requestWriter.Write(content);
                requestWriter.Close();

                StreamReader responseReader = new StreamReader(webRequest__1.GetResponse().GetResponseStream());
                string responseData = responseReader.ReadToEnd();
                responseReader.Close();
                webRequest__1.GetResponse().Close();


                JObject myObj = JObject.Parse(responseData);
                string Token = myObj["Token"].ToString();
                Response.Redirect("https://sadad.shaparak.ir/VPG/Purchase/Index?token=" + Token);
                //https://sadad.shaparak.ir/VPG/api/v0/Advice/Verify
                //var cookie = Request.Cookies["Data"].Value;
                //var model = JsonConvert.DeserializeObject<PaymentRequest>(cookie);

                //var dataBytes = Encoding.UTF8.GetBytes(result.Token);

                //JValue rescode = myObj("ResCode");
                //JValue Token = myObj("Token");
                //if (rescode.ToString == "0")
                //{
                //    //Dim com As New SqlCommand
                //    //com.CommandText = "sp_insert_sadad_requests"
                //    //com.CommandType = System.Data.CommandType.StoredProcedure
                //    //com.Parameters.Clear()
                //    //com.Parameters.AddWithValue("@p_UID", funcs.UserID)
                //    //com.Parameters.AddWithValue("@p_amount", Amount)
                //    //com.Parameters.AddWithValue("@p_token", Token.ToString)
                //    //com.Parameters.AddWithValue("@p_SignData", SignData)
                //    //com.Parameters.AddWithValue("@p_sdate", Now)
                //    //com.Parameters.AddWithValue("@p_payed", False)
                //    //funcs.executeCustom(com)
                //    Response.Redirect("https://sadad.shaparak.ir/VPG/Purchase/Index?token=" + Token.ToString);
                //}

            }
            catch (WebException ex)
            {
                WebResponse errResp = ex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    //Response.Write(ex.Message + "<br/>" + text);
                }
            }

        }
    }
}