namespace NumberInWords
{
    /// <summary>
    /// Преобразование украинских денег (грн + копейки) в сумму прописью
    /// </summary>
    public class HryvnaToStringProvider : MoneyToStringProviderBase
    {
        public HryvnaToStringProvider(bool shortHrn, bool shortCopecks, bool digitCopecks) :
            base(shortHrn, shortCopecks, digitCopecks)
        { }
        // варианты написания рублей
        private static readonly string[] Roubles = { "гривня", "гривні", "гривень" };
        // варианты написания копеек
        private static readonly string[] Copecks = { "копійка", "копійки", "копійок" };

        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return NumberToRussianString.WordGender.Feminine;
        }
        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? Roubles[(int)wordMode] : Copecks[(int)wordMode];
        }
        protected override string GetShortName(bool high)
        {
            return high ? "грн." : "коп.";
        }
    }
}