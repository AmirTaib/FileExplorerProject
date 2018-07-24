using BLL;
using BLL.Args;
using BOL;
using System;
using System.IO;

namespace UIL
{
    class Program
    {

        static FileExplorerManager fileManager = new FileExplorerManager();

        /// <summary> 
        /// This function takes the user's choice number (1 or 2) and the instance of the 'FilesResultManager' class, 
        /// receives user information from the user, 
        /// and calls the GetAllFiles() function to search for the results
        /// 
        /// This function contains the Event 'callback' Which creates a new show of 'SearchResultModel' and displays the result to the user.
        /// </summary>
        /// 
        /// <param name="numSelect"></param>
        ///  The requested number is 1 or 2
        ///  To know whether the user wants to search from "My Computer" or a specif specific folder.
        ///  
        /// <param name="filesResult"></param>
        /// An instance of the 'FilesResultManager' class
        /// containing the GetAllFiles() search function and the Event 'FileActionHandler'.
        /// 
        public static void SearchFileFromDirectory(byte numSelect, FilesResultManager filesResult)
        {
            string directoryName;
            string originalDirectory;
            bool isDirectoryExists;
            string keyword;
            int fileMatchFoundCounter = 0;

            if (numSelect == 1)
            {
                originalDirectory = "My Computer";
                Console.WriteLine("Enter file name to search:");
                keyword = Console.ReadLine().Trim();

                try
                {
                    int searchID = fileManager.AddNewSearch(new SearchDetailModel
                    {
                        SearchTime = DateTime.Now,
                        DirectoryPath = "My Computer",
                        KeyWord = keyword
                    });
                    fileMatchFoundCounter = 0;
                    EventHandler<FileFoundEventArgs> callback = (sender, fileFoundEventArgs) =>
                    {
                        fileMatchFoundCounter++;
                        Console.WriteLine($"{fileFoundEventArgs.FileFullPath}");
                        fileManager.UpdateDbResults(new SearchResultModel
                        {
                            FullPath = fileFoundEventArgs.FileFullPath,
                            SearchId = searchID
                        });
                    };

                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                    filesResult.FileFound += callback;
                    foreach (var drive in allDrives)
                    {
                        filesResult.GetAllFiles(originalDirectory, drive.Name, keyword);
                    }
                    filesResult.FileFound -= callback;
                }
                catch (Exception ex)
                {
                    WriteError(ex);
                }
            }
            else if (numSelect == 2)
            {
                Console.WriteLine(" Enter root directory to search in:");

                while (!(isDirectoryExists = Directory.Exists(directoryName = Console.ReadLine())))
                {
                    Console.Clear();
                    Console.WriteLine(" The requested folder does not exist \n" +
                                      " Please try again or select another folder");
                }

                originalDirectory = directoryName;
                Console.WriteLine(" Enter file name to search:");
                keyword = Console.ReadLine().Trim();

                int searchID = fileManager.AddNewSearch(new SearchDetailModel
                {
                    SearchTime = DateTime.Now,
                    DirectoryPath = originalDirectory,
                    KeyWord = keyword
                });
               
                EventHandler<FileFoundEventArgs> callback = (sender, fileFoundEventArgs) =>
                {
                    Console.WriteLine($"{fileFoundEventArgs.FileFullPath}");
                    fileMatchFoundCounter++;
                    try
                    {
                        fileManager.UpdateDbResults(new SearchResultModel
                        {
                            FullPath = fileFoundEventArgs.FileFullPath,
                            SearchId = searchID
                        });
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex);
                    }
                };

                filesResult.FileFound += callback;
                filesResult.GetAllFiles(originalDirectory, directoryName, keyword);
                filesResult.FileFound -= callback;
            }

            if (fileMatchFoundCounter== 0)
            {
                Console.WriteLine(" No files containing the requested string were found.");
            }

            Console.WriteLine();
            Console.WriteLine(" Search finished !! \n Want to continue?");
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// This function receives an error comment
        /// Replaces text color to red
        /// And returns to black after displaying the error
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// 
        public static void WriteError(Exception ex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Faild: {ex.Message}");
            Console.ForegroundColor = color;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(" HELLO! \n WELCOM TO THE FILE EXPLORER PROJECT \n");

            bool wantUse = true;

            while (wantUse)
            {
                FilesResultManager filesResult = new FilesResultManager();
                string keyNumToSearch;
                byte numSelect;
                bool isValidNumber = false;

                while (isValidNumber == false)
                {
                    Console.WriteLine(
                                  " 1. Enter file name to search. \n" +
                                  " 2. Enter file name to search + parent directory to search in. \n" +
                                  " 3. Exit");
                    keyNumToSearch = Console.ReadLine();
                    bool isNumber = byte.TryParse(keyNumToSearch, out numSelect);
                    Console.Clear();

                    if (!isNumber)
                    {
                        Console.WriteLine("Invalid character... \n" +
                            "Please insert a number between 1 and 3!");
                    }
                    else
                    {
                        numSelect = byte.Parse(keyNumToSearch);

                        if (numSelect == 1 || numSelect == 2)
                        {
                            SearchFileFromDirectory(numSelect, filesResult);
                            isValidNumber = true;
                        }
                        else if (numSelect == 3)
                        {
                            Console.WriteLine("GOOD BYE...");
                            isValidNumber = true;
                            wantUse = false;
                        }
                        else if (numSelect > 3 || numSelect < 1)
                        {
                            Console.WriteLine("Invalid number... \n" +
                                "Please insert a number between 1 and 3!");
                        }

                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
