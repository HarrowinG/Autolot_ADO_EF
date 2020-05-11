namespace AutoLotEfConsoleApp.EF
{
    public class ShortCar
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public override string ToString() => $"{Make} with Id {CarId}.";
    }
}
