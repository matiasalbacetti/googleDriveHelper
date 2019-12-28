using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveHelper
{
    public class GoogleDriveService
    {
        private static DriveService service;

        public static DriveService getService() //Obtener el servicio de Google Sheets autentificado
        {
            if (service == null)
            {

                string[] Scopes = { DriveService.Scope.Drive }; //Este scope es de lectura y escritura
                string ApplicationName = "MatiDrive"; //El nombre de la aplicación registrada en la consola de desarrolladores de Google

                UserCredential credential;

                //client_secret.json es el archivo de credenciales de la aplicación costos.calc registrada en la consola de desarrolladores de Google
                using (var stream = new FileStream("Credentials/client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "Credentials"; //Ruta en la que se van a guardar las credenciales

                    //Obtención de autorización de Google
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                // Create Google Sheets API service.
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }

            return service;
        }

        public static DriveFolder getFolderInfo(string folderId, string folderName)
        {
            var folder = new DriveFolder(folderId, folderName);

            folder.id = folderId;
            folder.name = folderName;

            var request = getService().Files.List();

            request.Q = "'" + folderId + "' in parents and trashed = false";
            request.PageSize = 1000;

            FileList response = new FileList();

            List<File> files = new List<File>();

            do
            {
                request.PageToken = response.NextPageToken;

                response = request.Execute();

                files.AddRange(response.Files);

            } while (!String.IsNullOrWhiteSpace(response.NextPageToken));

            var subFolders = files.Where(f => f.MimeType == "application/vnd.google-apps.folder").ToList();

            folder.filesNumber = files.Count - subFolders.Count;

            foreach (var subFolder in subFolders)
            {
                folder.subFolders.Add(getFolderInfo(subFolder.Id, subFolder.Name));
            }

            return folder;
        }
    }
}
