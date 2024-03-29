//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebPubApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class OrgBranch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrgBranch()
        {
            this.Authorships = new HashSet<Authorship>();
        }
    
        public int ID { get; set; }
        [Display(Name = "Organization branch")]
        public string Title { get; set; }
        public int OrgID { get; set; }
        public int CountryID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Authorship> Authorships { get; set; }
        public virtual Country Country { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
