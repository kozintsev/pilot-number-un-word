using System;

namespace NumberInWords
{
    /// <summary>
    /// Преобразование денег в сумму прописью
    /// </summary>
    public abstract class MoneyToStringProviderBase : IMoneyToStringProvider
    {
        protected abstract NumberToRussianString.WordGender GetGender(bool high);
        // Функция возвращает наименование денежной единицы в соответствующей форме 
        //  (1) рубль / (2) рубля / (5) рублей
        protected abstract string GetName(NumberToRussianString.WordMode wordMode, bool high);
        // Функция возвращает сокращенное наименование денежной единицы 
        protected abstract string GetShortName(bool high);
        // сокращенное написание рублей ? - рублей/руб.
        private readonly bool _shortHigh;
        // сокращенное написание копеек ? - копеек/коп.
        private readonly bool _shortLow;
        // отображение копеек в виде цифр ? - 00
        private readonly bool _digitLow;
        // Конструктор
        protected MoneyToStringProviderBase(bool shortHigh, bool shortLow, bool digitLow)
        {
            _shortHigh = shortHigh;
            _shortLow = shortLow;
            _digitLow = digitLow;
        }
        // Реализация интерфейса IMoneyToStringProvider
        // Метод родительского интерфейса IFormatProvider
        public object GetFormat(Type formatType)
        {
            return formatType != typeof(RoubleToStringProvider) ? null : this;
        }
        // Функция возвращает число рублей и копеек прописью
        public string MoneyToString(Money m)
        {
            var r = m.High;
            long c = m.Low;
            return string.Format("{0} {1} {2} {3}",
                NumberToRussianString.NumberToString(r, GetGender(true)),
                _shortHigh ?
                    GetShortName(true) :
                    GetName(NumberToRussianString.GetWordMode(r), true),
                _digitLow ?
                    string.Format("{0:d2}", c) :
                    NumberToRussianString.NumberToString(c, GetGender(false)),
                _shortLow ?
                    GetShortName(false) :
                    GetName(NumberToRussianString.GetWordMode(c), false));
        }
    }
}