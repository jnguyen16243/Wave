using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveForm : MonoBehaviour
{
    LineRenderer wave;
    private float counter;
    private float dist;
    private float z;
    private float zChange = (360f * (Mathf.PI * 2)) / 512f;
   

    //public Transform origin;
    //public Transform destination;
    //public Material waveMaterial;
    public float  _widthMultiplier;
    public int _maxScale;
    public int yPosition;
    public bool _useBuffer;
    public float _radius;
    public float _thetaChange;
    public int lineSize;
    float theta = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        wave = GetComponent<LineRenderer>();
        wave.widthMultiplier = _widthMultiplier;
        wave.positionCount = lineSize; //length of samples in AudioPeer
       // wave.material = waveMaterial;

        
    }
    
    // Update is called once per frame
    void Update()
    {
        float pieces = (2f * Mathf.PI) / wave.positionCount;
        float r = AudioPeer._Amplitude;
        if(r < 0)
        {
            r = 100;
        }
        Debug.Log("Amplitude: " + r);
        if (_useBuffer){
            //float random = Random.Range(0.5f, 10.0f);
            
            for (int i = 0; i < wave.positionCount; i++)
            {
                theta += _thetaChange;
                float angle = i * pieces; // 512 divided by angle
                //float x = _radius * Mathf.Cos(angle);
                //float z = _radius * Mathf.Sin(angle);
                
                float x = _radius * r * Mathf.Cos(angle);
                float z =  _radius * r * Mathf.Sin(angle);

                //Vector3 position = new Vector3(x, (AudioPeer._samples[i] * _maxScale) - yPosition, z);
                Vector3 position = new Vector3(x, (Mathf.Cos(theta) *_maxScale) + yPosition, z );

                wave.SetPosition(i, position);
            }
        }
        if (!_useBuffer)
        {
            
            float max = 0; 
            for (int i = 0; i < wave.positionCount; i++)
            {
                if(max < AudioPeer._samples[i])
                {
                    max = AudioPeer._samples[i];
                }
                float angle =  i * pieces; // 512 divided by angle
                float x = _radius *Mathf.Cos(angle);
                float z = _radius* Mathf.Sin(angle);
                Vector3 position = new Vector3(x, (AudioPeer._samples[i] * _maxScale) - yPosition, z);
                
                
                wave.SetPosition(i, position);
            }
            //Debug.Log(max);
        }
        
    }
}
