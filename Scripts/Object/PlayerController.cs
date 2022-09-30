using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed; // 플레이어 이동 속도
    [SerializeField]
    private float runSpeed;          // 뛰었을 때
    private float applySpeed;        // 플레이어 현재 속도
    public float stamina_Max=100;    // 최대 스테미너
    public float stamina;            // 스테미너

    // 점프 힘 변수
    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isRun = false; // 뛰는 여부
    private bool hasRun = true; // 뛸 수 있는가?
    private bool isMove = false; // 움직이는가?
    private bool isGround = true; // 바닥에 있는지

    // 일시 정지 변수
    public bool IsPause = false;

    // 카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    // 물체를 바라보는 최대 거리
    [SerializeField]
    private float MaxDistance;

    // 카메라 회전 한계
    [SerializeField]
    private float cameraRotationLimit; // Y 각도 제한
    private float currentCameraRotationX = 0f;
    //private float currentCameraRotationY = 0f;

    // 플레이어 정지 변수
    public bool hasStop = true;
    public bool hasStart = false;   // 게임 시작하면 제약된게 풀리게 해주는 변수

    // 아이템 변수
    public GameObject[] item;
    int itemValue = 0;  // 아이템 먹은 수
    int slotIndex = 0;  // 선택한 슬롯 번호

    // 슬롯 확인 변수
    bool SlotDown1;
    bool SlotDown2;
    bool SlotDown3;
    bool SlotDown4;
    bool SlotDown5;
    bool SlotDown6;
    bool SlotDown7;

    bool hasItem = false;   // 아이템이 존재 하는지 확인용 변수

    // 사운드 변수
    AudioSource audioWorking;

    // GameObject nearObject = null;  // Trigger에 접촉한 Object 확인
    GameObject flashLight;  // 손전등

    [SerializeField]
    private Camera theCamera;

    private Rigidbody playerRigid;
    private CapsuleCollider capsuleCollider;
    private RaycastHit hit;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioWorking = GetComponent<AudioSource>();
        applySpeed = walkSpeed;
        GameManager.instance.PlayerStop(true);
    }

    // 물리적인 충돌 현상 및 에러를 제어
    void FixedUpdate() {
        FreezeRotation();
    }

    // Object 충돌에 의한 회전 방지
    void FreezeRotation(){
        playerRigid.angularVelocity = Vector3.zero;
    }

    void Update()
    {
//  [ 플레이어 동작 감지 ]------------------------------------------------
        if (hasStop){
            IsGround();             // 바닥 닿음 여부 확인 (점프 여부 확인)
            TryJump();              // 점프
            TryRun();               // 뛰기/걷기
            Move();                 // 플레이어 움직임
            CameraRotation();       // 카메라 위아래 방향
            CharacterRotation();    // 카메라 왼쪽오른쪽 방향
//  ---------------------------------------------------------------------
            RayCast();              // 바라보는 Object 감지
            ItemEvent();            // 아이템 처리
            GetSlot();              // Slot의 키 확인
            ItemSwap();             // 아이템 선택
            Interation();           // 아이템 줍기
        }
        if (hasStart){
            GameManager.instance.AnyKey();  // 아무키 입력시 호출된 인터페이스 종료
            Pause();                        // 게임 일시 정지
            if (!GameManager.instance.gameEnd)
                GameManager.instance.TimeUI();  // 타이머
        }
        
    }

    // 게임 일시 정지
    private void Pause(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!IsPause){
                GameManager.instance.MouseClip(false);
                GameManager.instance.GameStop(true);
                Time.timeScale = 0;
            }
            else{
                GameManager.instance.MouseClip(true);
                GameManager.instance.GameStop(false);
                GameManager.instance.Save(false);
                Time.timeScale = 1;
            }
            IsPause = !IsPause;
        }
    }

    // 1~5번 키 입력 확인
    void GetSlot(){
        SlotDown1 = Input.GetButtonDown("Slot1");
        SlotDown2 = Input.GetButtonDown("Slot2");
        SlotDown3 = Input.GetButtonDown("Slot3");
        SlotDown4 = Input.GetButtonDown("Slot4");
        SlotDown5 = Input.GetButtonDown("Slot5");
        SlotDown6 = Input.GetButtonDown("Slot6");
        SlotDown7 = Input.GetButtonDown("Slot7");
    }

    // 1~7번 아이템 선택 확인
    void ItemSwap(){
        if (SlotDown1) slotIndex = 0;
        if (SlotDown2) slotIndex = 1;
        if (SlotDown3) slotIndex = 2;
        if (SlotDown4) slotIndex = 3;
        if (SlotDown5) slotIndex = 4;
        if (SlotDown6) slotIndex = 5;
        if (SlotDown7) slotIndex = 6;

        if ( SlotDown1 || SlotDown2 || SlotDown3 || SlotDown4 || SlotDown5 || SlotDown6 || SlotDown7 ){
            if (item[slotIndex] !=  null) {
                Item itemInfo = item[slotIndex].GetComponent<Item>();
                GameManager.instance.PickItem(itemInfo.itemName, slotIndex);
                Debug.Log("아이템 선택 : " + slotIndex);
            }
            else{
                GameManager.instance.PickItem(slotIndex);
            }
        }
    }

    // 아이템 줍기
    void Interation(){
        // if (hasItem && nearObject != null){
        if (hasItem){
            int itemLocation = 0;               // 아이템 슬롯 공간 확인

            // 슬롯 공간 찾기
            for(int i=0; i<item.Length; i++){
                if (item[i] == null){
                    itemLocation = i;
                    break;
                }
            }

            // 슬롯에 아이템 넣기
            if (itemValue < item.Length && item[itemLocation] == null){
                // item[itemLocation] = nearObject.gameObject;
                item[itemLocation] = hit.collider.gameObject;
                Item handItem = item[itemLocation].GetComponent<Item>();
                GameManager.instance.ItemAdd(handItem.itemImage, itemLocation);
                GameManager.instance.PickItem(handItem.itemName, itemLocation);
                item[itemLocation].gameObject.SetActive(false);

                slotIndex = itemLocation;   // 인벤토리 위치에 해당 아이템 선택시키기

                handItem.hasDoor = false;
                itemValue++;       // 아이템이 슬롯에 차지한 공간
                // nearObject = null; // 아이템을 먹으면 Trigger에서 받은 nearObject는 지워야한다. (error)
            }
            else{
                Debug.Log("가방 꽉참!");
                GameManager.instance.Action();  // 가방 초과 시 문구 출력
            }

            Debug.Log("itmeLocation : "+itemLocation);
            Debug.Log("itme.length : "+item.Length);
            Debug.Log("itmeValue : "+itemValue);

            hasItem = false;
        }
    }

    // 전방 Object 확인
    private void RayCast(){
        Debug.DrawRay(theCamera.transform.position, theCamera.transform.forward * MaxDistance, Color.red);
        if (Input.GetMouseButtonDown(1)){
            if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, MaxDistance)){
                if (hit.collider.gameObject != null){
                    CheckTag(hit.collider.tag);
                    if (hit.collider.tag != "Problem")
                        GameManager.instance.Action(hit.collider.gameObject);
                        
                    Debug.Log("Raycast : " + hit.collider.tag);                    
                }
            }
            else{
                if (item[slotIndex] != null){
                    ItemUse();  // 아이템 사용 여부 
                    Debug.Log("ItemUse()");
                }
            }
        }
    }

    // Object 이벤트 처리
    private void CheckTag(string tagName){
        if (tagName == "FlashLight"){
            hit.collider.gameObject.SetActive(false);
        }
        if (tagName == "Item" || tagName == "Key"){
            hasItem = true;
        }
        if (tagName == "Keypad"){
            KeyPad Keypad = hit.collider.GetComponent<KeyPad>();
            Keypad.KeyPadUI();
            hasStop = false;
        }
        if (tagName == "Door"){
            DoorController door = hit.collider.GetComponent<DoorController>();
            door.doorEvent();
        }
        if (tagName == "Lock"){
            if (item[slotIndex] != null){
                if (item[slotIndex].tag == "Key"){
                    Lock doorLock = hit.collider.GetComponent<Lock>();
                    Item itemUse = item[slotIndex].GetComponent<Item>();
                    if (doorLock.KeyCheck(itemUse.value)){ // 잠금이 풀렸는가?
                        ItemClose(false);
                    }
                } else Debug.Log("키가 아니거나 맞지 않습니다.");
            } else Debug.Log("들고 있는 아이템이 없습니다.");
        }
        if (tagName == "Problem"){
            ProblemManager problmeObj = hit.collider.GetComponent<ProblemManager>();
            problmeObj.Mission();
        }
        if (tagName == "FilmProjector"){
            FilmProjectorScript LastObj = hit.collider.GetComponent<FilmProjectorScript>();
            LastObj.LastMission();
        }
    }

    // 아이템 사용
    private void ItemUse(){
        // if (item[slotIndex] != null && nearObject == null){
        if (item[slotIndex] != null){
            Item itemNum = item[slotIndex].GetComponent<Item>();
            if (itemNum.value == 10){   // 음식일 때
                Food foodStamina = item[slotIndex].GetComponent<Food>();
                stamina_Max += foodStamina.staminaGauge;
                if (stamina_Max > 100) stamina_Max = 100;
                ItemClose(false);
            }
            if (itemNum.value == 11){   // 일기장일 때
                Diary diary = item[slotIndex].GetComponent<Diary>();
                diary.DiaryOpen();
            }
            if (itemNum.value == 12){   // 문제 종이일 때
                PaperMission paperMission = item[slotIndex].GetComponent<PaperMission>();
                paperMission.CallObject();
            }
        }
    }
    
    // 아이템 메소드
    private void ItemEvent(){
        // // 아이템 버리기
        if (Input.GetKeyDown(KeyCode.F)){
            if (item[slotIndex] != null){
                ItemClose(true);
            } else Debug.Log("버릴 아이템이 없습니다.");
        }
    }

    // 아이템 버릴 위치
    private void ItemClose(bool hasRealization){
        Vector3 PlayerPosition = theCamera.transform.position + theCamera.transform.forward*1f;
        item[slotIndex].transform.position = PlayerPosition;
        if (hasRealization == true){
            item[slotIndex].SetActive(true);
        }

        item[slotIndex] = null;
        GameManager.instance.itemSlotImage[slotIndex].sprite = null;
        GameManager.instance.ItemRemove();

        itemValue--;
    }

    // Trigger colider와 접촉이 중인지
    private void OnTriggerStay(Collider other) {
        // if (other.tag == "Item" || other.tag == "Key"){
        //     nearObject = other.gameObject;
        // }
        if (other.tag == "Speak"){
            GameManager.instance.Action(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    // Trigger colider와 접촉이 끝났는지
    private void OnTriggerExit(Collider other) {
        // if (other.tag == "Item" || other.tag == "Key"){
        //     nearObject = null;
        // }
    }

    // 땅 착지 여부 (점프 중첩 방지)
    private void IsGround(){
        // 땅 착지 여부를 확인한다.
        // Physics.Raycast는 레이저를 쏴서 오브젝트를 확인
        // (쏠 위치, 방향, 레이저길이)
        // 방향에서 -transform.position을 쓰면 안되고 Vecotr3.down을 씀
        // capsuleCollider.bounds.extents.y는 
        // 캐릭터 오브젝트(capsuleCollider.bounds)의 반절(extents)에 y 크기를 가져온다.
        // 레이저 길이에 여유가 있도록 0.1f를 곱한다.
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    // 점프
    private void TryJump(){
        if (Input.GetKeyDown(KeyCode.Space) && isGround){
            playerRigid.velocity = transform.up * jumpForce;
        }
    }

    // 뛰기 제어
    private void TryRun(){
        // 쉬프트를 누르면 움직일 때 뜀과 동시에 스테미나 감소
        // 스테미나 전부 감소되면 걷기로 바꿔야됨
        // 이때 쉬프트를 누르고 있을 동안에 스테미나가 올라간다고 다시 뛰면 안됨.
        if (Input.GetKey(KeyCode.LeftShift)){
            if (isMove){
                Running();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)){
            RunningCancel();
            hasRun = true;
        }

        if (GameManager.instance.second % 10 == 0){ // 10초 씩 지나면 스테미나 감소
            if (stamina_Max >= 0)
                stamina_Max -= (float)0.05;
            else
                stamina_Max = 0;
        }

        if (isRun){
            if (stamina < 0){
                stamina = 0;
                hasRun = false;
            }
            else stamina = stamina - Time.deltaTime * 20;
        }
        else{
            if (stamina > stamina_Max-1) stamina = stamina_Max;
            else stamina = stamina + Time.deltaTime * 10;
        }

        GameManager.instance.Stamina(stamina*(float)(0.01));
    }
    
    // 달리기
    private void Running(){
        if (hasRun){
            isRun = true;
            applySpeed = runSpeed;
        }
        else RunningCancel();
    }

    // 걷기
    private void RunningCancel(){
        isRun = false;
        applySpeed = walkSpeed;
    }

    // 동작
    private void Move(){
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xInput;
        Vector3 moveVertical = transform.forward * zInput;

        Vector3 newVelocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        playerRigid.MovePosition(transform.position + newVelocity * Time.deltaTime);

        if (xInput != 0 || zInput != 0){
            isMove = true;
            if (!audioWorking.isPlaying){
                audioWorking.Play();
            }
        }
        else{
            isMove = false;
            audioWorking.Stop();
        }
    }

    // 좌우 회전
    private void CharacterRotation(){
        // 좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        
        playerRigid.MoveRotation(playerRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 카메라 회전
    private void CameraRotation(){
        // 상하 카메라 회전
        // Mouse Y를 주는데 왜 변수 이름을 xRotation으로 했냐면
        // 마우스는 3차원이 아니기 때문에 x와 y값만 받는다.
        // 우리가 만드는건 3차원이기 때문에 유니티 씬 화면에서 회전하는 방향을 확인해 보면
        // 마우스를 Y방향으로 움직이면 x값이 바뀌는 것을 확인할 수 있다.
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; // 천천히 움직이도록
        
        currentCameraRotationX -= _cameraRotationX;
        // -45 ~ 45도로 Y값을 볼 수 있도록 고정
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
