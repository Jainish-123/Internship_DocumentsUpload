using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SampleWebApi_8.Controllers
{
    public class documentUploadController : ApiController
    {
        List<string> validExtensions = new List<string>() {".txt", ".docx", ".pdf", ".pptx", ".csv"};
        string upFileName, ext, filepath;


        [Route("api/documentUpload/uploadDocument")]
        [HttpPost]
        public string uploadDocument()          //public async Task<string> uploadDocument()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                HttpContext context = HttpContext.Current;

                string root = context.Server.MapPath("~/App_Data");

                //MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    upFileName = Path.GetFileName(file.FileName).Trim('"');

                    ext = Path.GetExtension(upFileName);

                    if ((file.ContentLength < 1000000) && (validExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    else
                    {
                        return $"{upFileName} : Document size is too large or Document type is not supported...!";
                    }
                }

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    upFileName = Path.GetFileName(file.FileName).Trim('"');

                    ext = Path.GetExtension(upFileName);

                    filepath = Path.Combine(root, upFileName);

                    HttpContext.Current.Request.Files[i].SaveAs(filepath);

                    //InsertDocumentData(filepath);
                }

                //await Request.Content.ReadAsMultipartAsync(provider);

                //foreach (var fi in provider.FileData)
                //{
                //    string name = fi.Headers.ContentDisposition.FileName.Trim('"');

                //    string localFileName = fi.LocalFileName;

                //    string filepath = Path.Combine(root, name);

                //    File.Move(localFileName, filepath);

                //    //InsertDocumentData(filepath);
                //}


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Document uploaded.";
        }
    }
}
