using System.Collections;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    [SerializeField] private float _delay;
    private WaitForSeconds _wait;
    private Coroutine _routine;
    // 객체를 캐싱해두고 재활용하자
    // 메모리를 신경쓰며 할당한다

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        Debug.Log("Start 시작");

        Debug.Log("Start 종료");
    }

    private void OnDisable()
    {
        _routine = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Run();
        if (Input.GetKeyDown(KeyCode.Alpha2)) Stop();
    }

    private void Run()
    {
        if (_routine != null) return;

        _routine = StartCoroutine(MyRoutine());
    }

    private void Stop()
    {
        if (_routine == null) return;

        StopCoroutine(_routine);
        _routine = null;
    }

    // 함수의 반환형은 'IEnumerator'
    private IEnumerator MyRoutine()
    {
        while (true)
        {
            // Delegate 괄호 안에 들어온 조건이 참이 될 때까지 기다린다
            yield return _wait;
            Debug.Log("Coroutine");
            yield return _wait;
            Debug.Log("Coroutine2");    
        }

        // 반환할 때는 'yield return'
        // yield return 000 : 000이 충족되는 상황까지 함수를 종료하고 대기할 것.
        // yield return null; // 매 프레임 실행 (자주 사용 안 됨, 이런 상황에서는 Update를 사용)

        // 루틴을 아예 멈출 때
        // yield break; // Coroutine에서 return 같은 역할
    }
}
