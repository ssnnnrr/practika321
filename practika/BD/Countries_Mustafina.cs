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
    
    public partial class Countries_Mustafina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Countries_Mustafina()
        {
            this.Management_Mustafina = new HashSet<Management_Mustafina>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string capital { get; set; }
        public string continent { get; set; }
        public Nullable<int> square { get; set; }
        public Nullable<int> population { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Management_Mustafina> Management_Mustafina { get; set; }
    }
}
