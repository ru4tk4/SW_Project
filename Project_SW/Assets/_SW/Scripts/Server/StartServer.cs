using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class StartServer : MonoBehaviour {
    testClient tc = new testClient();
    public List<string> jSONOs;
    public string ip;
    // Use this for initialization
    void Start() {
        int i = 0;
        foreach (MySelectable selectable in MySelectable.allMySelectables)
        {
            selectable.ID = i;
            i += 1;
        }

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("st");
            tc.ConnectServer(ip,this);
        }
        if (jSONOs.Count > 0)
        {
            
            foreach(string s in jSONOs)
            {
                JSONObject JSONData = new JSONObject(s);
                Debug.Log(JSONData.ToString());
                switch (JSONData.GetField("Type").str)
                {
                    case "Move":
                        Debug.Log(JSONData.GetField("Type").str);
                        foreach (MySelectable selectable in MySelectable.allMySelectables)
                        {
                            
                            Debug.Log(JSONData.GetField("ObjectID").num.ToString());
                            
                            if (JSONData.GetField("ObjectID").num == selectable.ID)
                            {
                                Debug.Log(JSONData.GetField("ObjectID").str);
                                selectable.UnitMove
                                    (
                                    extension.StringToVector3(JSONData.GetField("Point").str),
                                    extension.StringToVector3(JSONData.GetField("Pos").str)
                                    );
                            }

                        }

                        break;


                }
               
            }
            jSONOs.Clear();

        }
        /*
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("UP");
            JSONObject json = new JSONObject();


            json.AddField("ID", gameObject.name);
            json.AddField("pos", transform.position.ToString());
            json.AddField("num", 1231515656);
        }*/
    }


    public void onData_toServer(JSONObject Data)
    {

        

        tc.SckSSend(Data.ToString());


    }

    public void p(string s)
    {
       
    }


    private void OnApplicationQuit()
    {
        tc.Quit();
    }

}


public class Student
{
    public int ID { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public string Sex { get; set; }
}
class Program
{
    static void Main(string[] args)
    {
        List<Student> lstStuModel = new List<Student>()
            {
                new Student(){ID=1,Name="張飛",Age=250,Sex="男"},
                new Student(){ID=2,Name="潘金蓮",Age=300,Sex="女"}
            };

        //Newtonsoft.Json序列化
        string jsonData = JsonConvert.SerializeObject(lstStuModel);

        //Console.WriteLine(jsonData);

        //Newtonsoft.Json反序列化
        string json = @"{ 'Name':'C#','Age':'3000','ID':'1','Sex':'女'}";
        Student descJsonStu = JsonConvert.DeserializeObject<Student>(json);//反序列化
        //Console.WriteLine(string.Format("反序列化： ID={0},Name={1},Sex={2},Sex={3}", descJsonStu.ID, descJsonStu.Name, descJsonStu.Age, descJsonStu.Sex));
        //Console.ReadKey();
    }
}

