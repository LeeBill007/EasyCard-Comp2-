
//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------


namespace EasyCardSystem.Models
{

using System;
    using System.Collections.Generic;
    
public partial class RecordData
{

    public int RecordID { get; set; }

    public string EmpID { get; set; }

    public string CardID { get; set; }

    public System.DateTime TimeLend { get; set; }

    public Nullable<System.DateTime> TimeReturn { get; set; }

    public int UseDay { get; set; }

    public Nullable<int> TotalSpent { get; set; }

    public string RecordDisable { get; set; }

    public string RecordState { get; set; }

}

}
