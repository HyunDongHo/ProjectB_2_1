using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RNG
{
    public class RNGManager : Singleton<RNGManager>
    {
        private RandomSetting GetRandomSingle(RandomSetting randomSetting)
        {
            float convertPercentage = Mathf.Clamp(randomSetting.percentage, 0f, 100f);

            if (UnityEngine.Random.Range(0f, 100f) <= convertPercentage)
            {
                RandomSetting successRandomSetting = randomSetting;
                return successRandomSetting;
            }
            else
            {
                RandomSetting failRandomSetting = new RandomSetting(100 - randomSetting.percentage);
                return failRandomSetting;
            }
        }

        private RandomSetting GetRandomMultiple(RandomSetting[] randomSettings)
        {
            List<RandomSetting> randoms = new List<RandomSetting>();
            foreach (RandomSetting item in randomSettings)
                randoms.Add(item);

            float totalProbaility = 0;
            foreach (var random in randoms)
                totalProbaility += random.percentage;

            if ((decimal)totalProbaility > 100)
            {
                Debug.LogError($"[RNGManager] 확률이 (현재 확률: {(decimal)totalProbaility}) 100이 아닙니다. 강제로 총 확률을 100으로 조정합니다.");

                float exceedProbaility = totalProbaility - 100;

                // 1차 조정은 소수가 포함되지 않는 값으로 계산하여 조정.
                int divideMinusValue = Mathf.FloorToInt(exceedProbaility / randoms.Count);
                foreach (var random in randoms)
                    random.percentage -= divideMinusValue;

                // 2차 조정은 소수점을 처리를 위해 계산하여 조정. 
                float remainDivideMinusValue = exceedProbaility - divideMinusValue * randoms.Count;
                randoms[UnityEngine.Random.Range(0, randoms.Count)].percentage -= remainDivideMinusValue;
            }
            else if((decimal)totalProbaility < 100)
            {
                float remainProbaility = 100 - totalProbaility;

                if(remainProbaility > 0.0001)
                { 
                    Debug.LogError($"[RNGManager] 확률이 (현재 확률: {(decimal)totalProbaility}) 100이 아닙니다. 남은 확률을 추가합니다.");

                    if (remainProbaility > 0)
                    {
                        RandomSetting remainRandomSetting = new RandomSetting(remainProbaility);
                        randoms.Add(remainRandomSetting);
                    }
                }
            }

            // 가중치 계산.
            randoms.Sort((a, b) =>
            {
                if (a.percentage > b.percentage)
                {
                    return 1;
                }
                else if (a.percentage < b.percentage)
                {
                    return -1;
                }

                return 0;
            });

            float randomValue = UnityEngine.Random.Range(0f, 100f);
            for (int i = 0; i < randoms.Count; i++)
            {
                if (randomValue < randoms[i].percentage)
                {
                    return randoms[i];
                }
                else
                {
                    randomValue -= randoms[i].percentage;
                }
            }

            // RandomValue가 만약 100이 나오게 되면 임의의 점을 찾을 수 없기 때문에 처리.
            return randoms[randoms.Count - 1];
        }

        public RandomSetting GetRandom(params RandomSetting[] randomSettings)
        {
            // 다중일 때는 좀 더 복잡한 가중치 랜덤 함수를 호출하여 Return
            if(randomSettings.Length > 1)
            {
                return GetRandomMultiple(randomSettings);
            }
            // 단일일 때는 간단하게 랜덤 함수를 호출하여 Return
            else
            {
                int firstRow = 0;

                return GetRandomSingle(randomSettings[firstRow]);
            }
        }
    }

    public class RandomSetting
    {
        public float percentage { get; set; }
        public string name { get; private set; }

        public RandomSetting(float percentage, string name = "")
        {
            this.percentage = percentage;
            this.name = name;
        }
    }
}
