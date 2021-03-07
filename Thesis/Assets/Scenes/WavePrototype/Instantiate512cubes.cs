using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512cubes : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[512];
    public float _maxScale;
    public static float[] _freqBand = new float[8];
    public bool _useBuffer;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 512; i++)
        {
            Debug.Log("i");
            GameObject _instanceSampleCube = (GameObject)Instantiate(_sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _instanceSampleCube.transform.position = Vector3.forward * 100;
            _sampleCube[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_useBuffer)
        {
            for (int i = 0; i < 512; i++)
            {
                if (_sampleCube != null)
                {
                    _sampleCube[i].transform.localScale = new Vector3(10, (AudioPeer._samplesBuffer[i] * _maxScale) + 2, 10);
                }
            }
        }
        if (!_useBuffer)
        {
            for (int i = 0; i < 512; i++)
            {
                if (_sampleCube != null)
                {
                    _sampleCube[i].transform.localScale = new Vector3(10, (AudioPeer._samples[i] * _maxScale) + 2, 10);
                }
            }
        }
        
        MakeFrequencyBands();
    }
    void MakeFrequencyBands()
    {
        /*
         * 22050 / 512 = 43Hertz per sample
         * 
         * 
         */
    }
}
