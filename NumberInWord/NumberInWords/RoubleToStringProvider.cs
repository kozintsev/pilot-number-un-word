namespace NumberInWords
{
    /// <summary>
    /// Преобразование русских денег (рубли + копейки) в сумму прописью
    /// </summary>
    public class RoubleToStringProvider : MoneyToStringProviderBase
    {
        public RoubleToStringProvider(bool shortRoubles, bool shortCopecks, bool digitCopecks) :
            base(shortRoubles, shortCopecks, digitCopecks)
        { }
        // варианты написания рублей
        private static readonly string[] Roubles = { "рубль", "рубля", "рублів" };
        // варианты написания копеек
        private static readonly string[] Copecks = { "копійка", "копійки", "копійок" };
        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return high ? NumberToRussianString.WordGender.Masculine : NumberToRussianString.WordGender.Feminine;
        }
        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? Roubles[(int)wordMode] : Copecks[(int)wordMode];
        }
        protected override string GetShortName(bool high)
        {
            return high ? "руб." : "коп.";
        }
    }
}