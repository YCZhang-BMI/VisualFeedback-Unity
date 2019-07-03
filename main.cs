using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using System.IO;



public class main : MonoBehaviour
{

    public int localPort = 2001;
    static UdpClient udp;
    Thread thread;
    public int sessionNum = 1;
    static string text = "R011000100001040451";
    public int trialCount = 1;
    public int numERDL = 0;
    public int numERDR = 0;
    public int ERDL = 0;
    public int ERDR = 0;
    public int pmERDL = 1;
    public int pmERDR = 1;
	public int threshold = 40;
    public int armStatus = 0;
    public int trialAssign = 1;
    public int score = 0;
    public float percentage = 0;


    public GameObject feedback;
    public GameObject canvas;
    public GameObject barL;
    public GameObject barR;
	public GameObject thBarL;
	public GameObject thBarR;
    public GameObject coin;
    public GameObject thumbProximal;
    public GameObject indexProximal;
    public GameObject indexInter;
    public GameObject indexDistal;
    public GameObject midProximal;
    public GameObject midInter;
    public GameObject midDistal;
    public GameObject ringProximal;
    public GameObject ringInter;
    public GameObject ringDistal;
    public GameObject litProximal;
    public GameObject litInter;
    public GameObject litDistal;

    public Vector3 TPori = new Vector3(-5.809f, 8.874001f, -56.386f);
    public Vector3 TPtgt = new Vector3(-5.809f, 8.874001f, -39.554f);
    public Vector3 IPori = new Vector3(-5.459f, 0.158f, -5.812f);
    public Vector3 IPtgt = new Vector3(-62.474f, 10.813f, -12.598f);
    public Vector3 IIori = new Vector3(-6.415f, -16.793f, 4.922f);
    public Vector3 IItgt = new Vector3(-51.226f, -22.357f, 7.825f);
    public Vector3 IDori = new Vector3(-11.095f, 5.699f, 1.107f);
    public Vector3 IDtgt = new Vector3(-27.845f, 12.784f, 19.204f);
    public Vector3 MPori = new Vector3(-4.965f, 0.166f, -1.852f);
    public Vector3 MPtgt = new Vector3(-64.537f, 3.885f, -4.296f);
    public Vector3 MIori = new Vector3(-12.778f, -0.119f, -0.367f);
    public Vector3 MItgt = new Vector3(-51.788f, -0.254f, -0.578f);
    public Vector3 MDori = new Vector3(-6.071f, 0.136f, -1.151f);
    public Vector3 MDtgt = new Vector3(-24.503f, 8.105f, -4.422f);

    public Vector3 RPori = new Vector3(-11.455f, 0.515f, -4.129f);
    public Vector3 RPtgt = new Vector3(-67.166f, 9.367f, -10.479f);
    public Vector3 RIori = new Vector3(-5.563f, 0.33f, -0.097f);
    public Vector3 RItgt = new Vector3(-44.724f, 0.417f, -0.136f);
    public Vector3 RDori = new Vector3(-7.276f, 0.5470001f, -0.47f);
    public Vector3 RDtgt = new Vector3(-20.641f, -39.755f, 21.262f);

    public Vector3 LPori = new Vector3(-7.595f, -0.429f, 3.208f);
    public Vector3 LPtgt = new Vector3(-54.825f, -4.525f, 5.525f);
    public Vector3 LIori = new Vector3(-11.319f, -0.858f, -3.138f);
    public Vector3 LItgt = new Vector3(-60.657f, -5.729f, 6.289f);
    public Vector3 LDori = new Vector3(-10.756f, -0.087f, 0.414f);
    public Vector3 LDtgt = new Vector3(-29.594f, -0.563f, 1.322f);


    public Text taskText;
    public Text scoreText;


    string folderPath = @"D:/";

    private void Start()
    {
        //string hostname = Dns.GetHostName();
        //IPAddress[] adrList = Dns.GetHostAddresses(hostname);
        //foreach (IPAddress adress in adrList)
        //{
        //    string iphead = adress.ToString().Substring(0, 2);
        //    string ipend = adress.ToString().Substring(adress.ToString().Length - 2, 2);
        //    int ipend2 = 0;
        //    int iphead2 = 0;
        //    bool result = int.TryParse(ipend, out ipend2);
        //    bool result2 = int.TryParse(iphead, out iphead2);

        //}

        FileInfo fi = new FileInfo(folderPath + "tasksession" + sessionNum.ToString() + ".tsv");
        StreamWriter sw = fi.AppendText();

        udp = new UdpClient(localPort);
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();

        Debug.Log("started");
    }
    public void FixedUpdate()
    {        
        string status = text.Substring(0, 1);
        Int32.TryParse(text.Substring(1, 2), out trialCount);
        if (text.Substring(3, 1) == "1")
        {
            pmERDL = 0;
        }else
        {
            pmERDL = 1;
        }        
        Int32.TryParse(text.Substring(4, 3), out numERDL);
        if (text.Substring(7, 1) == "1")
        {
            pmERDR = 0;
        }
        else
        {
            pmERDR = 1;
        }
        Int32.TryParse(text.Substring(8, 3), out numERDR);
        Int32.TryParse(text.Substring(11, 2), out sessionNum);
		Int32.TryParse (text.Substring (13, 3), out score);
        Int32.TryParse(text.Substring(16, 2), out armStatus);
        Int32.TryParse(text.Substring(18, 1), out trialAssign);
        ERDL = numERDL * pmERDL;
        ERDR = numERDR * pmERDR;
        percentage = (float)armStatus;
        switch (status)
        {
            case "R":
                canvas.SetActive(true);
                //feedback.SetActive(false);
                taskText.text = "Ready";
                break;
            case "r":
                canvas.SetActive(true);
                //feedback.SetActive(false);
                taskText.text = "Rest " + trialCount.ToString();
                break;
            case "t":
                canvas.SetActive(false);
                feedback.SetActive(true);
                scoreText.text = "Score : " + score.ToString();

                //arm.transform.eulerAngles = new Vector3(-88.913F, 45+180-armStatus, 0);
                //coin.GetComponent<RectTransform>().anchoredPosition = new Vector3(-289 + 578 * trialAssign, 100, 0);                
                break;
            case "b":
                taskText.text = "Break";
                canvas.SetActive(true);
                //feedback.SetActive(false);


                thumbProximal.transform.localEulerAngles = TPtgt;
                indexProximal.transform.localEulerAngles = IPtgt;
                indexInter.transform.localEulerAngles = IItgt;
                indexDistal.transform.localEulerAngles = IDtgt;
                midProximal.transform.localEulerAngles = MPtgt;
                midInter.transform.localEulerAngles = MItgt;
                midDistal.transform.localEulerAngles = MDtgt;

                ringProximal.transform.localEulerAngles = RPtgt;
                ringInter.transform.localEulerAngles = RItgt;
                ringDistal.transform.localEulerAngles = RDtgt;

                litProximal.transform.localEulerAngles = LPtgt;
                litInter.transform.localEulerAngles = LItgt;
                litDistal.transform.localEulerAngles = LDtgt;
                break;
            case "c":
                canvas.SetActive(true);
                //feedback.SetActive(false);
                switch (trialAssign)
                {
                    case 0:
                        taskText.text = "Rest";
                        break;
                    case 1:
                        taskText.text = "Left";
                        break;
                    case 2:
                        taskText.text = "Right";
                        break;
                }
                break;

        }
        barL.transform.eulerAngles = new Vector3(180f * pmERDL, 0, 0);
        barL.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, numERDL);
        barR.transform.eulerAngles = new Vector3(180f * pmERDR, 0, 0);
        barR.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, numERDR);
        thBarL.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, threshold, 0);
        thBarR.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, threshold, 0);
        thumbProximal.transform.localEulerAngles = TPtgt + (TPori - TPtgt) * (percentage / 90);
        indexProximal.transform.localEulerAngles = IPtgt - (IPtgt - IPori) * (percentage / 90);
        indexInter.transform.localEulerAngles = IItgt - (IItgt - IIori) * (percentage / 90);
        indexDistal.transform.localEulerAngles = IDtgt - (IDtgt - IDori) * (percentage / 90);
        midProximal.transform.localEulerAngles = MPtgt - (MPtgt - MPori) * (percentage / 90);
        midInter.transform.localEulerAngles = MItgt - (MItgt - MIori) * (percentage / 90);
        midDistal.transform.localEulerAngles = MDtgt - (MDtgt - MDori) * (percentage / 90);

        ringProximal.transform.localEulerAngles = RPtgt - (RPtgt - RPori) * (percentage / 90);
        ringInter.transform.localEulerAngles = RItgt - (RItgt - RIori) * (percentage / 90);
        ringDistal.transform.localEulerAngles = RDtgt - (RDtgt - RDori) * (percentage / 90);

        litProximal.transform.localEulerAngles = LPtgt - (LPtgt - LPori) * (percentage / 90);
        litInter.transform.localEulerAngles = LItgt - (LItgt - LIori) * (percentage / 90);
        litDistal.transform.localEulerAngles = LDtgt - (LDtgt - LDori) * (percentage / 90);


        Debug.Log(45F - armStatus);



    }
    private void OnApplicationQuit()
    {
        thread.Abort();
        udp.Close();
    }

    private static void ThreadMethod()
    {

        Debug.Log("thread");
        while (true)
        {
            Debug.Log("2");
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            Debug.Log("1");
            try
            {
                Debug.Log("5");
                byte[] data = udp.Receive(ref remoteEP);
                Debug.Log("4");
                text = Encoding.ASCII.GetString(data);
                Debug.Log(text);
                //float value = float.Parse(text);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            Debug.Log("3");
        }
    }
}