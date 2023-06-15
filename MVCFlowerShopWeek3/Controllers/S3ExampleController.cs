using Microsoft.AspNetCore.Mvc;

using Amazon; // for linkin gyour AWS account
using Amazon.S3; //For the bucket
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration; //appsettings.json section
using System.IO; // input output
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MVCFlowerShopWeek3.Controllers
{
    public class S3ExampleController : Controller
    {
        private const string S3BucketName = "mvcflowertp061828";

        //Function 2: Modifiy to become upload file page:
        public IActionResult Index()
        {
            return View();
        }

        //Function 1: Learn how to get back the keys from the appsetting.json file
        private List<string> getKeys()
        {
            List<string> Keys = new List<string>();

            var bulider = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configure = bulider.Build();

            Keys.Add(configure["Values:Key1"]);
            Keys.Add(configure["Values:Key2"]);
            Keys.Add(configure["Values:Key3"]);

            return Keys;
        }

        //Funtion 3: Learn how to upload single or multiple file(s) to S3:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessUploadImage(List<IFormFile> imageFile)
        {
            //Start Connection
            //1. add cerdential for 
            List<string> Keys = getKeys();
            AmazonS3Client s3agent = new AmazonS3Client(Keys[0], Keys[1], Keys[2],
                RegionEndpoint.USEast1);

            //File Validation
            //2. Read image by image an dstore to S3
            foreach (var singleimage in imageFile)
            {

                if (singleimage.Length <= 0)
                {
                    return BadRequest("File of " + singleimage.FileName +
                        "is no content!! Please try again!");
                }
                else if (singleimage.Length >= 1048576) //File mor ethan 1MB
                {
                    return BadRequest("File of" + singleimage.FileName +
                        "is more than 1MB!! Enter smaller one !!");
                }
                else if (singleimage.ContentType.ToLower() != "image/png"
                    && singleimage.ContentType.ToLower() != "image/jpeg")
                {
                    return BadRequest("File of " + singleimage.FileName +
                        "is not a valid image we need!! Give a valid one!!");
                }

                //pass the validation then submit/upload to the S3
                try
                {
                    //upload to S3
                    PutObjectRequest request = new PutObjectRequest //generate the request
                    {
                        InputStream = singleimage.OpenReadStream(),
                        BucketName = S3BucketName,
                        Key = "images/" + singleimage.FileName,
                        CannedACL = S3CannedACL.PublicRead

                    };
                    //send out the request - excute the request
                    await s3agent.PutObjectAsync(request);

                }

                catch (AmazonS3Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }

            //once done upoading, go back to the index page
            return RedirectToAction("Index", "S3Example");

        }
    }
}
