//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPF_SBAR
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encashment
    {
        public long encashmentID { get; set; }
        public System.DateTime encashmentDate { get; set; }
        public decimal encashmentSum { get; set; }
        public short departmentID { get; set; }
    
        public virtual Departments Departments { get; set; }
    }
}