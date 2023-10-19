// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using Unity.VisualScripting;

namespace BackendData.Chart.PassInfo
{
    //===============================================================
    // Quest ��Ʈ�� �����͸� �����ϴ� Ŭ����
    //===============================================================
    public class Manager : Base.Chart
    {

        // �� ��Ʈ�� Item(row) ������ ��� Dictionary
        readonly Dictionary<int, Item> _dictionary = new();

        // �ٸ� Ŭ�������� Add, Delete�� ������ �Ұ����ϵ��� �б� ���� Dictionary
        public IReadOnlyDictionary<int, Item> Dictionary => (IReadOnlyDictionary<int, Item>)_dictionary.AsReadOnlyCollection();


        // ��Ʈ ���� �̸� ���� �Լ�
        // ��Ʈ �ҷ����⸦ ���������� ó���ϴ� BackendChartDataLoad() �Լ����� �ش� �Լ��� ���� ��Ʈ ���� �̸��� ��´�.
        public override string GetChartFileName()
        {
            return "PassInfo";
        }

        // Backend.Chart.GetChartContents���� �� ��Ʈ ���¿� �°� �Ľ��ϴ� Ŭ����
        // ��Ʈ ���� �ҷ����� �Լ��� BackendData.Base.Chart�� BackendChartDataLoad�� �������ּ���
        protected override void LoadChartDataTemplate(JsonData json)
        {
            foreach (JsonData eachItem in json)
            {
                Item info = new Item(eachItem);
                _dictionary.Add(info.PassID, info);  
            }
        }
        public Dictionary<int, Item> GetChartPassInfoData()
        {
            Dictionary<int, Item> PassInfoData = new();

            foreach (var data in _dictionary)
            {
                PassInfoData.Add(data.Value.PassID, data.Value);
            }
            return PassInfoData;
        }
        public PremiumType GetPremiumType(PassType passType)
        {
            PremiumType premiumType = PremiumType.None;
            foreach (var data in _dictionary)
            {
                if (data.Value.PassType == passType)
                    premiumType = data.Value.PremiumType;
            }
            return premiumType;
        }
        public int GetPassID(PassType passType)
        {
            int id = 0;
            foreach (var data in _dictionary)
            {
                if (data.Value.PassType == passType)
                    id = data.Value.PassID;
            }
            return id;
        }
        public PassType GetFirstDataPassType()
        {
            return _dictionary[1].PassType;
        }
    }
}