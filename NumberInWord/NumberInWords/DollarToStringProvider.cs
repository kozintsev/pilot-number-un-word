namespace NumberInWords
{
    /// <summary>
    /// Преобразование американских денег (доллары + центы) в сумму прописью
    /// </summary>
    public class DollarToStringProvider : MoneyToStringProviderBase
    {
        public DollarToStringProvider(bool shortDollar, bool shortCent, bool digitCent) :
            base(shortDollar, shortCent, digitCent)
        { }
        // варианты написания долларов
        private static readonly string[] dollars =
            { "долар", "долара", "доларів" };
        // варианты написания центов
        private static readonly string[] cents =
            { "цент", "цента", "центів" };
        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return NumberToRussianString.WordGender.Masculine;
        }
        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? dollars[(int)wordMode] : cents[(int)wordMode];
        }
        protected override string GetShortName(bool high)
        {
            return high ? "дол." : "ц.";
        }
    }
}