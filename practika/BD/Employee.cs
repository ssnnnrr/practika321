//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace practika.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int tab_number { get; set; }
        public string code { get; set; }
        public string surname { get; set; }
        public string post { get; set; }
        public Nullable<decimal> salary { get; set; }
        public Nullable<int> boss { get; set; }
    
        public virtual Department Department { get; set; }
    }
}
