using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationWebForm
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var token = HttpContext.Current.Request.Form["Token"];
            var resCode = int.Parse(HttpContext.Current.Request.Form["ResCode"]);

            if (resCode == 0)
            {
                var dataBytes = Encoding.UTF8.GetBytes(token);

                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;

                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String("fFnSgtgWppg17ntADfoepx9uL6Dl16Bx"), new byte[8]);

                var signedData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));


                var data = new
                {
                    token = token,
                    SignData = signedData
                };

                var ipgUri = "https://sadad.shaparak.ir/api/v0/AdviceEx/Verify";

                var res = CallApi<VerifyResultData>(ipgUri, data);
                if (res != null && res.Result != null)
                {
                    if (res.Result.ResCode == "0")
                    {
                        var result = res.Result;
                        res.Result.Succeed = true;
                        Message.Text =  res.Result.Description;
                        ResCode.Text = res.Result.ResCode;
                        RRN.Text = res.Result.RetrivalRefNo;
                        Trace.Text = res.Result.SystemTraceNo;
                    }
                }
            }
        }

        public class VerifyResultData
        {
            public bool Succeed { get; set; }
            public string ResCode { get; set; }
            public string Description { get; set; }
            public string Amount { get; set; }
            public string RetrivalRefNo { get; set; }
            public string SystemTraceNo { get; set; }
            public string OrderId { get; set; }
            public string CardNo { get; set; }
        }

        public static async Task<T> CallApi<T>(string apiUrl, object value)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                var w = client.PostAsJsonAsync(apiUrl, value);
                w.Wait();
                HttpResponseMessage response = w.Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsAsync<T>();
                    result.Wait();
                    return result.Result;
                }
                return default(T);
            }
        }
    }
}