using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MT.CSGPortal.Entities.ViewModels;

namespace MT.CSGPortal.Entities.DTOs
{
    public class SearchResult<TEntity>
    {
        private List<TEntity> entityObj;

        public List<TEntity> ResultData { get { return entityObj;} set {entityObj = value; }}
        public int TotalRecordCount { get; set; }
        public bool EndOfRecords { get; set; }
    }    
}
