using AutoLotDAL.Models.Base;

namespace AutoLotDAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order : EntityBase
    {
        public int CustomerId { get; set; }

        public int CarId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(CarId))]
        public virtual Inventory Inventory { get; set; }
    }
}
