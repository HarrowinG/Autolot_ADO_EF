using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLotDAL.Models
{
    public partial class Inventory
    {
        [NotMapped]
        public string MakeColor => $"{Make} + ({Color})";

        public override string ToString()
        {
            return $"{PetName ?? "** No Name **"} is a {Color} {Make} with Id {CarId}.";
        }
    }
}
