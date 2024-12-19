using System.Collections.Generic;
using System;
using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        public double savedCookieCount = 0; // Поле для сохранения количества печенек
        public double savedCookiesPerSecond = 0; // Сохранённое количество печенек в секунду

        // Сохранение данных улучшений
        public double[] savedUpgradeCosts; // Стоимости улучшений
        public int[] savedUpgradeLevels;   // Уровни улучшений
    }
}

