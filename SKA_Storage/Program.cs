using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Drawing;

using IntAirAct;

// Add using statements to access AWS SDK for .NET services. 
// Both the Service and its Model namespace need to be added 
// in order to gain access to a service. For example, to access
// the EC2 service, add:
// using Amazon.EC2;
// using Amazon.EC2.Model;

namespace SKA_Storage
{
    class Program
    {
        
       public static string BUCKET_NAME = "Images1985";
        public static string S3_KEY;
        public const string File_Path = @"C:\Users\Mohammad\Documents\Visual Studio 2012\Projects\SKA_Storage\hii.pdf";


        public static AmazonS3 GetS3Client()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(
                    appConfig["AWSAccessKey"],
                    appConfig["AWSSecretKey"]
                    );
            return s3Client;
        }
        // Parse the input request



        /// <summary>
        /// //////////////////////////////Create a new bucket//////////////////////////////
        /// </summary>
        /// <param name="client"></param>
        public static void CreateBucket(AmazonS3 client, string cmap)
        {
            Console.Out.WriteLine("Checking S3 bucket with name " + cmap);

            ListBucketsResponse response = client.ListBuckets();

            bool found = false;
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName == cmap)
                {
                    Console.Out.WriteLine("   Bucket found will not create it.");
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                Console.Out.WriteLine("   Bucket not found will create it.");

                client.PutBucket(new PutBucketRequest().WithBucketName(cmap));

                Console.Out.WriteLine("Created S3 bucket with name " + cmap);
            }
        }

        /// <summary>
        /// ///////////////////Create a new Folder///////////////////////////////
        /// </summary>
        /// <param name="client"></param>
        public static void CreateNewFolder(AmazonS3 client, string cmap, string clipping)
        {
            String FolderKey = clipping + "/";
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(cmap);
            request.WithKey(FolderKey);
            request.WithContentBody("");
            client.PutObject(request);
        }

        /// <summary>
        /// /////////////////////Create a new file in folder/////////////////
        /// </summary>
        /// <param name="client"></param>
        public static void CreateNewFileInFolder(AmazonS3 client, string cmap, string clipping)
        {

            String FolderKey = clipping + "/" + "Demo Create File.txt";
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(cmap);
            request.WithKey(S3_KEY);
            request.WithContentBody("This is body of S3 object.");
            client.PutObject(request);
        }


        /// <summary>
        /// ////////////////// Upload a new file//////////////////////////////////
        /// </summary>
        /// <param name="client"></param>
        public static void UploadFile(AmazonS3 client)
        {
            //S3_KEY is name of file we want upload
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(BUCKET_NAME);
            request.WithKey(S3_KEY);
            //request.WithInputStream(MemoryStream);
            request.WithFilePath(File_Path);
            client.PutObject(request);
            //Console.WriteLine("Yay");

        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        /// <summary>
        /// /////////////////////get object and store on local server////////////////////////
        /// </summary>
        /// <param name="s3Client"></param>
        /// <returns></returns>

        public static byte[] getobject(AmazonS3 s3Client)
        {

            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = BUCKET_NAME;
            request.Key = S3_KEY;
            GetObjectResponse res = s3Client.GetObject(request);
            res.WriteResponseStreamToFile(@"C:\Users\Mohammad\Desktop\Zahra.jpg");
            Image img = Image.FromFile(@"C:\Users\Mohammad\Desktop\Zahra.jpg");
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();



        }

        /// <summary>
        /// ////////////////////////Get Image///////////////////////////////////
        /// </summary>
        /// <param name="s3Client"></param>
        /// <returns></returns>
        public static byte[] GetFile(AmazonS3 s3Client, string cmap, string clipping)
        {

            using (s3Client)
            {

                MemoryStream file = new MemoryStream();
                try
                {
                    GetObjectResponse r = s3Client.GetObject(new GetObjectRequest()
                    {
                        BucketName = cmap,
                        Key = clipping + "/" + S3_KEY
                    });
                    try
                    {


                        // long transferred = 0L;
                        BufferedStream stream2 = new BufferedStream(r.ResponseStream);
                        byte[] buffer = new byte[0x2000];
                        int count = 0;
                        while ((count = stream2.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            file.Write(buffer, 0, count);

                        }

                    }
                    finally
                    {
                    }
                    Console.WriteLine();
                    return file.ToArray();

                }
                catch (AmazonS3Exception)
                {
                    Console.WriteLine("Oops!");
                }
            }
            return null;
        }



        /// <summary>
        /// /////////////////////List all of objects in a bucket//////////////////////
        /// </summary>
        /// <param name="s3Client"></param>
        /// <returns></returns>

        public static string SliceNumbers(AmazonS3 s3Client, string cmap, string clipping)
        {

            int count = 0;

            try
            {
                //ListBucketsRequest buckr = new ListBucketsRequest();
                //ListBucketsResponse response = s3Client.ListBuckets(buckr);

                
                    ListObjectsRequest Lor = new ListObjectsRequest()
                    {
                        BucketName = cmap,
                        Prefix= clipping

                        //with Delimiter is '/', it will not get folder. {we need just count files in a bucket which are not forlsers!
                        //Delimiter= "/"
                    };
                    ListObjectsResponse response1 = s3Client.ListObjects(Lor);
                    foreach (S3Object s3Object in response1.S3Objects)
                    {
                        count++;
                    }
               
            }


            catch (AmazonS3Exception ex)
            {
                //Show Exception
            }


            return count.ToString();

        }

        public static String MakeUrl(AmazonS3 s3Client)
        {
            string preSignedURL = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = BUCKET_NAME,
                Key = S3_KEY,
                Expires = System.DateTime.Now.AddMinutes(30),
                Protocol = Protocol.HTTP

            });

            //Console.WriteLine(preSignedURL);

            return preSignedURL;
        }



        public static byte[] GetImage(int sliceNumber, string cmap, string clipping)
        {
            
            AmazonS3 s3Client = Program.GetS3Client();
            S3_KEY = sliceNumber + ".jpg";
            var a = GetFile(s3Client, cmap, clipping);

            return a;
        }

        /* public static string GetImage()
         {
             //Program WebProgram = new Program();
             AmazonS3 s3Client = Program.GetS3Client();
             S3_KEY = Slice_Number + ".jpg";
             var a = GetFile(s3Client);
             List<String> list = new List<String>();

             foreach (byte b in a)
             {
                 list.Add(b.ToString());

             }
             string dogCsv = string.Join(",", list.ToArray());
             //return list[8];
             return dogCsv;
         }

             public AmazonS3 s3Client { get; set; }

         }*/

    }
        class WebServer
        {
            static void Main(string[] args)
            {
                Program p = new Program();
                AmazonS3 s3Client = Program.GetS3Client();
                //Program.CreateNewFolder(s3Client, Program.BUCKET_NAME, "first folder");
                //Program.CreateBucket(s3Client);   

                IAIntAirAct intAirAct = IAIntAirAct.New();

                IARoute imageRoute = IARoute.Get("/SKA/image/{slicenumber}");
                IARoute imageRouteParameters = IARoute.Get("/SKA/image/{slicenumber}/{cmap}/{clipping}");
                IARoute numberOfSlicesRoute = IARoute.Get("/SKA/numberOfSlices/{cmap}/{clipping}");


                intAirAct.Route(numberOfSlicesRoute, delegate(IARequest request, IAResponse response)
                {
                    try
                    {
                        string cmap = request.Parameters["cmap"];
                        string clipping = request.Parameters["clipping"];
                        response.SetBodyWithString(Program.SliceNumbers(s3Client, cmap, clipping));
                    }
                    catch
                    {
                        response.StatusCode = 404;
                    }
                });

                intAirAct.Route(imageRouteParameters, delegate(IARequest request, IAResponse response)
                {
                    try
                    {
                        int sliceNumber = Convert.ToInt32(request.Parameters["slicenumber"]);
                        string cmap = request.Parameters["cmap"];
                        string clipping = request.Parameters["clipping"];

                        byte[] data = Program.GetImage(sliceNumber,cmap, clipping);

                        response.Body = data;
                        response.ContentType = "image/jpeg";
                    }
                    catch
                    {
                        response.StatusCode = 404;
                    }
                });


                intAirAct.Start();
                //WebServer ws = new WebServer(SendResponse, "http://localhost:8080/");
                //ws.Run();
                Console.WriteLine("");
                Console.WriteLine("A simple webserver. Press a key to quit.");
                Console.ReadKey();

                //ws.Stop();

            }
        }


    
}
