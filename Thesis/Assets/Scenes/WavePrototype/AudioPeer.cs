using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    
    public static float[] _samples = new float[512]; //spectrum audio source
    public static float[] _samples2 = new float[10000]; //output data
    public static float[] _freqBand = new float[8]; //frequency bands
    public static float[] _samplesBuffer = new float[8]; // buffer for frequency bands but I used this for sample buffers instead
    public static float[] _bandBuffer = new float[8]; //buffer for frequency band
    float[] _sampleDecrease = new float[8]; //decrease array for buffer

    //Usable Range
    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];
    //Amplitdue
    public static float _Amplitude, _AmplitudeBuffer;

    float _AmplitudeHighest;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       GetSpectrumAudioSource();
       MakeFrequencyBands();
       SampleBuffer();
       CreateAudioBands();
       GetAmplitude();
       
    }
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if(_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }
    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples,0,FFTWindow.Blackman);
        //_audioSource.GetOutputData(_samples, 0);
        //_audioSource.GetOutputData(_samples2, 0);
    }
    void SampleBuffer()
    {
        for(int g = 0; g < _freqBand.Length; ++g)
        {
            if(_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _sampleDecrease[g] = 0.005f;

            }
            if(_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _sampleDecrease[g];
                _sampleDecrease[g] *= 3.5f;
            }
        }
    }
    void MakeFrequencyBands()
    {
        int count = 0;
        float average = 0; 
        for(int i = 0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2; 
            if(i == 7)
            {
                sampleCount += 2; 
            }
            for(int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10; 



        }
    }
    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;
        for(int i = 0; i < 8; i++)
        {
            
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitude += _audioBandBuffer[i];

           
        }
        if(_CurrentAmplitude > _AmplitudeHighest)
        {
            _AmplitudeHighest = _CurrentAmplitude;
        }

        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
        //Debug.Log("Current AmplitudeBuffer" + _AmplitudeBuffer);
    }
    
    /*22050 Hertz for the song /512 hertz = 43 herts per sample
     * 20 -60 hertz
     * 250 - 500 hertz
     * 500-2000
     * 2000-4000hertz
     * 4000-6000
     * 6000-20000hertz
     * 0-2 = 86 hertz
     * 1- 4 = 172 hertz - 87-258 hertz
     * 2 - 8 = 344 Hertz - 259 - 602 hertz
     * 
     * 
     */
}
