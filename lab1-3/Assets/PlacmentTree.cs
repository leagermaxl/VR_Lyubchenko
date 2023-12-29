using System.Collections;
using UnityEngine;

public class PlacmentTree : MonoBehaviour
{
    [Header("Param generation placment tree")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    [SerializeField] private float randomlyPlace;
    [SerializeField] private int randomCountGenerateTree;

    [SerializeField] private MeshGenerator _terrain;
    [SerializeField] private Transform _parrent;
    [SerializeField] private Texture2D _noiseMapTexture;
    [SerializeField] private float _density = 0.5f;

    [SerializeField] private TreeGenerator _treeGenerator;
    private GameObject[] _trees;
    private void Start()
    {
        int width = _terrain.xSize;
        int height = _terrain.zSize;
        float scale = 5;
        _noiseMapTexture = GetNoiseMap(width, height, scale);
        _trees = new GameObject[randomCountGenerateTree];
        for (int i = 0; i < randomCountGenerateTree; i++)
        {
            _trees[i] = _treeGenerator.GenerateTree();
        }
        StartCoroutine(PlaceObjects());
    }
    private IEnumerator PlaceObjects()
    {
        for (int x = 0; x < _terrain.xSize; x++)
        {
            for (int z = 0; z < _terrain.zSize; z++)
            {
                float noiseMapValue = _noiseMapTexture.GetPixel(x, z).g;

                if (noiseMapValue > 1 - _density)
                {
                    Vector3 pos = new Vector3(x + Random.Range(-randomlyPlace, randomlyPlace), 0, z + Random.Range(-randomlyPlace, randomlyPlace));
                    pos.y = MeshGenerator.GetHeight(new Vector3(pos.x, _terrain.ySize, pos.z));
                    Vector3 localPosition = new Vector3(pos.x + _parrent.position.x, pos.y, pos.z + _parrent.position.z);
                    if (Fitness(localPosition))
                    {
                        break;
                    }
                    SpawnTree(localPosition);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
    public Texture2D GetNoiseMap(int width, int height, float scale)
    {
        Texture2D noiseMapTexture = new Texture2D(width, height);

        for (int x = 0; x < _terrain.xSize; x++)
        {
            for (int z = 0; z < _terrain.zSize; z++)
            {
                float noiseValue = Mathf.PerlinNoise((float)x / width * scale, (float)z / height * scale);

                noiseMapTexture.SetPixel(x, z, new Color(0, noiseValue, 0));
            }
        }

        noiseMapTexture.Apply();

        return noiseMapTexture;
    }
    private bool Fitness(Vector3 position)
    {
        if (position.y > maxHeight || position.y < minHeight)
        {
            return true;
        }
        if (position.x > _terrain.xSize + _parrent.position.x || position.x < -(_terrain.xSize + _parrent.position.x))
        {
            return true;
        }
        if (position.z > _terrain.zSize + _parrent.position.z || position.z < -(_terrain.zSize + _parrent.position.z))
        {
            return true;
        }
        return false;
    }
    private void SpawnTree(Vector3 spawnPoint)
    {
        GameObject spawn = _trees[Random.Range(0, randomCountGenerateTree)];
        Quaternion randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        Instantiate(spawn, spawnPoint, randomRotation, _parrent);
    }
}
