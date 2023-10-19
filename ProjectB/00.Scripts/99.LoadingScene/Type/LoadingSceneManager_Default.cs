using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManager_Default : LoadingSceneManagers
{
    protected override IEnumerator DelayWhileLoading(AsyncOperation asyncOperation)
    {
        yield return StartCoroutine(base.DelayWhileLoading(asyncOperation));
    }
}
