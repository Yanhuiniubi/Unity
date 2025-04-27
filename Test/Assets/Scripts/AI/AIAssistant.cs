using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/**
 * �ǻ���֪��ģ�� WebAPI �ӿڵ���ʾ�� �ӿ��ĵ����ؿ�����https://www.xfyun.cn/doc/spark/Web.html
 * ���������ӣ�https://www.xfyun.cn/doc/spark/%E6%8E%A5%E5%8F%A3%E8%AF%B4%E6%98%8E.html ��code���ش�����ʱ�ؿ���
 * @author iflytek
 */
public class AIAssistant
{
    public static AIAssistant Inst = new AIAssistant();
    static ClientWebSocket webSocket0;
    static CancellationToken cancellation;
    // Ӧ��APPID������Ϊwebapi����Ӧ�ã�����ͨ�ǻ���֪��ģ����Ȩ��
    const string x_appid = "23058bf7";
    // �ӿ���Կ��webapi����Ӧ�ÿ�ͨ�ǻ���֪��ģ�ͺ󣬿���̨--�ҵ�Ӧ��---�ǻ���֪��ģ��---��Ӧ�����apikey��
    const string api_secret = "MjRhNGU5MDI4MTM5ZTZkNGM1MjA5YTZi";
    // �ӿ���Կ��webapi����Ӧ�ÿ�ͨ�ǻ���֪��ģ�ͺ󣬿���̨--�ҵ�Ӧ��---�ǻ���֪��ģ��---��Ӧ�����apisecret��
    const string api_key = "d710e8ba9afe79c5301b57c72b1b7822";

    static string hostUrl = "wss://spark-api.xf-yun.com/v4.0/chat";

    async public void SendQuestion(List<Content> con, string question,float temperature = 0.5f)
    {
        string authUrl = GetAuthUrl();
        string url = authUrl.Replace("http://", "ws://").Replace("https://", "wss://");
        using (webSocket0 = new ClientWebSocket())
        {
            try
            {
                await webSocket0.ConnectAsync(new Uri(url), cancellation);

                JsonRequest request = new JsonRequest();
                request.header = new Header()
                {
                    app_id = x_appid,
                    uid = "12345"
                };
                request.parameter = new Parameter()
                {
                    chat = new Chat()
                    {
                        domain = "4.0Ultra",//ģ������Ĭ��Ϊ�ǻ�ͨ�ô�ģ��
                        temperature = temperature,//�¶Ȳ�����ֵ�����ڿ����������ݵ�����ԺͶ����ԣ�ֵԽ�������Խ�ߣ���Χ��0��1��
                        max_tokens = 1024,//�������ݵ���󳤶ȣ���Χ��0��4096��
                    }
                };
                request.payload = new Payload()
                {
                    message = new Message()
                    {
                        text = con,
                        //text = new List<Content>
                        //{
                        //     new Content() { role = role, content = question },
                        //     // new Content() { role = "assistant", content = "....." }, // AI����ʷ�ش���������ʡ���˾������ݣ����Ը�����Ҫ��Ӹ�����ʷ�Ի���Ϣ��������������ݡ�
                        //}
                    }
                };
                AIChatData.Inst.RecvData(true, question, false);
                string jsonString = JsonConvert.SerializeObject(request);
                //���ӳɹ�����ʼ��������


                var frameData2 = System.Text.Encoding.UTF8.GetBytes(jsonString.ToString());


                webSocket0.SendAsync(new ArraySegment<byte>(frameData2), WebSocketMessageType.Text, true, cancellation);

                // ������ʽ���ؽ�����н���
                byte[] receiveBuffer = new byte[1024];
                WebSocketReceiveResult result = await webSocket0.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellation);
                StringBuilder resp = new StringBuilder();
                bool isUpdate = false;
                while (!result.CloseStatus.HasValue)
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                        //���������Ϊjson

                        JObject jsonObj = JObject.Parse(receivedMessage);
                        int code = (int)jsonObj["header"]["code"];


                        if (0 == code)
                        {
                            int status = (int)jsonObj["payload"]["choices"]["status"];


                            JArray textArray = (JArray)jsonObj["payload"]["choices"]["text"];
                            string content = (string)textArray[0]["content"];
                            resp.Append(content);
                            if (status != 2)
                            {
                                Debug.Log($"�ѽ��յ����ݣ� {receivedMessage}");
                            }
                            else
                            {
                                Debug.Log($"���һ֡�� {receivedMessage}");
                                int totalTokens = (int)jsonObj["payload"]["usage"]["text"]["total_tokens"];
                                Debug.Log($"���巵�ؽ���� {resp}");
                                Debug.Log($"��������token���� {totalTokens}");
                                AIChatData.Inst.RecvData(false, resp.ToString(), isUpdate);
                                AIChatEvent.OnResponseEnd?.Invoke();
                                break;
                            }
                            AIChatData.Inst.RecvData(false, resp.ToString(), isUpdate);
                            isUpdate = true;
                        }
                        else
                        {
                            Debug.Log($"���󱨴� {receivedMessage}");
                        }


                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Debug.Log("�ѹر�WebSocket����");
                        break;
                    }

                    result = await webSocket0.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellation);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        
    }
    // ����codeΪ������ʱ�����ѯhttps://www.xfyun.cn/document/error-code�������
    string GetAuthUrl()
    {
        string date = DateTime.UtcNow.ToString("r");

        Uri uri = new Uri(hostUrl);
        StringBuilder builder = new StringBuilder("host: ").Append(uri.Host).Append("\n").//
                                Append("date: ").Append(date).Append("\n").//
                                Append("GET ").Append(uri.LocalPath).Append(" HTTP/1.1");

        string sha = HMACsha256(api_secret, builder.ToString());
        string authorization = string.Format("api_key=\"{0}\", algorithm=\"{1}\", headers=\"{2}\", signature=\"{3}\"", api_key, "hmac-sha256", "host date request-line", sha);
        //System.Web.HttpUtility.UrlEncode

        string NewUrl = "https://" + uri.Host + uri.LocalPath;

        string path1 = "authorization" + "=" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authorization));
        date = date.Replace(" ", "%20").Replace(":", "%3A").Replace(",", "%2C");
        string path2 = "date" + "=" + date;
        string path3 = "host" + "=" + uri.Host;

        NewUrl = NewUrl + "?" + path1 + "&" + path2 + "&" + path3;
        return NewUrl;
    }




    public string HMACsha256(string apiSecretIsKey, string buider)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(apiSecretIsKey);
        System.Security.Cryptography.HMACSHA256 hMACSHA256 = new System.Security.Cryptography.HMACSHA256(bytes);
        byte[] date = System.Text.Encoding.UTF8.GetBytes(buider);
        date = hMACSHA256.ComputeHash(date);
        hMACSHA256.Clear();

        return Convert.ToBase64String(date);

    }
    public void Disconnect()
    {
        webSocket0.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
    }
}



//����������
public class JsonRequest
{
    public Header header { get; set; }
    public Parameter parameter { get; set; }
    public Payload payload { get; set; }
}

public class Header
{
    public string app_id { get; set; }
    public string uid { get; set; }
}

public class Parameter
{
    public Chat chat { get; set; }
}

public class Chat
{
    public string domain { get; set; }
    public double temperature { get; set; }
    public int max_tokens { get; set; }
    public bool show_ref_label { get; set; }
}

public class Payload
{
    public Message message { get; set; }
}

public class Message
{
    public List<Content> text { get; set; }
}

[Serializable]
public class Content
{
    public string role;
    public string content;
}