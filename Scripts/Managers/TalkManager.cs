using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerataData();
    }

    // 게임 시작시 대화 데이터를 저장
    void GenerataData()
    {
        talkData.Add(1, new string[]{"가방이 꽉찼습니다."});  // 인벤토리 초과 시
        talkData.Add(2, new string[]{"일기장을 얻었다."});  // 일기장 얻을 시

        talkData.Add(10, new string[]{"열쇠를 얻었다!"});    // 열쇠
        talkData.Add(20, new string[]{"잠겨있다."});    // 빨간 좌물쇠
        talkData.Add(30, new string[]{"이게 뭐야?", "2진수가 왜 여기서 나와?"});    // 2진수 바이너리
        talkData.Add(40, new string[]{"어느 방향을 가르키고 있다.", "이 방향은 뭐지? 힌트 같은건가?"});    // 2진수 방의 화살표

        talkData.Add(11, new string[]{"햄버거를 얻었다."}); // 햄버거
        talkData.Add(12, new string[]{"핫도그를 얻었다."}); // 핫도그
        talkData.Add(13, new string[]{"케이크를 얻었다."}); // 케이크
        talkData.Add(14, new string[]{"체리를 얻었다."}); // 체리
        talkData.Add(15, new string[]{"수박을 얻었다."}); // 수박
        talkData.Add(16, new string[]{"바나나를 얻었다."}); // 바나나

        talkData.Add(41, new string[]{"헉..!?", "뭐야!"});   // 유령을 보고 하는 소리

        talkData.Add(100, new string[]{"잠겨있다."});    // 2진수방 문
        talkData.Add(200, new string[]{"잠겨있다."});    // 키패드 문

        talkData.Add(101, new string[]{"컴퓨터가 있다", "자세히 보니 뭐라고 써져있다."});    // 1번째 방 모니터
        talkData.Add(102, new string[]{"컴퓨터에 어떤 화면이 나와있다."});    // 3번째 방 모니터
        talkData.Add(103, new string[]{"수상한 종이를 얻었다."});    // 문제 종이
        talkData.Add(104, new string[]{"대놓고 이 컴퓨터만 켜놓다니..", "뭔지 봐볼까?!"});    // osi 모니터
        talkData.Add(105, new string[]{"후.. 몸이 점점 힘들다.", "이게 제발 마지막이길.."});   // 강당 모니터
        talkData.Add(106, new string[]{"빔 프로젝트?", "어라! 켜졌다."}); // 빔 프로젝터

        // 몇몇 구간에 대한 대화
        talkData.Add(1000, new string[]{"..?", "여긴 어디지?", "분명 집에서 자고 있었는데..", "ㅅ..설마 납치?!",
                                        "나 인신매매 당하는거야?!", "o(TヘTo)...", "일단 주변을 확인해보자..!"});
        talkData.Add(1001, new string[]{"이 방은 뭐지?", "책상위에 무언가 써져있다"});
        talkData.Add(1002, new string[]{"여기엔 먹을 게 더 많군!!", "상하지는 않았겠지?", "다행히 죽게 내버려두지는 않나보다.", "날 가뒀지만 인간미는 있네"});
        talkData.Add(1003, new string[]{"여기가 마지막 방인가?", ".."});  // 강당 문 열고
    }

    public string GetTalk(int id, int talkIndex){
        if (GameManager.instance.hasDoorLock){  // 문이 잠기면
            talkData[20] = new string[]{"잠겨있다."};
        }
        else{
            talkData[20] = new string[]{"열렸다."};
        }

        if (talkIndex == talkData[id].Length){  // 다음 대화가 없으면
            return null;
        }
        else{                                   // 다음 대화가 있으면
            return talkData[id][talkIndex];
        }
    }
}
