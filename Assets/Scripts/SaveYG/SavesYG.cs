using System.Collections.Generic;
using System;
using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        public double savedCookieCount = 0; // ���� ��� ���������� ���������� �������
        public double savedCookiesPerSecond = 0; // ���������� ���������� ������� � �������

        // ���������� ������ ���������
        public double[] savedUpgradeCosts; // ��������� ���������
        public int[] savedUpgradeLevels;   // ������ ���������
    }
}

