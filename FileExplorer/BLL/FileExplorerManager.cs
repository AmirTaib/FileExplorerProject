using System;
using BOL;
using DAL;

namespace BLL
{
    public class FileExplorerManager
    {
        /// <summary> 
        /// Creates a new instance of the class 'SearchDetail' from EF
        /// and returns the "SearchID" before displaying the results. 
        /// </summary>
        /// 
        /// <param name="searchDetailModel"></param>
        /// Contains user search information
        /// 
        /// <returns>Returns "SearchID" to the 'SearchResultModel'</returns>
        /// 
        public int AddNewSearch(SearchDetailModel searchDetailModel)
        {
            using (FileExplorerEntities ef = new FileExplorerEntities())
            {
                Search_Detail newSearchDetail = new Search_Detail
                {
                    DirectoryPath = searchDetailModel.DirectoryPath,
                    SearchTime = searchDetailModel.SearchTime,
                    KeyWord = searchDetailModel.KeyWord
                };

                ef.Search_Details.Add(newSearchDetail);
                ef.SaveChanges();
                return newSearchDetail.SearchDetailsID;
            }
        }
        
        /// <summary>
        /// Creates a new instance of the class 'SearchResult' from EF
        /// </summary>
        /// 
        /// <param name="searchResult"></param>
        /// Contains user 'SearchResult' to the db.
        /// 
        public void UpdateDbResults(SearchResultModel searchResult)
        {
            using (FileExplorerEntities ef = new FileExplorerEntities())
            {
                Search_Result newSearchResults = new Search_Result
                {
                    SearchDetailsID = searchResult.SearchId,
                    FullPath = searchResult.FullPath
                };
                ef.Search_Results.Add(newSearchResults);
                ef.SaveChanges();
            }
        }
    }
}
