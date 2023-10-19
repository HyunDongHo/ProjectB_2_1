// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using Unity.VisualScripting;

namespace BackendData.Chart.LevelPass
{
    //===============================================================
    // Quest ��Ʈ�� �����͸� �����ϴ� Ŭ����
    //===============================================================
    public class Manager : Base.Chart
    {

        // �� ��Ʈ�� Item(row) ������ ��� Dictionary
        readonly Dictionary<double, Item> _dictionary = new();

        // �ٸ� Ŭ�������� Add, Delete�� ������ �Ұ����ϵ��� �б� ���� Dictionary
        public IReadOnlyDictionary<double, Item> Dictionary => (IReadOnlyDictionary<double, Item>)_dictionary.AsReadOnlyCollection();


        // ��Ʈ ���� �̸� ���� �Լ�
        // ��Ʈ �ҷ����⸦ ���������� ó���ϴ� BackendChartDataLoad() �Լ����� �ش� �Լ��� ���� ��Ʈ ���� �̸��� ��´�.
        public override string GetChartFileName()
        {
            return "LevelPass";
        }

        // Backend.Chart.GetChartContents���� �� ��Ʈ ���¿� �°� �Ľ��ϴ� Ŭ����
        // ��Ʈ ���� �ҷ����� �Լ��� BackendData.Base.Chart�� BackendChartDataLoad�� �������ּ���
        protected override void LoadChartDataTemplate(JsonData json)
        {
            foreach (JsonData eachItem in json)
            {
                Item info = new Item(eachItem);
                _dictionary.Add(info.Level, info);
            }
        }
        public Dictionary<double, Item> GetChartLevelPassData()
        {
            Dictionary<double, Item> LevelPassData = new();

            foreach (var data in _dictionary)
            {
                LevelPassData.Add(data.Value.Level, data.Value);
            }
            return LevelPassData;
        }
    }
}