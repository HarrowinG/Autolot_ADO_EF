using System.ComponentModel.DataAnnotations;

namespace AutoLotDAL.Models.Base
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        //reflect to SQL RowVersion for parallelism
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
