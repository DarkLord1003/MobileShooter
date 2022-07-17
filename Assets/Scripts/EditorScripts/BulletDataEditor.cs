#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(BulletData))]
public class BulletDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BulletData bulletData = (BulletData)target;

        bulletData.Speed = EditorGUILayout.FloatField("Speed", bulletData.Speed);
        bulletData.DestroyOnImpact = EditorGUILayout.Toggle("Destroy On Impact", bulletData.DestroyOnImpact);

        if (bulletData.DestroyOnImpact)
        {
            bulletData.MinDestroyTime = EditorGUILayout.FloatField("Minimum Destroy Time", bulletData.MinDestroyTime);
            bulletData.MaxDestroyTime = EditorGUILayout.FloatField("Maximum Destroy Time", bulletData.MaxDestroyTime);
        }
    }
}

#endif
