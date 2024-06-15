using HotelListing.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Models.Dto
{
    public class EntityDto : EntityDto<int>
    {
        /// <summary>
        /// Creates a new <see cref="EntityDto"/> object.
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(int id)
            : base(id)
        {
        }
    }
    public class EntityDto<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }
}
