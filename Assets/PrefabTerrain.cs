using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

public class PrefabTerrain : MonoBehaviour
{
    public GameObject gobj;
    public GameObject parnt;
    public int delay;
    public int radius;

    // Start is called before the first frame update
    void Start()
    {
        //placeObjSpiralConc(gobj, parnt.transform.position, radius);
        genCaves(gobj, parnt.transform.position, new Vector3(50, 50, 50));
    }

    async void placeObjSpiral(GameObject tileobj, Vector3 centerpos, int radius = 1)
    {
        await Task.Delay(1000);
        for (int slen = 1; slen <= 2 * radius + 1; slen += 2)
        {
            if (!EditorApplication.isPlaying) return;

            if (slen == 1)
            {
                var tmp = Instantiate(tileobj, centerpos, Quaternion.identity);
                tmp.transform.parent = parnt.transform;
                await Task.Delay(delay);
            }
            else
            {
                for (int j = 1; j < slen; j++)
                {
                    var tmp = Instantiate(tileobj, new Vector3(centerpos.x - ((slen - 1) / 2) + j, centerpos.y, centerpos.z - ((slen - 1) / 2)), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    await Task.Delay(delay);
                }
                for (int j = 1; j < slen; j++)
                {
                    var tmp = Instantiate(tileobj, new Vector3(centerpos.x + ((slen - 1) / 2), centerpos.y, centerpos.z - ((slen - 1) / 2) + j), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    await Task.Delay(delay);
                }
                for (int j = 1; j < slen; j++)
                {
                    var tmp = Instantiate(tileobj, new Vector3(centerpos.x + ((slen - 1) / 2) - j, centerpos.y, centerpos.z + ((slen - 1) / 2)), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    await Task.Delay(delay);
                }
                for (int j = 1; j < slen; j++)
                {
                    var tmp = Instantiate(tileobj, new Vector3(centerpos.x - ((slen - 1) / 2), centerpos.y, centerpos.z + ((slen - 1) / 2) - j), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    await Task.Delay(delay);
                }
            }
        }
    }

    async void placeObjSpiralConc(GameObject tileobj, Vector3 centerpos, int radius = 1)
    {
        await Task.Delay(1000);

        float rx = Random.Range(-1000f, 1000f);
        float rz = Random.Range(-1000f, 1000f);

        for (int slen = 1; slen <= 2 * radius + 1; slen += 2)
        {
            if (!EditorApplication.isPlaying) return;

            int offset = (slen - 1) / 2;
            if (slen == 1)
            {
                var tmp = Instantiate(tileobj, centerpos, Quaternion.identity);
                tmp.transform.parent = parnt.transform;
                tmp.transform.position += new Vector3(0, Mathf.PerlinNoise(tmp.transform.position.x / 20 + rx, tmp.transform.position.z / 20 + rz) * 12, 0);
            }
            else for (int j = 1; j < slen; j++)
                {
                    var tmp = Instantiate(tileobj, new Vector3(centerpos.x - offset + j, centerpos.y, centerpos.z - offset), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    tmp.transform.position += new Vector3(0, Mathf.PerlinNoise(tmp.transform.position.x / 20 + rx, tmp.transform.position.z / 20 + rz) * 12, 0);

                    tmp = Instantiate(tileobj, new Vector3(centerpos.x + offset, centerpos.y, centerpos.z - offset + j), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    tmp.transform.position += new Vector3(0, Mathf.PerlinNoise(tmp.transform.position.x / 20 + rx, tmp.transform.position.z / 20 + rz) * 12, 0);

                    tmp = Instantiate(tileobj, new Vector3(centerpos.x + offset - j, centerpos.y, centerpos.z + offset), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    tmp.transform.position += new Vector3(0, Mathf.PerlinNoise(tmp.transform.position.x / 20 + rx, tmp.transform.position.z / 20 + rz) * 12, 0);

                    tmp = Instantiate(tileobj, new Vector3(centerpos.x - offset, centerpos.y, centerpos.z + offset - j), Quaternion.identity);
                    tmp.transform.parent = parnt.transform;
                    tmp.transform.position += new Vector3(0, Mathf.PerlinNoise(tmp.transform.position.x / 20 + rx, tmp.transform.position.z / 20 + rz) * 12, 0);
                    await Task.Delay(delay);
                }
        }
    }

    async void genCaves(GameObject gobj, Vector3 centerpos, Vector3 size)
    {

        float rx = Random.Range(-1000f, 1000f);
        float ry = Random.Range(-1000f, 1000f);
        float rz = Random.Range(-1000f, 1000f);

        bool waitflag = false;

        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.z; j++)
            {
                for (int k = 0; k < size.x; k++)
                {
                    if (!EditorApplication.isPlaying) return;
                    if (Perlin3D((k + centerpos.x) * .05f + rx, (i + centerpos.y) * .05f + ry, (j + centerpos.z) * .05f + rz) >= 0.5f)
                    {
                        var tmp = Instantiate(gobj, new Vector3(k + centerpos.x, i + centerpos.y, j + centerpos.z), Quaternion.identity);
                        tmp.transform.parent = parnt.transform;
                        waitflag = true;
                    }
                }
            }
            if (waitflag)
            {
                await Task.Delay(delay);
                waitflag = false;
            }
        }
    }

    static float Perlin3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);

        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6;
    }
}
