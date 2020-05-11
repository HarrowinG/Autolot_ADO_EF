namespace AutoLotEfConsoleApp.EF
{
    public partial class Car
    {
        public override string ToString()
        {
            return $"{CarNickName ?? "** No Name **"} is a {Color} {Make} with Id {CarId}.";
        }
    }
}
