using BLL.Args;
using BOL;
using System;
using System.Collections.Generic;
using System.IO;

namespace BLL
{
    /// <summary>
    /// FileResulteManeger handle the search logic only
    /// </summary>
    public class FilesResultManager : SearchResultModel
    {
        /// <summary>
        /// 'errorDirMsg' & 'errorFileMsg' contain the error messages and if we want we can show them in the Program only
        /// </summary>
        List<string> errorDirMsg = new List<string>();
        List<string> errorFileMsg = new List<string>();

        /// <summary>
        /// Counts the number of results to notify the user if no results match the search.
        /// </summary>
        public event EventHandler<FileFoundEventArgs> FileFound;

        /// <summary>
        /// A function protected containing the Event 'FileActionHandler' 
        /// ==> So that the event can also be used in the inheritens class.
        /// </summary>
        /// 
        /// <param name="filePath"></param>
        /// Contains result of the user search.
        /// 
        protected virtual void OnFileActionHandler(string filePath)
        {
            FileFound?.Invoke(this, new FileFoundEventArgs(filePath));
        }

        /// <summary>
        ///  Recursive function,
        ///  The search function that searches on the user's computer 
        ///  and throws an event whenever a file matching the search word is found.
        /// </summary>
        /// 
        /// <param name="originalDirectory"></param>
        /// The primary directory where the user wants to perform the search.
        /// <param name="directory"></param>
        /// Current directory in which the search is performed
        /// <param name="filePath"></param>
        /// The word that the user is looking for
        /// 
        public void GetAllFiles(string originalDirectory, string directory, string filePath)
        {
            try
            {
                foreach (var f in Directory.GetFiles(directory, $"*{filePath}*"))
                {
                    OnFileActionHandler(f);
                }

                foreach (var d in Directory.GetDirectories(directory))
                {
                    try
                    {
                        GetAllFiles(originalDirectory, d, filePath);
                    }
                    catch (Exception e)
                    {
                        errorDirMsg.Add(e.Message.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                errorFileMsg.Add(e.Message.ToString());
            }
        }
    }
}
