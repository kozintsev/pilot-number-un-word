using System;

namespace NumberInWords
{
    /// <summary>
    /// Интерфейс специализированного провайдера преобразования денег в строковое представление
    /// </summary>
    public interface IMoneyToStringProvider : IFormatProvider
    {
        string MoneyToString(Money m);
    }
}