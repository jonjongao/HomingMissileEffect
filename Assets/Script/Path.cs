using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour
{
    public Transform target;
    public Vector3[] paths;
    public float varianceValue;
    public Vector2 pathIntensity;
    public float time = 2f;
    public GameObject[] objs;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<GameObject> objList = new List<GameObject>();
            
            for (int i = 0; i < 10; i++)
            {
                paths = CreatePath(target);
                paths[paths.Length - 1] = paths[paths.Length - 2];
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.position = transform.position;
                objList.Add(obj);
                //iTween.MoveTo(obj, iTween.Hash("position", target.position, "time", 1.5f, "path", paths, "easetype", iTween.EaseType.easeInQuad));
                iTween.MoveTo(obj, iTween.Hash("position", target.position, "speed", 30f, "path", paths, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "DirectHit", "oncompletetarget", gameObject));
            }
            objs = objList.ToArray();
        }
        if(paths.Length!=0)
        {
            //paths[paths.Length-1] = target.position;
            //DirectHit();
        }
    }

    public void DirectHit()
    {
        Debug.Log("dirHit");
        foreach (GameObject o in objs)
            iTween.MoveTo(o, iTween.Hash("position", target.position, "speed", 50f));
    }

    public Vector3[] CreatePath(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Vector3 normal = direction.normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, normal);
        float variance = direction.magnitude * varianceValue;
        Vector3 point = Vector3.zero;
        float amount = 0.0f;
        List<Vector3> path = new List<Vector3>();
        int time = 0;

        while (amount < 1.0)
        {
            cross = Vector3.Cross(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), normal);
            amount = Mathf.Clamp01(amount + Random.Range(pathIntensity.x, pathIntensity.y));
            point = Vector3.Lerp(transform.position, target.position, amount);
            point += cross * Random.Range(-variance, variance);
            if (amount == 1.0)
                point = target.position;
            if (time == 0)
                path.Add(transform.position);
            path.Add(point);
            if (variance - (varianceValue * 10f) > 0)
                variance -= (varianceValue * 10f);
            else
                variance = 0;
            //Debug.Log(variance);
            time++;
        }

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pre;
            Vector3 cur;
            if (i == 0)
            {
                pre = path[i];
                cur = path[i];
            }
            else
            {
                pre = path[i - 1];
                cur = path[i];
            }
            Debug.DrawLine(pre, cur, Color.Lerp(Color.green, Color.red, i * 0.1f), 5f);
        }

        return path.ToArray();
    }
}
