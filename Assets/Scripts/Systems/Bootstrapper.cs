using UnityEngine;

// From: https://gist.github.com/Matthew-J-Spencer/08d046f6a6a7bc1f0dd0b71bc4607ddc
public static class Bootstrapper {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
    }
}