using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Data.Entity
{
    public interface IAuditedEntity
    {
        DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator of this entity.
        /// </summary>
        long? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
       DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        long? LastModifierUserId { get; set; }

        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
