using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCardSystem.Models
{
    public class RecordDataViewModel
    {
        public string CardID { get; set; }
        public string CardDept { get; set; }
        public string CardName { get; set; }
        public string CardState { get; set; }
        public int CardAmount { get; set; }
        public string CardDisable { get; set; }

        public string EmpID { get; set; }
        public string EmpCardCode { get; set; }
        public string EmpDept { get; set; }
        public string EmpName { get; set; }
        public string EmpPwd { get; set; }
        public string Email { get; set; }
        public string EmpState { get; set; }
        public string Role { get; set; }
        public string EmpDisable { get; set; }

        
        public int RecordID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public System.DateTime TimeLend { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<System.DateTime> TimeReturn { get; set; }
        public int UseDay { get; set; }
        public Nullable<int> TotalSpent { get; set; }
        public string RecordDisable { get; set; }
        public string RecordState { get; set; }
    }
}