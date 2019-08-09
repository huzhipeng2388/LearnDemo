using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace QiNiuDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public List<Object> UploadQiniu()
        {
            Mac mac = new Mac("xxx", "xxx");// AK SK使用
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = "xxx"; //仓储名
            string token = Auth.CreateUploadToken(mac,putPolicy.ToJsonString());//token生成

            IFormFileCollection files = Request.Form.Files;

            Config config = new Config()
            {
                Zone = Zone.ZONE_CN_East,
                UseHttps = true
            };
            var res = Request.Form.ToArray();

            FormUploader upload = new FormUploader(config);
            HttpResult result = new HttpResult();
            List<Object> list = new List<Object>();
            foreach (IFormFile file in files)//获取多个文件列表集合
            {
                if (file.Length > 0)
                {
                    var _fileName = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    var _qiniuName = Guid.NewGuid().ToString("D"); //重命名
                    Stream stream = file.OpenReadStream();
                    result = upload.UploadStream(stream, _qiniuName, token, null);
                    if (result.Code == 200)
                    {

                        list.Add(new {statu = 200,filePath = "xxx外链域名xxx" + _qiniuName });
                    }
                    else
                    {
                        throw new Exception(result.RefText);//上传失败错误信息
                    }
                }
            }
            return list;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
