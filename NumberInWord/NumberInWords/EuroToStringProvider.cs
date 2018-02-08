namespace NumberInWords
{
    /// <summary>
    /// Преобразование европейских денег (евро + центы) в сумму прописью
    /// </summary>
    public class EuroToStringProvider : MoneyToStringProviderBase
    {
        public EuroToStringProvider(bool shortEuro, bool shortCent, bool digitCent) :
            base(shortEuro, shortCent, digitCent)
        { }
        // варианты написания долларов
        private static readonly string[] Dollars = { "євро", "євро", "євро" };
        // варианты написания центов
        private static readonly string[] Cents = { "євро цент", "євро цента", "євро центів" };
        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return NumberToRussianString.WordGender.Neuter;
        }
        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? Dollars[(int)wordMode] : Cents[(int)wordMode];
        }
        protected override string GetShortName(bool high)
        {
            return high ? "євро." : "євро ц.";
        }
    }
}