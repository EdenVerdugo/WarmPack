using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Core.Web
{
    public class FtpClient
    {
        private readonly string _ftp;
        private readonly string _user;
        private readonly string _password;

        public FtpClient(string ftpDirectory, string user, string password)
        {
            _ftp = ftpDirectory;
            _user = user;
            _password = password;
        }

        public FtpWebResponse Upload(Uri filePath, string folder = "")
        {
            if(folder != "")
            {
                if (!folder.EndsWith("/"))
                {
                    folder += "/";
                }
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create( _ftp + "/" + folder + filePath.Segments.Last());
            request.Method = WebRequestMethods.Ftp.UploadFile;
            
            request.Credentials = new NetworkCredential(_user, _password);
            
            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(filePath.LocalPath))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response;
            }
        }


        public FtpWebResponse Delete(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftp + fileName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(_user, _password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response;
            }
        }

        public static FtpWebResponse Create(string ftpDirectory, string user, string password, string fileName, FtpClientMethod method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpDirectory + fileName);
            request.Method = GetFtpMethod(method);
            request.Credentials = new NetworkCredential(user, password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response;
            }
        }

        private static string GetFtpMethod(FtpClientMethod method)
        {
            switch (method)
            {
                case FtpClientMethod.AppendFile:
                    return WebRequestMethods.Ftp.AppendFile;                    

                case FtpClientMethod.DeleteFile:
                    return WebRequestMethods.Ftp.DeleteFile;

                case FtpClientMethod.DownloadFile:
                    return WebRequestMethods.Ftp.DownloadFile;

                case FtpClientMethod.GetDateTimestamp:
                    return WebRequestMethods.Ftp.GetDateTimestamp;

                case FtpClientMethod.GetFileSize:
                    return WebRequestMethods.Ftp.GetFileSize;

                case FtpClientMethod.ListDirectory:
                    return WebRequestMethods.Ftp.ListDirectory;

                case FtpClientMethod.ListDirectoryDetails:
                    return WebRequestMethods.Ftp.ListDirectoryDetails;

                case FtpClientMethod.MakeDirectory:
                    return WebRequestMethods.Ftp.MakeDirectory;

                case FtpClientMethod.PrintWorkingDirectory:
                    return WebRequestMethods.Ftp.PrintWorkingDirectory;

                case FtpClientMethod.RemoveDirectory:
                    return WebRequestMethods.Ftp.RemoveDirectory;

                case FtpClientMethod.Rename:
                    return WebRequestMethods.Ftp.Rename;

                case FtpClientMethod.UploadFile:
                    return WebRequestMethods.Ftp.UploadFile;

                default:
                    return WebRequestMethods.Ftp.UploadFileWithUniqueName;
            }
        }
    }

    public enum FtpClientMethod
    {
        AppendFile,
        DeleteFile,
        DownloadFile,
        GetDateTimestamp,
        GetFileSize,
        ListDirectory,
        ListDirectoryDetails,
        MakeDirectory,
        PrintWorkingDirectory,
        RemoveDirectory,
        Rename,
        UploadFile,
        UploadFileWithUniqueName
    }
}
